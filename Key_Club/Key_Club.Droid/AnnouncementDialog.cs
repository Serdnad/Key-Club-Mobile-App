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
using static Android.Provider.MediaStore.Images;
using System.IO;
using Java.IO;

namespace Key_Club.Droid
{
    public class AnnouncementDialog : Android.Support.V4.App.DialogFragment
    {
        public EditText titleEdit;
        public EditText descEdit;
        public DateTime date;
        public int imageId = 1;
        public string imgString = "NONE";
        public ImageView imageView;
        //public byte[] test;

        public static AnnouncementDialog NewInstance(Bundle bundle)
        {
            AnnouncementDialog fragment = new AnnouncementDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.announcementDialog, container, false);
            titleEdit = view.FindViewById<EditText>(Resource.Id.annTitle);
            descEdit = view.FindViewById<EditText>(Resource.Id.annDescription);
            imageView = view.FindViewById<ImageView>(Resource.Id.annImageView);
            view.FindViewById<Button>(Resource.Id.annImagePicker).Click += AnnouncementDialog_Click;
            view.FindViewById<Button>(Resource.Id.annCancelButton).Click += CancelButtonClicked;
            view.FindViewById<Button>(Resource.Id.annAddButton).Click += AddButtonClicked;

            descEdit.LayoutParameters.Width = (int)(Resources.DisplayMetrics.WidthPixels * (5.0 / 7.0));

            return view;
        }

        private void AnnouncementDialog_Click(object sender, EventArgs e)
        {
            //DatePickerDialog dialog = new DatePickerDialog(this.Activity, setDate, DateTime.Today.Year, DateTime.Today.Month-1, DateTime.Today.Day);
            //dialog.Show();

            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), 1);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if ((requestCode == imageId) && (resultCode == -1) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                Bitmap mBitmap = Media.GetBitmap(this.Activity.ContentResolver, uri);

                //scale bitmap
                mBitmap = resizeBitmapFitXY(192, 192, mBitmap);

                imageView.SetImageBitmap(mBitmap);

                MemoryStream stream = new MemoryStream();
                mBitmap.Compress(Bitmap.CompressFormat.Webp, 95, stream);
                byte[] byteArray = stream.ToArray(); //it's possible to change the code so that this byte array is directly uploaded, which is essentially what's being done now in more steps.
                //test = byteArray;

                imgString = Convert.ToBase64String(byteArray);
            }
        }

        //Delete this, this is meant to go under Event Dialog
        private void setDate(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            date = e.Date;
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            //Value verification
            if(titleEdit.Text.Equals(""))
            {
                Toast.MakeText(this.Activity, "Title is empty", ToastLength.Short).Show();
                return;
            }
            else if (descEdit.Text.Equals(""))
            {
                Toast.MakeText(this.Activity, "Announcement is empty", ToastLength.Short).Show();
                return;
            }

            Announcement a = new Announcement();
            a.title = titleEdit.Text;
            a.description = descEdit.Text;
            a.date = DateTime.Now;
            a.imgString = imgString;

            DynamoDbService.uploadAnnouncement(a);
            ClubInfo.announcements.Insert(0, a);

            Toast.MakeText(Activity, "Announcement added", ToastLength.Short).Show();
            Dismiss();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            Dismiss();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            date = new DateTime(year, monthOfYear, dayOfMonth);
        }

        public Bitmap resizeBitmapFitXY(int width, int height, Bitmap bitmap)
        {
            Bitmap image = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            float originalWidth = bitmap.Width, originalHeight = bitmap.Height;
            Canvas canvas = new Canvas(image);
            float scale, xTranslation = 0.0f, yTranslation = 0.0f;
            if (originalWidth > originalHeight)
            {
                scale = height / originalHeight;
                xTranslation = (width - originalWidth * scale) / 2.0f;
            }
            else
            {
                scale = width / originalWidth;
                yTranslation = (height - originalHeight * scale) / 2.0f;
            }
            Matrix transformation = new Matrix();
            transformation.PostTranslate(xTranslation, yTranslation);
            transformation.PreScale(scale, scale);
            Paint paint = new Paint();
            paint.FilterBitmap = true;
            canvas.DrawBitmap(bitmap, transformation, paint);
            return image;
        }
    }
}