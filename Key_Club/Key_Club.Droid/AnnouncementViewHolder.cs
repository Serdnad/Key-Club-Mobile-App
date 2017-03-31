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
    class AnnouncementViewHolder : RecyclerView.ViewHolder
    {
        public TextView titleView { get; set; }
        public TextView bodyView { get; set; }
        public ImageView imageView { get; set; }
        public TextView dateView { get; set; }

        public AnnouncementViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            titleView = itemView.FindViewById<TextView>(Resource.Id.announcementTitle);
            bodyView = itemView.FindViewById<TextView>(Resource.Id.announcementBody);
            imageView = itemView.FindViewById<ImageView>(Resource.Id.announcementImage);
            dateView = itemView.FindViewById<TextView>(Resource.Id.announcementDate);
        }
    }
}