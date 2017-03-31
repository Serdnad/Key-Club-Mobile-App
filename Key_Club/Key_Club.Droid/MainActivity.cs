using System;
using Android.App;
using Android.Content;
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
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using System.Collections.Generic;
using Java.Lang;

namespace Key_Club.Droid
{
	[Activity (Label = "Key Club", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/Theme.MainTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait)]

    public class MainActivity : AppCompatActivity
	{
        private DrawerLayout mDrawerLayout;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);

            SupportActionBar ab = SupportActionBar;
            ab.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ab.SetDisplayHomeAsUpEnabled(true);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);

                //navigationView.LayoutParameters.Width = Resources.DisplayMetrics.WidthPixels - toolBar.LayoutParameters.Height;
            }

            setFragment(Resource.Id.nav_home);
            navigationView.Menu.GetItem(0).SetChecked(true);

            navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.navHeaderName).Text = "Hi, " + UserInfo.name;
        }

        public override void OnBackPressed()
        {
            //if (FragmentManager.BackStackEntryCount > 0)
            //{
            //    FragmentManager.PopBackStack();
            //}
            //else
            //{
            //    Finish();
            //}

            base.OnBackPressed();
        }

        private void setFragment(int viewID)
        {
            SupportFragment fragment = null;
            string title = "";

            switch (viewID)
            {
                case Resource.Id.nav_home:
                    fragment = new Home();
                    title = "Home";
                    break;
                case Resource.Id.nav_announcements:
                    fragment = new Announcements();
                    title = "Announcements";
                    break;
                case Resource.Id.nav_service:
                    fragment = new EventCalendar();
                    title = "Event Calendar";
                    break;
                case Resource.Id.nav_profile:
                    fragment = new UserProfile();
                    title = "My Profile";
                    break;
                //case Resource.Id.nav_settings:
                //    fragment = new Home();
                //    title = "Settings";
                //    break;
                case Resource.Id.nav_app_info:
                    fragment = new AppInfo();
                    title = "App Info";
                    break;
                default:
                    return;
            }

            // set the toolbar title
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            SupportActionBar.Title = title;

            var ft = this.SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.fragment_container, fragment);
            ft.CommitAllowingStateLoss();
        }

        private void SetUpDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                e.MenuItem.SetChecked(true);
                mDrawerLayout.CloseDrawers();
                setFragment(e.MenuItem.ItemId);
            };
        }
        
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    mDrawerLayout.OpenDrawer((int)GravityFlags.Left);
                    return true;

                default:
                    return true;
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}


