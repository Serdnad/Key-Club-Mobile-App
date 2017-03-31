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

namespace Key_Club.Droid
{
    public class AppInfo : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.appInfo, container, false);

            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarAddButton).Visibility = ViewStates.Gone;
            this.Activity.FindViewById<ImageButton>(Resource.Id.toolbarRefreshButton).Visibility = ViewStates.Gone;

            return view;
        }
    }
}