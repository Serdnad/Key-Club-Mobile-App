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
    [Register ("LogInViewController")]
    partial class LogInViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LoginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView LogInClubPicker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LogInErrorLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LogInName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LoginButton != null) {
                LoginButton.Dispose ();
                LoginButton = null;
            }

            if (LogInClubPicker != null) {
                LogInClubPicker.Dispose ();
                LogInClubPicker = null;
            }

            if (LogInErrorLabel != null) {
                LogInErrorLabel.Dispose ();
                LogInErrorLabel = null;
            }

            if (LogInName != null) {
                LogInName.Dispose ();
                LogInName = null;
            }
        }
    }
}