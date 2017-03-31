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
using Key_Club;

namespace Key_Club.Droid
{
    class DateEventListAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ServiceEvent> ItemClick;

        public readonly List<ServiceEvent> events;

        public DateEventListAdapter(List<ServiceEvent> events)
        {
            this.events = events;
        }

        public DateEventListAdapter(DateTime date)
        {
            events = new List<ServiceEvent>();

            for (int i = ClubInfo.events.Count - 1; i >= 0; i--)
            {
                if (ClubInfo.events.ElementAt(i).date == date)
                    events.Add(ClubInfo.events.ElementAt(i));
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = (DateEventListViewHolder)holder;
            viewHolder.titleView.Text = events[position].title;
            viewHolder.timesView.Text = events[position].timeStart.ToShortTimeString() + " - " + events[position].timeEnd.ToShortTimeString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.DateEventListItem, parent, false);

            return new DateEventListViewHolder(layout, OnItemClick);

        }

        public override int ItemCount
        {
            get { return events.Count; }
        }

        void OnItemClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, events[position]);
        }
    }
}