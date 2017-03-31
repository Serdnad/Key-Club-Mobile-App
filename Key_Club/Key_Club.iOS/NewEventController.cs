using Foundation;
using System;
using UIKit;

namespace Key_Club.iOS
{
    public partial class NewEventController : UIViewController
    {
        public NewEventController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			NewEventDescription.Layer.BorderWidth = (nfloat)0.5;
			ErrorLabel.TextColor = UIColor.Red;

			UIBarButtonItem uploadButton = new UIBarButtonItem();;
			uploadButton.Title = "Upload";
			uploadButton.Clicked += addEvent;
			NavigationItem.RightBarButtonItem = uploadButton;

			base.ViewDidLoad();
		}

		public void addEvent(object o, EventArgs eArgs)
		{
			if (Reachability.IsNetworkAvailable() == false)
			{
				ErrorLabel.Text = "No Internet Connection";
				ErrorLabel.Hidden = false;

				return;
			}
			else if (NewEventTitle.Text == "")
			{
				ErrorLabel.Text = "Title cannot be empty";
				ErrorLabel.Hidden = false;

				return;
			}
			else if (NewEventDescription.Text == "")
			{
				ErrorLabel.Text = "Description cannot be empty";
				ErrorLabel.Hidden = false;

				return;
			}
			else if (NewEventLocation.Text == "")
			{
				ErrorLabel.Text = "Location cannot be empty";
				ErrorLabel.Hidden = false;

				return;
			}

			ServiceEvent e = new ServiceEvent();
			e.title = NewEventTitle.Text;
			e.description = NewEventDescription.Text;
			e.location = NewEventLocation.Text;
			e.date = ToDateTime(NewEventDatePicker.Date);
			e.timeStart = ToDateTime(NewEventTimeStart.Date);
			e.timeEnd = ToDateTime(NewEventTimeEnd.Date);

			e.signUpLink = (NewEventLink.Text == "") ? "NONE" : NewEventLink.Text;

			DynamoDbService.uploadEvent(e);

			NavigationController.PopViewController(true);
		}

		public DateTime ToDateTime(NSDate date)
		{
			DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			var utcDateTime = reference.AddSeconds(date.SecondsSinceReferenceDate);
			var dateTime = utcDateTime.ToLocalTime();
			return dateTime;
		}
    }
}