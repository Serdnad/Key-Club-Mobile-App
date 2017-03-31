using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace Key_Club.Droid
{
    class EventCalendarViewHolder : RecyclerView.ViewHolder
    {
        public TextView dayNum { get; set; }
        public TextView eventsNum { get; set; }
        public ImageView eventImg { get; set; }
        public int num { get; set; }

        public EventCalendarViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            dayNum = itemView.FindViewById<TextView>(Resource.Id.calendarDay);
            eventsNum = itemView.FindViewById<TextView>(Resource.Id.calendarEventsNum);
            eventImg = ItemView.FindViewById<ImageView>(Resource.Id.calendarEventImg);
            if (num > 0)
            {
                eventsNum.Visibility = ViewStates.Visible;
                eventImg.Visibility = ViewStates.Visible;
            }
            else
            {
                eventsNum.Visibility = ViewStates.Invisible;
                eventImg.Visibility = ViewStates.Invisible;
            }

            itemView.Click += (sender, e) => listener(base.AdapterPosition);
        }
    }
}