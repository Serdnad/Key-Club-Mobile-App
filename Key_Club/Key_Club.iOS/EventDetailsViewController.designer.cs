// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Key_Club.iOS
{
    [Register ("EventDetailsViewController")]
    partial class EventDetailsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DetailsAddToCalendarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationBar DetailsDateBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView DetailsDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DetailsLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DetailsSignUpButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DetailsTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DetailsTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DetailsAddToCalendarButton != null) {
                DetailsAddToCalendarButton.Dispose ();
                DetailsAddToCalendarButton = null;
            }

            if (DetailsDateBar != null) {
                DetailsDateBar.Dispose ();
                DetailsDateBar = null;
            }

            if (DetailsDescription != null) {
                DetailsDescription.Dispose ();
                DetailsDescription = null;
            }

            if (DetailsLocation != null) {
                DetailsLocation.Dispose ();
                DetailsLocation = null;
            }

            if (DetailsSignUpButton != null) {
                DetailsSignUpButton.Dispose ();
                DetailsSignUpButton = null;
            }

            if (DetailsTime != null) {
                DetailsTime.Dispose ();
                DetailsTime = null;
            }

            if (DetailsTitle != null) {
                DetailsTitle.Dispose ();
                DetailsTitle = null;
            }
        }
    }
}