using Foundation;
using System;
using UIKit;
using EventKitUI;
using EventKit;

namespace Key_Club.iOS
{
    public partial class EventDetailsViewController : UIViewController
    {
		public ServiceEvent e;
		protected CreateEventEditViewDelegate eventControllerDelegate;

        public EventDetailsViewController (IntPtr handle) : base (handle)
        {
			
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			DetailsDateBar.TopItem.Title = e.date.ToString("dddd MMM dd, yyyy");
			DetailsTitle.Text = e.title;
			DetailsDescription.Text = e.description;
			DetailsTime.Text = e.timeStart.ToShortTimeString() + " - " + e.timeEnd.ToShortTimeString();
			DetailsLocation.Text = e.location;
			DetailsAddToCalendarButton.TouchUpInside += AddToCalendar;

			if (e.signUpLink == "NONE")
				DetailsSignUpButton.Hidden = true;
			else
				DetailsSignUpButton.TouchUpInside += OpenLink;
		}

		public void OpenLink(object o, EventArgs eventArgs)
		{
			if (!e.signUpLink.StartsWith("http"))
			   e.signUpLink = "https://" + e.signUpLink;
			UIApplication.SharedApplication.OpenUrl(new NSUrl(e.signUpLink));
		}

		public void AddToCalendar(object o, EventArgs eventArgs)
		{
			App.Current.EventStore.RequestAccess(EventKit.EKEntityType.Event, (bool granted, NSError error) =>
			{
				if (granted)
				{
					InvokeOnMainThread(() =>
					{
						EKEventEditViewController eventController = new EventKitUI.EKEventEditViewController();
						eventController.EventStore = App.Current.EventStore;eventControllerDelegate = new CreateEventEditViewDelegate(eventController);
						eventController.EditViewDelegate = eventControllerDelegate;

						EKEvent newEvent = EKEvent.FromStore(App.Current.EventStore);
						// make the event start 20 minutes from now and last 30 minutes
						newEvent.StartDate = ConvertDateTimeToNSDate(e.timeStart);
						newEvent.EndDate = ConvertDateTimeToNSDate(e.timeEnd);
						newEvent.Title = e.title;
						newEvent.Notes = e.description;
						newEvent.Location = e.location;
						if(e.signUpLink != "NONE")
							newEvent.Url = new NSUrl(e.signUpLink);

						eventController.Event = newEvent;

						PresentViewController(eventController, true, null);
					});
				}
				else
					new UIAlertView("Access Denied", "User Denied Access to Calendar Data", null, "ok", null).Show();
			});

		}

		protected class CreateEventEditViewDelegate : EventKitUI.EKEventEditViewDelegate
		{
			protected EventKitUI.EKEventEditViewController eventController;

			public CreateEventEditViewDelegate(EventKitUI.EKEventEditViewController eventController)
			{
				this.eventController = eventController;
			}

			public override void Completed(EventKitUI.EKEventEditViewController controller, EKEventEditViewAction action)
			{
				eventController.DismissViewController(true, null);
			}
		}

		public NSDate ConvertDateTimeToNSDate(DateTime date)
		{
			DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate(
				(date - newDate).TotalSeconds);
		}
    }
}