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
using System.Globalization;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

namespace Key_Club.Droid
{
    public class DateEventList : SupportFragment, View.IOnTouchListener, GestureDetector.IOnGestureListener
    {
        private GestureDetector gestureDetector;

        DateTime date;
        TextView headerDate;
        RecyclerView mRecyclerView;
        DateEventListAdapter adapter;
        RecyclerView.LayoutManager mLayoutManager;
        TextView noEventsText;

        List<ServiceEvent> events = new List<ServiceEvent>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            date = DateTime.Parse(Arguments.GetString("date"));
            this.Activity.FindViewById<SupportToolbar>(Resource.Id.toolBar).Title = "Event Calendar";
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Visibility = ViewStates.Gone;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Visibility = ViewStates.Gone;

            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.DateEventsList, container, false);

            var prevDayButton = view.FindViewById<ImageButton>(Resource.Id.datePrevDay);
            prevDayButton.Click += PrevDayButton_Click;

            var nextDayButton = view.FindViewById<ImageButton>(Resource.Id.dateNextDay);
            nextDayButton.Click += NextDayButton_Click; ;

            headerDate = view.FindViewById<TextView>(Resource.Id.eventsDate);
            headerDate.Text = date.ToLongDateString();

            noEventsText = view.FindViewById<TextView>(Resource.Id.eventsEmptyText);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(this.Activity, LinearLayoutManager.Vertical, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            adapter = new DateEventListAdapter(date);
            adapter.ItemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(adapter);

            view.FindViewById<RecyclerView>(Resource.Id.recyclerView).SetOnTouchListener(this);
            gestureDetector = new GestureDetector(this);

            checkEventsExist();

            return view;
        }

        private void checkEventsExist()
        {
            if(adapter.events.Count == 0)
            {
                mRecyclerView.Visibility = ViewStates.Gone;
                noEventsText.Visibility = ViewStates.Visible;
            }
            else
            {
                mRecyclerView.Visibility = ViewStates.Visible;
                noEventsText.Visibility = ViewStates.Gone;
            }
        }

        private void Adapter_ItemClick(object sender, ServiceEvent e)
        {
            EventDetails eventDetails = new EventDetails();
            Bundle args = new Bundle();
            args.PutString("title", e.title);
            args.PutString("date", e.date.ToLongDateString());
            args.PutString("description", e.description);
            args.PutString("link", e.signUpLink);
            args.PutString("location", e.location);
            args.PutString("startTime", e.timeStart.ToShortTimeString());
            args.PutString("endTime", e.timeEnd.ToShortTimeString());
            args.PutString("time", e.timeStart.ToShortTimeString() + " - " + e.timeEnd.ToShortTimeString());
            eventDetails.Arguments = args;

            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.fragment_container, eventDetails);
            ft.AddToBackStack(null);
            ft.CommitAllowingStateLoss();
        }

        private void NextDayButton_Click(object sender, EventArgs e)
        {
            date = date.AddDays(1);
            Arguments.PutString("date", date.ToString());
            adapter = new DateEventListAdapter(date);
            mRecyclerView.SetAdapter(adapter);
            adapter.ItemClick += Adapter_ItemClick;
            headerDate.Text = date.ToLongDateString();
            checkEventsExist();
        }

        private void PrevDayButton_Click(object sender, EventArgs e)
        {
            date = date.AddDays(-1);
            Arguments.PutString("date", date.ToString());
            adapter = new DateEventListAdapter(date);
            adapter.ItemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(adapter);
            headerDate.Text = date.ToLongDateString();
            checkEventsExist();
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if(Math.Abs(velocityX) > Math.Abs(velocityY))
            {
                if (velocityX > 0)
                    PrevDayButton_Click(null, null);
                else if (velocityX < 0)
                    NextDayButton_Click(null, null);
            }

            return true;
        }

        public bool OnDown(MotionEvent e) { return true; }

        public void OnLongPress(MotionEvent e) { }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) { return true; }

        public void OnShowPress(MotionEvent e) { }

        public bool OnSingleTapUp(MotionEvent e) { return false; }

        public bool OnTouch(View v, MotionEvent e)
        {
            gestureDetector.OnTouchEvent(e);
            return false;
        }
    }
}