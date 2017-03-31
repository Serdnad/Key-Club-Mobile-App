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
using Android.Support.V7.AppCompat;
using Android.Graphics;
using System.IO;
using Java.IO;

namespace Key_Club.Droid
{
    public class EventDialog : Android.Support.V4.App.DialogFragment, DatePickerDialog.IOnDateSetListener, TimePickerDialog.IOnTimeSetListener
    {
        public EditText titleEdit;
        public EditText descEdit;
        public EditText locationEdit;
        public EditText timeStartEdit;
        public EditText timeEndEdit;
        public EditText linkEdit;
        public TextView dateView;
        public DateTime date = new DateTime(2000, 1, 1);
        public DateTime startTime;
        public DateTime endTime;

        public static EventDialog NewInstance(Bundle bundle)
        {
            EventDialog fragment = new EventDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.eventDialog, container, false);
            titleEdit = view.FindViewById<EditText>(Resource.Id.eventTitle);
            descEdit = view.FindViewById<EditText>(Resource.Id.eventDescription);
            locationEdit = view.FindViewById<EditText>(Resource.Id.eventLocation);
            timeStartEdit = view.FindViewById<EditText>(Resource.Id.eventStartTime);
            timeEndEdit = view.FindViewById<EditText>(Resource.Id.eventEndTime);
            linkEdit = view.FindViewById<EditText>(Resource.Id.eventSignUpLink);
            dateView = view.FindViewById<TextView>(Resource.Id.eventDateView);
            dateView.Text = date.ToString("dd/MM/yyyy");

            view.FindViewById<Button>(Resource.Id.eventDatePicker).Click += pickDate;
            view.FindViewById<Button>(Resource.Id.eventCancelButton).Click += CancelButtonClicked;
            view.FindViewById<Button>(Resource.Id.eventAddButton).Click += AddButtonClicked;

            descEdit.LayoutParameters.Width = (int)(Resources.DisplayMetrics.WidthPixels * (5.0 / 7.0));

            return view;
        }

        private void pickDate(object sender, EventArgs e)
        {
            DatePickerDialog dialog = new DatePickerDialog(this.Activity, setDate, DateTime.Today.Year, DateTime.Today.Month-1, DateTime.Today.Day);
            dialog.Show();
        }
        
        private void setDate(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            date = e.Date;
            dateView.Text = date.ToString("dd/MM/yyyy");
        }

        private void setStartTime(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            date = e.Date;
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            //Value verification
            if (titleEdit.Text.Equals(""))
            {
                Toast.MakeText(this.Activity, "Title is empty", ToastLength.Short).Show();
                return;
            }
            else if (descEdit.Text.Equals(""))
            {
                Toast.MakeText(this.Activity, "Decsription is empty", ToastLength.Short).Show();
                return;
            }
            else if (locationEdit.Text.Equals(""))
            {
                Toast.MakeText(this.Activity, "Location is empty", ToastLength.Short).Show();
                return;
            }
            else if(DateTime.TryParse(timeStartEdit.Text, out startTime) == false)
            {
                Toast.MakeText(this.Activity, "Start time not a time", ToastLength.Short).Show();
                return;
            }
            else if (DateTime.TryParse(timeEndEdit.Text, out endTime) == false)
            {
                Toast.MakeText(this.Activity, "End time not a time", ToastLength.Short).Show();
                return;
            }
            else if (date.Equals(new DateTime(2000, 1, 1)))
            {
                Toast.MakeText(this.Activity, "Date not set", ToastLength.Short).Show();
                return;
            }

            ServiceEvent se = new ServiceEvent();
            se.title = titleEdit.Text;
            se.description = descEdit.Text;
            se.location = locationEdit.Text;
            se.date = date;
            se.timeStart = startTime;
            se.timeEnd = endTime;
            se.signUpLink = (linkEdit.Text.Equals("")) ? "NONE" : linkEdit.Text;

            DynamoDbService.uploadEvent(se);
            ClubInfo.events.Add(se);

            Toast.MakeText(Activity, "Event added", ToastLength.Short).Show();
            Dismiss();
        }    

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            Dismiss();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            
        }
    }
}