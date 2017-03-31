using Foundation;
using System;
using UIKit;

namespace Key_Club.iOS
{
    public partial class CalendarViewController : UIViewController
    {
        public CalendarViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			CalendarRefreshButton.Clicked += RefreshCalendar;

			base.ViewDidLoad();
		}

		public async void RefreshCalendar(object o, EventArgs e)
		{
			await DynamoDbService.getEvents();

			CalendarController calendar = (CalendarController)ChildViewControllers[0];
			calendar.collectionView.ReloadData();
		}
    }
}