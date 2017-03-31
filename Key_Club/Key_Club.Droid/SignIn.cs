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
    [Activity(Label = "Key Club", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/Theme.MainTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait)]

    public class SignIn : AppCompatActivity
    {
        List<string> clubList = new List<string>();
        EditText nameField;
        string club = "";

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.signIn);

            nameField = FindViewById<EditText>(Resource.Id.signInName);

            Spinner clubSpinner = FindViewById<Spinner>(Resource.Id.clubSpinner);
            clubSpinner.ItemSelected += ClubSpinner_ItemSelected;
            clubList = new List<string>(ClubInfo.clubs);
            clubList.Sort();
            clubList.Insert(0, "Select your club");
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, clubList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            clubSpinner.Adapter = adapter;

            FindViewById<Button>(Resource.Id.signInButton).Click += SignIn_Click;
        }

        private async void SignIn_Click(object sender, EventArgs e)
        {
            try
            {
                await DynamoDbService.getUserInfo(club, nameField.Text); //get user info and save it to device
                ISharedPreferences prefss = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editor = prefss.Edit();
                editor.PutString("userName", UserInfo.name);
                editor.PutString("userClub", UserInfo.club);
                editor.PutString("userRole", UserInfo.role);
                editor.Apply();        // applies changes asynchronously (editor.Commit() for synchronous)

                StartActivity(typeof(MainActivity));
                DynamoDbService.getAnnouncements();
                DynamoDbService.getEvents();

                Finish();
            }
            catch(System.Exception exception)
            {
                Toast.MakeText(this, "User does not exist", ToastLength.Short).Show();
            }
        }

        private void ClubSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            club = clubList[e.Position];
        }
    }
}


