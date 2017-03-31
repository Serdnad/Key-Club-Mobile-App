using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace Key_Club.iOS
{
	public partial class LogInViewController : UIViewController
	{
		public static List<string> clubList;

		public LogInViewController(IntPtr handle) : base(handle)
		{
			//await DynamoDbService.getClubList();
		}

		public override async void ViewDidLoad()
		{
			await DynamoDbService.getClubList();
			clubList = new List<string>(ClubInfo.clubs);
			clubList.Sort();

			LogInName.ShouldReturn += (textField) =>
			{
				textField.ResignFirstResponder();
				return true;
			};

			LoginButton.TouchUpInside += LogIn;
			LogInClubPicker.Model = new ClubPickerViewModel();

			LogInErrorLabel.TextColor = UIColor.Red;
			LogInErrorLabel.Hidden = true;

			base.ViewDidLoad();
		}

		public async void LogIn(object o, EventArgs e)
		{

			if (Reachability.IsNetworkAvailable() == false)
			{
				LogInErrorLabel.Text = "No Internet Connection";
				LogInErrorLabel.Hidden = false;

				return;
			}
			else if (LogInName.Text.Equals(""))
			{
				LogInErrorLabel.Text = "Name cannot be empty";
				LogInErrorLabel.Hidden = false;

				return;
			}

			try
			{
				await DynamoDbService.getUserInfo(clubList[(int)LogInClubPicker.SelectedRowInComponent(0)], LogInName.Text);

				NSUserDefaults.StandardUserDefaults.SetString(UserInfo.name, "UserName");
				NSUserDefaults.StandardUserDefaults.SetString(UserInfo.club, "UserClub");
				NSUserDefaults.StandardUserDefaults.SetString(UserInfo.role, "UserRole");

				DynamoDbService.getAnnouncements();
				DynamoDbService.getEvents();

				PresentViewController(Storyboard.InstantiateViewController("RootTabController"), false, null);
			}
			catch
			{
				LogInErrorLabel.Text = "No such user exists";
				LogInErrorLabel.Hidden = false;

				return;
			}
		}

		public class ClubPickerViewModel : UIPickerViewModel
		{
			public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
			{
				return clubList.Count;
			}

			public override string GetTitle(UIPickerView pickerView, nint row, nint component)
			{
				return clubList[(int)row];
			}

			public override nint GetComponentCount(UIPickerView pickerView)
			{
				return 1;
			}
		}

		public class ClubPickerViewDelegate : UIPickerViewDelegate
		{
			public override void Selected(UIPickerView pickerView, nint row, nint component)
			{
				base.Selected(pickerView, row, component);
			}
		}
	}
}