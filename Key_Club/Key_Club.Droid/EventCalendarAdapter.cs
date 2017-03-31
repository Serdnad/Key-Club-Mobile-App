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
    class EventCalendarAdapter : RecyclerView.Adapter
    {
        public event EventHandler<DateTime> ItemClick;

        private readonly HashSet<ServiceEvent> events;
        private readonly List<DateTime> days;

        public EventCalendarAdapter(List<DateTime> days)
        {
            this.days = days;
            
            events = ClubInfo.events;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = (EventCalendarViewHolder)holder;
            viewHolder.dayNum.Text = days[position].Day.ToString();

            viewHolder.num = 0;
            viewHolder.eventsNum.Visibility = ViewStates.Invisible;
            viewHolder.eventImg.Visibility = ViewStates.Invisible;
            for (int i = events.Count - 1; i >= 0; i--)
                if (events.ElementAt(i).date == days[position])
                    viewHolder.num++;

            if(viewHolder.num > 0)
            {
                viewHolder.eventsNum.Visibility = ViewStates.Visible;
                viewHolder.eventImg.Visibility = ViewStates.Visible;
                viewHolder.eventsNum.Text = viewHolder.num.ToString();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EventCalendarItem, parent, false);

            layout.FindViewById<LinearLayout>(Resource.Id.calendarGrid).LayoutParameters.Height = UserInfo.ANDROID_GRID_HEIGHT;

            return new EventCalendarViewHolder(layout, OnItemClick);
        }

        public override int ItemCount
        {
            get { return days.Count; }
        }

        void OnItemClick(int position)
        {
            
            if (ItemClick != null)
                ItemClick(this, days[position].Date);
        }
    }
}