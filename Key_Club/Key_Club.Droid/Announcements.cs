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
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

namespace Key_Club.Droid
{
    public class Announcements : Android.Support.V4.App.Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;

        List<Announcement> announcements = new List<Announcement>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.Announcements, container, false);
            if (!UserInfo.role.ToLower().Equals("member") && !UserInfo.role.Equals(""))
            {
                this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Visibility = ViewStates.Visible;
                //this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Click += AddAnnouncement; ;
            }
            ImageButton refreshButton = this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton);
            refreshButton.Visibility = ViewStates.Visible;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Visibility = ViewStates.Visible;
            //this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click += RefreshAnnouncements;

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(this.Activity, LinearLayoutManager.Vertical, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            var adapter = new AnnouncementAdapter(ClubInfo.announcements);
            
            mRecyclerView.SetAdapter(adapter);

            return view;
        }

        private void AddAnnouncement(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            SupportFragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }

            ft.AddToBackStack(null);

            AnnouncementDialog newAnnDialog = new AnnouncementDialog();
            newAnnDialog.Show(ft, "dialog");
        }

        public async void RefreshAnnouncements(object sender, EventArgs e)
        {
            Toast.MakeText(this.Activity, "Refreshing announcements...", ToastLength.Short).Show();
            await DynamoDbService.getAnnouncements();
            var adapter = new AnnouncementAdapter(ClubInfo.announcements);
            mRecyclerView.SetAdapter(adapter);
        }

        public override void OnStop()
        {
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click -= RefreshAnnouncements;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Click -= AddAnnouncement;

            base.OnStop();
        }

        public override void OnResume()
        {
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click += RefreshAnnouncements;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Click += AddAnnouncement;

            base.OnStop();
        }
    }
}