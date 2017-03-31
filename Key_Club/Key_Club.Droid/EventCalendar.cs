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
using System.Globalization;

namespace Key_Club.Droid
{
    public class EventCalendar : SupportFragment, View.IOnTouchListener, GestureDetector.IOnGestureListener
    {
        private GestureDetector gestureDetector;

        bool dateInitiated = false;
        DateTime date;
        RecyclerView mRecyclerView;
        EventCalendarAdapter adapter;
        RecyclerView.LayoutManager mLayoutManager;
        TextView headerMonth;

        List<ServiceEvent> events = new List<ServiceEvent>();
        List<DateTime> days = new List<DateTime>(42); //42 days in 6 weeks

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.eventCalendar, container, false);
            if (!UserInfo.role.ToLower().Equals("member") && !UserInfo.role.Equals(""))
            {
                ImageButton addEventButton = this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton);
                addEventButton.Visibility = ViewStates.Visible;
                //addEventButton.Click += AddEvent;
            }
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Visibility = ViewStates.Visible;
            //this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click += RefreshEvents;

            if (dateInitiated == false)
            {
                date = DateTime.Today;
                dateInitiated = true;
            }

            headerMonth = view.FindViewById<TextView>(Resource.Id.calendarMonth);
            headerMonth.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((date.ToString("MMMM yyyy"))); //Overly complex but lazy way of capitalizing month


            ImageButton prevMonthButton = view.FindViewById<ImageButton>(Resource.Id.calendarPrevMonth);
            prevMonthButton.Click += PrevMonthButton_Click;

            ImageButton nextMonthButton = view.FindViewById<ImageButton>(Resource.Id.calendarNextMonth);
            nextMonthButton.Click += NextMonthButton_Click;

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new GridLayoutManager(this.Activity, 7, GridLayoutManager.Vertical, false);

            mRecyclerView.SetLayoutManager(mLayoutManager);

            init(date);
            adapter = new EventCalendarAdapter(days);//, events);
            adapter.ItemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(adapter);

            

            UserInfo.ANDROID_GRID_HEIGHT = (int)((Resources.DisplayMetrics.HeightPixels - (view.FindViewById<RelativeLayout>(Resource.Id.calendarHeaderLayout).LayoutParameters.Height * 1
            + view.FindViewById<LinearLayout>(Resource.Id.calendarDaysHeader).LayoutParameters.Height)) / 8);

            view.FindViewById<RecyclerView>(Resource.Id.recyclerView).SetOnTouchListener(this);
            gestureDetector = new GestureDetector(this);

            return view;
        }

        private async void RefreshEvents(object sender, EventArgs e)
        {
            Toast.MakeText(this.Activity, "Refreshing Events...", ToastLength.Short).Show();
            await DynamoDbService.getEvents();
            adapter = new EventCalendarAdapter(days);
            adapter.ItemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(adapter);
        }

        private void AddEvent(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            SupportFragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }

            ft.AddToBackStack(null);

            EventDialog newEventDialog = new EventDialog();
            newEventDialog.Show(ft, "dialog");

            //adapter = new EventCalendarAdapter(days);
            //adapter.ItemClick += Adapter_ItemClick;
            //mRecyclerView.SetAdapter(adapter);
        }

        private void NextMonthButton_Click(object sender, EventArgs e)
        {
            date = date.AddMonths(1);
            init(date);
            adapter = new EventCalendarAdapter(days);
            adapter.ItemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(adapter);
            headerMonth.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((date.ToString("MMMM yyyy")));
        }

        private void PrevMonthButton_Click(object sender, EventArgs e)
        {
            date = date.AddMonths(-1);
            init(date);
            adapter = new EventCalendarAdapter(days);
            adapter.ItemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(adapter);
            headerMonth.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((date.ToString("MMMM yyyy")));
        }

        private void Adapter_ItemClick(object sender, DateTime e)
        {
            DateEventList eventList = new DateEventList();
            Bundle args = new Bundle();
            args.PutString("date", e.ToString());
            eventList.Arguments = args;
            
            var ft = this.FragmentManager.BeginTransaction();
            ft.SetCustomAnimations(Resource.Animation.abc_popup_enter, Resource.Animation.abc_popup_exit);
            ft.Replace(Resource.Id.fragment_container, eventList);
            ft.AddToBackStack(null);
            ft.CommitAllowingStateLoss();
        }

        public void init(DateTime dt)
        {
            DateTime start = DateTime.Now;
            int daysToAdd = 42;
            days.Clear();
            days = new List<DateTime>(42);
            dt = new DateTime(dt.Year, dt.Month, 1);

            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    days.Add(dt.AddDays(-6));
                    daysToAdd--;
                    goto case DayOfWeek.Friday;
                case DayOfWeek.Friday:
                    days.Add(dt.AddDays(-5));
                    daysToAdd--;
                    goto case DayOfWeek.Thursday;
                case DayOfWeek.Thursday:
                    days.Add(dt.AddDays(-4));
                    daysToAdd--;
                    goto case DayOfWeek.Wednesday;
                case DayOfWeek.Wednesday:
                    days.Add(dt.AddDays(-3));
                    daysToAdd--;
                    goto case DayOfWeek.Tuesday;
                case DayOfWeek.Tuesday:
                    days.Add(dt.AddDays(-2));
                    daysToAdd--;
                    goto case DayOfWeek.Monday;
                case DayOfWeek.Monday:
                    days.Add(dt.AddDays(-1));
                    daysToAdd--;
                    break;
            }

            for (int i = 0; i < daysToAdd; i++)
                days.Add(dt.AddDays(i));

            long stopwatch = DateTime.Now.Ticks - start.Ticks;
        }

        public override void OnStop()
        {
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click -= RefreshEvents;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Click -= AddEvent;

            base.OnStop();
        }

        public override void OnResume()
        {
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Click += RefreshEvents;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Click += AddEvent;

            base.OnResume();
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if (velocityX > 0)
                PrevMonthButton_Click(null, null);
            else if (velocityX < 0)
                NextMonthButton_Click(null, null);

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