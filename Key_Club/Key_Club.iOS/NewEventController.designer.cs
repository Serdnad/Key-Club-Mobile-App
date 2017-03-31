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
    [Register ("NewEventController")]
    partial class NewEventController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ErrorLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker NewEventDatePicker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView NewEventDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NewEventLink { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NewEventLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker NewEventTimeEnd { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker NewEventTimeStart { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NewEventTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ErrorLabel != null) {
                ErrorLabel.Dispose ();
                ErrorLabel = null;
            }

            if (NewEventDatePicker != null) {
                NewEventDatePicker.Dispose ();
                NewEventDatePicker = null;
            }

            if (NewEventDescription != null) {
                NewEventDescription.Dispose ();
                NewEventDescription = null;
            }

            if (NewEventLink != null) {
                NewEventLink.Dispose ();
                NewEventLink = null;
            }

            if (NewEventLocation != null) {
                NewEventLocation.Dispose ();
                NewEventLocation = null;
            }

            if (NewEventTimeEnd != null) {
                NewEventTimeEnd.Dispose ();
                NewEventTimeEnd = null;
            }

            if (NewEventTimeStart != null) {
                NewEventTimeStart.Dispose ();
                NewEventTimeStart = null;
            }

            if (NewEventTitle != null) {
                NewEventTitle.Dispose ();
                NewEventTitle = null;
            }
        }
    }
}