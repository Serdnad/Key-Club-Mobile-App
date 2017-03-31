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
    class DateEventListViewHolder: RecyclerView.ViewHolder
    {
        public TextView titleView { get; set; }
        public TextView timesView { get; set; }

        public DateEventListViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            titleView = itemView.FindViewById<TextView>(Resource.Id.eventTitle);
            timesView = itemView.FindViewById<TextView>(Resource.Id.eventTimes);

            itemView.Click += (sender, e) => listener(base.AdapterPosition);
        }
    }
}