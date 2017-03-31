using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uri = Android.Net.Uri;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using Android.Support.V7.App;
using Android.Provider;
using Java.Util;

namespace Key_Club.Droid
{
    public class EventDetails : Android.Support.V4.App.Fragment
    {
        string title, date, location, desc, time, link;
        DateTime startTime, endTime;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            title = Arguments.GetString("title");
            date = Arguments.GetString("date");
            location = Arguments.GetString("location");
            desc = Arguments.GetString("description");
            link = Arguments.GetString("link");
            time = Arguments.GetString("time");
            startTime = DateTime.Parse(Arguments.GetString("startTime"));
            endTime = DateTime.Parse(Arguments.GetString("endTime"));

            this.Activity.FindViewById<SupportToolbar>(Resource.Id.toolBar).Title = "Event Details";

            var view = inflater.Inflate(Resource.Layout.eventDetails, container, false);
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Visibility = ViewStates.Gone;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Visibility = ViewStates.Gone;

            if (link == "NONE")
                view.FindViewById<Android.Support.V7.Widget.AppCompatButton>(Resource.Id.eventSignUp).Visibility = ViewStates.Gone;
            else
                view.FindViewById<Android.Support.V7.Widget.AppCompatButton>(Resource.Id.eventSignUp).Click += OpenSignUpLink;

            view.FindViewById<TextView>(Resource.Id.eventTitle).Text = title;
            view.FindViewById<TextView>(Resource.Id.eventDateHeader).Text = date;
            view.FindViewById<TextView>(Resource.Id.eventDescription).Text = desc;
            view.FindViewById<TextView>(Resource.Id.eventTimes).Text = "Time: " + time;
            view.FindViewById<TextView>(Resource.Id.eventLocation).Text = "Location: " + location;
            view.FindViewById<Button>(Resource.Id.addEventToCalendar).Click += addEventToCalendar_Click;

            return view;
        }

        private void OpenSignUpLink(object sender, EventArgs e)
        {
            if (!link.StartsWith("http"))
                link = "http://" + link;
            Intent intent = new Intent(Intent.ActionView, Uri.Parse(link));
            StartActivity(intent);
        }

        private void addEventToCalendar_Click(object sender, EventArgs e)
        {
            DateTime eventDate = DateTime.Parse(date).AddMonths(-1);

            Calendar eventStart = Calendar.GetInstance(Java.Util.TimeZone.Default);
            Calendar eventEnd = Calendar.GetInstance(Java.Util.TimeZone.Default);

            eventStart.Set(eventDate.Year, eventDate.Month, eventDate.Day, startTime.Hour, startTime.Minute);
            eventEnd.Set(eventDate.Year, eventDate.Month, eventDate.Day, endTime.Hour, endTime.Minute);

            Intent intent = new Intent(Intent.ActionEdit);
            intent.SetData(CalendarContract.Events.ContentUri);
            intent.PutExtra("title", "Key Club: " + title);
            intent.PutExtra("description", desc);
            intent.PutExtra("eventLocation", location);
            intent.PutExtra("beginTime", eventStart.TimeInMillis);
            intent.PutExtra("endTime", eventEnd.TimeInMillis);
            StartActivity(intent);
        }
    }
}