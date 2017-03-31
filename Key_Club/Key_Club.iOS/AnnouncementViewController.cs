using Foundation;
using System;
using UIKit;

namespace Key_Club.iOS
{
	public partial class AnnouncementViewController : UIViewController
    {
        public AnnouncementViewController (IntPtr handle) : base (handle)
        {
			
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (UserInfo.role == "Member")
				AddAnnouncementButton.Enabled = false;
		}
    }
}