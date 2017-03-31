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
    [Register ("ProfileViewController")]
    partial class ProfileViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProfileClubLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProfileNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProfilePositionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem ProfileRefreshButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ProfileSignOutButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ProfileClubLabel != null) {
                ProfileClubLabel.Dispose ();
                ProfileClubLabel = null;
            }

            if (ProfileNameLabel != null) {
                ProfileNameLabel.Dispose ();
                ProfileNameLabel = null;
            }

            if (ProfilePositionLabel != null) {
                ProfilePositionLabel.Dispose ();
                ProfilePositionLabel = null;
            }

            if (ProfileRefreshButton != null) {
                ProfileRefreshButton.Dispose ();
                ProfileRefreshButton = null;
            }

            if (ProfileSignOutButton != null) {
                ProfileSignOutButton.Dispose ();
                ProfileSignOutButton = null;
            }
        }
    }
}