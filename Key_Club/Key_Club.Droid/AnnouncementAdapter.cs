using System;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Key_Club;
using Android.Graphics;
using Android.Util;

namespace Key_Club.Droid
{
    class AnnouncementAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;

        private readonly List<Announcement> announcements;

        public AnnouncementAdapter(List<Announcement> announcements)
        {
            this.announcements = announcements;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = (AnnouncementViewHolder)holder;
            viewHolder.titleView.Text = announcements[position].title;
            viewHolder.bodyView.Text = announcements[position].description;
            viewHolder.dateView.Text = announcements[position].date.ToString("MM/dd/yyyy");
            if (announcements[position].imgString.Equals("") || announcements[position].imgString.Equals("NONE"))
                viewHolder.imageView.SetImageResource(Resource.Drawable.KC_Logo);
            else
            {
                byte[] decodedString = Base64.Decode(announcements[position].imgString, Base64Flags.Default);
                Bitmap decodedByte = BitmapFactory.DecodeByteArray(decodedString, 0, decodedString.Length);

                viewHolder.imageView.SetImageBitmap(decodedByte);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AnnouncementItem, parent, false);

            return new AnnouncementViewHolder(layout, OnItemClick);
            
        }

        public override int ItemCount
        {
            get { return announcements.Count; }
        }

        void OnItemClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}