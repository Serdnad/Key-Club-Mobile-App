using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace Key_Club.Droid
{
    public class UserProfile : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.userProfile, container, false);
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Visibility = ViewStates.Gone;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Visibility = ViewStates.Visible;

            view.FindViewById<TextView>(Resource.Id.profileName).Text = "Hi, " + UserInfo.name;
            view.FindViewById<TextView>(Resource.Id.profileClub).Text = "Club: " + UserInfo.club;
            view.FindViewById<TextView>(Resource.Id.profileRole).Text = "Position: " + UserInfo.role;

            view.FindViewById<Button>(Resource.Id.signOutButton).Click += SignOutUser;

            return view;
        }

        private void SignOutUser(object sender, EventArgs e)
        {
            UserInfo.resetInfo();

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.Activity);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("userName", "EMPTY");
            editor.PutString("userClub", "EMPTY");
            editor.PutString("userRole", "EMPTY");
            editor.Apply();        // applies changes asynchronously (editor.Commit() for synchronous)

            StartActivity(new Intent(this.Activity, typeof(Splash)));
            this.Activity.Finish();
        }

        private async void RefreshProfile(object sender, EventArgs e)
        {
            await DynamoDbService.getUserInfo(UserInfo.club, UserInfo.name); //Get user info and save it to device
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.Activity);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("userName", UserInfo.name);
            editor.PutString("userClub", UserInfo.club);
            editor.PutString("userRole", UserInfo.role);
            editor.Apply();        // applies changes asynchronously (editor.Commit() for synchronous)
        }

        public override void OnResume()
        {
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click += RefreshProfile;
            base.OnResume();
        }

        public override void OnStop()
        {
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click -= RefreshProfile;
            base.OnStop();
        }
    }
}