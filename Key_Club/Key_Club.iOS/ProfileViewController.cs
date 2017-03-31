using Foundation;
using System;
using UIKit;

namespace Key_Club.iOS
{
    public partial class ProfileViewController : UIViewController
    {
        public ProfileViewController (IntPtr handle) : base (handle)
        {

        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ProfileNameLabel.Text = "Hi, " + UserInfo.name;
			ProfileClubLabel.Text = "Club: " + UserInfo.club;
			ProfilePositionLabel.Text = "Position: " + UserInfo.role;

			ProfileRefreshButton.Clicked += RefreshProfile;
			ProfileSignOutButton.TouchUpInside += SignOut;
		}

		private async void SignOut(object sender, EventArgs e)
		{
			NSUserDefaults.StandardUserDefaults.SetString("", "UserName");
			NSUserDefaults.StandardUserDefaults.SetString("", "UserClub");
			NSUserDefaults.StandardUserDefaults.SetString("", "UserRole");

			PresentViewController(Storyboard.InstantiateViewController("LogInViewController"), true, null);
		}

		private async void RefreshProfile(object sender, EventArgs e)
		{
			await DynamoDbService.getUserInfo(UserInfo.club, UserInfo.name);

			NSUserDefaults.StandardUserDefaults.SetString(UserInfo.role, "UserRole");

			ProfileNameLabel.Text = "Hi, " + UserInfo.name;
			ProfileClubLabel.Text = "Club: " + UserInfo.club;
			ProfilePositionLabel.Text = "Position: " + UserInfo.role;
		}
    }
}