using Foundation;
using System;
using UIKit;

namespace Key_Club.iOS
{
    public partial class EventListViewController : UIViewController
    {
        public EventListViewController (IntPtr handle) : base (handle)
        {
			Title = "Events";
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
    }
}