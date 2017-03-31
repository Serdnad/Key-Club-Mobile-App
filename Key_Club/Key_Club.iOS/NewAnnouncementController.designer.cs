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
    [Register ("NewAnnouncementController")]
    partial class NewAnnouncementController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AnnErrorLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView NewAnnDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NewAnnImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView NewAnnImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NewAnnTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AnnErrorLabel != null) {
                AnnErrorLabel.Dispose ();
                AnnErrorLabel = null;
            }

            if (NewAnnDescription != null) {
                NewAnnDescription.Dispose ();
                NewAnnDescription = null;
            }

            if (NewAnnImageButton != null) {
                NewAnnImageButton.Dispose ();
                NewAnnImageButton = null;
            }

            if (NewAnnImageView != null) {
                NewAnnImageView.Dispose ();
                NewAnnImageView = null;
            }

            if (NewAnnTitle != null) {
                NewAnnTitle.Dispose ();
                NewAnnTitle = null;
            }
        }
    }
}