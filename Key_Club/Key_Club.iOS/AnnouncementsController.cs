using Foundation;
using System;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using WebP.Touch;

namespace Key_Club.iOS
{
	public partial class AnnouncementsController : UIViewController
	{
		AnnouncementsSource source;
		UICollectionViewFlowLayout layout;
		UICollectionView collectionView;
		UIRefreshControl refresh = new UIRefreshControl();

		float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
		float cellHeight = 100;

		public AnnouncementsController(IntPtr handle) : base(handle)
		{
			layout = new UICollectionViewFlowLayout
			{
				SectionInset = new UIEdgeInsets(5, 5, 5, 5),
				MinimumInteritemSpacing = 5,
				MinimumLineSpacing = 5,
				ItemSize = new CoreGraphics.CGSize(screenWidth, cellHeight)
			};

			collectionView = new UICollectionView(UIScreen.MainScreen.Bounds, layout);
			collectionView.BackgroundColor = UIColor.White;
			collectionView.ContentSize = View.Frame.Size;
			collectionView.AlwaysBounceVertical = true;

			source = new AnnouncementsSource();
			AutomaticallyAdjustsScrollViewInsets = false;

			collectionView.RegisterClassForCell(typeof(AnnouncementCell), AnnouncementCell.CellId);
			collectionView.Source = source;

			refresh.ValueChanged += refreshAnnouncements;

			collectionView.AddSubview(refresh);
		}

		public override void LoadView()
		{
			base.LoadView();

			View = collectionView;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public async void refreshAnnouncements(object o, EventArgs e)
		{
			await DynamoDbService.getAnnouncements();
			collectionView.ReloadData();
			refresh.EndRefreshing();
		}

		class AnnouncementsSource : UICollectionViewSource
		{
			public override nint GetItemsCount(UICollectionView collectionView, nint section)
			{
				return ClubInfo.announcements.Count;
			}

			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
			{
				var cell = (AnnouncementCell)collectionView.DequeueReusableCell(AnnouncementCell.CellId, indexPath);

				cell.Title = ClubInfo.announcements[indexPath.Row].title;
				cell.Description = ClubInfo.announcements[indexPath.Row].description;
				cell.Date = ClubInfo.announcements[indexPath.Row].date.ToShortDateString();

				string imgString = ClubInfo.announcements[indexPath.Row].imgString;

				if (imgString.Equals("NONE"))
					cell.Image = UIImage.FromBundle("Images/KC_Logo.png");
				else
				{
					byte[] ba = Convert.FromBase64String(imgString);
					cell.Image = new WebPCodec().Decode(ba);
				}

				return cell;
			}

			public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
			{
				Console.WriteLine("Row {0} selected", indexPath.Row);
			}

			public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
			{
				return true;
			}
		}

		class AnnouncementCell : UICollectionViewCell
		{
			float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;

			UILabel titleLabel;
			UILabel dateLabel;
			UITextView descriptionText;
			UIImageView imageView;

			public static readonly NSString CellId = new NSString("AnnouncementCell");

			public string Title
			{
				get { return titleLabel.Text; }
				set
				{
					titleLabel.Text = value;
					SetNeedsDisplay();
				}
			}

			public string Date
			{
				get { return dateLabel.Text; }
				set
				{
					dateLabel.Text = value;
					SetNeedsDisplay();
				}
			}

			public new string Description
			{
				get { return descriptionText.Text; }
				set
				{
					descriptionText.Text = value;
					SetNeedsDisplay();
				}
			}

			public UIImage Image
			{
				get { return imageView.Image; }
				set
				{
					imageView.Image = value;
					SetNeedsDisplay();
				}
			}

			[Export("initWithFrame:")]
			AnnouncementCell(RectangleF frame) : base(frame)
			{
				float imageWidth = (float)UIScreen.MainScreen.Bounds.Width/4;
				float viewHeight;
				if (imageWidth + imageWidth / 4 < 100)
					viewHeight = imageWidth + imageWidth / 4;
				else
				{
					viewHeight = 95;
					imageWidth = 95 / (1.25f);
				}
				//= (imageWidth + imageWidth / 4 > 100) ? (imageWidth + imageWidth / 4, imageWidth = imageWidth) : 90;
				float cellHeight = 100;

				//BackgroundColor = UIColor.LightGray;
				titleLabel = new UILabel(new CoreGraphics.CGRect(imageWidth, 0, screenWidth - imageWidth, 20))
				{
					//BackgroundColor = UIColor.Red,
					TextColor = UIColor.Black,
					Font = UIFont.SystemFontOfSize(12.5f, UIFontWeight.Bold)
					//make bold
					//TextAlignment = UITextAlignment.Center
				};

				descriptionText = new UITextView(new CoreGraphics.CGRect(imageWidth - 5, 18, screenWidth - imageWidth, cellHeight - 18))
				{
					TextColor = UIColor.Black,
					Editable = false,
					Font = UIFont.SystemFontOfSize(12),
					ScrollEnabled = false,
					ContentInset = UIEdgeInsets.Zero
				};

				dateLabel = new UILabel(new CoreGraphics.CGRect(0, 0, imageWidth, 20))
				{
					//BackgroundColor = UIColor.Red,
					TextColor = UIColor.Gray,
					Font = UIFont.SystemFontOfSize(12),
					//make bold
					TextAlignment = UITextAlignment.Center
				};

				imageView = new UIImageView(new CoreGraphics.CGRect(5, 18, imageWidth-10, imageWidth-10)); //width account for margins
				//imageView.Image = image;

				ContentView.AddSubview(titleLabel);
				ContentView.AddSubview(descriptionText);
				ContentView.AddSubview(imageView);
				ContentView.AddSubview(dateLabel);
			}
		}
	}
}