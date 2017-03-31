using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;

namespace Key_Club.Droid
{
    [Activity(Label = "Key Club", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/Theme.SplashTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait)]

    public class Splash : AppCompatActivity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //await DynamoDbService.getUserInfo("Belen Jesuit", "Andres Gutierrez"); //Get user info and save it to device
            //ISharedPreferences prefss = PreferenceManager.GetDefaultSharedPreferences(this);
            //ISharedPreferencesEditor editor = prefss.Edit();
            //editor.PutString("userName", UserInfo.name);
            //editor.PutString("userClub", UserInfo.club);
            //editor.PutString("userRole", UserInfo.role);
            //editor.Apply();        // applies changes asynchronously (editor.Commit() for synchronous)

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            if (!prefs.GetString("userName", "EMPTY").Equals("EMPTY"))
            {
                UserInfo.name = prefs.GetString("userName", "EMPTY");
                UserInfo.club = prefs.GetString("userClub", "EMPTY");
                UserInfo.role = prefs.GetString("userRole", "EMPTY");

                StartActivity(typeof(MainActivity));
                DynamoDbService.getAnnouncements();
                DynamoDbService.getEvents();
            }
            else
            {
                await DynamoDbService.getClubList();
                StartActivity(typeof(SignIn));
            }


            Finish();
        }
    }
}


