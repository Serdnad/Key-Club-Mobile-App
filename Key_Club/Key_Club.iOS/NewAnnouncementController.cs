using Foundation;
using System;
using UIKit;
using Photos;
using SkiaSharp;
using System.IO;
using System.Drawing;

namespace Key_Club.iOS
{
    public partial class NewAnnouncementController : UIViewController
    {
		static public string imgString = "NONE";

		UIImagePickerController imagePicker = new UIImagePickerController();

        public NewAnnouncementController (IntPtr handle) : base (handle)
        {
			
        }

		public override void ViewDidLoad()
		{
			UIBarButtonItem uploadButton = new UIBarButtonItem();
			uploadButton.Title = "Upload";
			uploadButton.Clicked += AddAnnouncement;
			NavigationItem.RightBarButtonItem = uploadButton;

			base.ViewDidLoad();

			NewAnnTitle.ShouldReturn += (textField) =>
			{
				NewAnnDescription.BecomeFirstResponder();
				return false;
			};

			NewAnnDescription.Layer.BorderWidth = (nfloat)0.5;
			//NewAnnDescription.Layer.BorderColor = new CoreGraphics.CGColor("black");
			NewAnnDescription.Changed += (sender, e) =>
			{
				if (NewAnnDescription.Text.Length > 0)
				{
					if (NewAnnDescription.Text[NewAnnDescription.Text.Length - 1] == '\n')
						View.EndEditing(true);
				}
			};

			NewAnnImageButton.TouchUpInside += PickImage;

			//NewAnnDescription.ReturnKeyType += (textField) =>
			//{
			//	textField.ResignFirstResponder();
			//	return true;
			//};

		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			NewAnnDescription.ResignFirstResponder();
		}

		public void AddAnnouncement(object o, EventArgs e)
		{
			if (NewAnnTitle.Text == "")
			{
				AnnErrorLabel.TextColor = UIColor.Red;
				AnnErrorLabel.Text = "Title cannot be empty";
				return;
			}
			else if (NewAnnDescription.Text == "")
			{
				AnnErrorLabel.TextColor = UIColor.Red;
				AnnErrorLabel.Text = "Announcement cannot be empty";
				return;
			}

			Announcement a = new Announcement();
			a.title = NewAnnTitle.Text;
			a.description = NewAnnDescription.Text;
			a.date = DateTime.Now;
			a.imgString = imgString;

			DynamoDbService.uploadAnnouncement(a);

			NavigationController.PopViewController(true);
		}

		public void PickImage(object o, EventArgs e)
		{
			PHPhotoLibrary.RequestAuthorizationAsync();


			imagePicker = new UIImagePickerController();

			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
			imagePicker.Delegate = new pickerDelegate(this);

			NavigationController.PresentModalViewController(imagePicker, true);
		}

		private class pickerDelegate : UIImagePickerControllerDelegate
		{
			public NewAnnouncementController vc;

			public pickerDelegate(NewAnnouncementController controller) : base()
			{
				vc = controller;
			}

			public override void FinishedPickingImage(UIImagePickerController picker, UIImage image, NSDictionary editingInfo)
			{
				vc.NewAnnImageView.Image = image;
				image = image.Scale(new CoreGraphics.CGSize(192, 192));

				using (NSData imageData = image.AsJPEG())
				{
					MemoryStream stream = new MemoryStream();

					SKImage newImg = SKImage.FromData(new SKData(imageData.ToArray()));
					newImg.Encode(SKImageEncodeFormat.Webp, 95).AsStream().CopyTo(stream);

					imgString = Convert.ToBase64String(stream.ToArray());
				}

				picker.DismissViewController(true, null);
			}
		}
    }
}