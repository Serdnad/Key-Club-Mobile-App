using Foundation;
using System;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using WebP.Touch;

namespace Key_Club.iOS
{
	public partial class EventListController : UIViewController
	{
		public static List<ServiceEvent> events = new List<ServiceEvent>();

		EventListSource source;
		UICollectionViewFlowLayout layout;
		UICollectionView collectionView;
		UIRefreshControl refresh = new UIRefreshControl();

		static float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
		float cellHeight = 50;

		public EventListController(IntPtr handle) : base(handle)
		{
			events.Clear();

			foreach(ServiceEvent e in ClubInfo.events)
			{
				if (e.date == UserInfo.IOS_SELECTED_DATE)
					events.Add(e);
			}

			layout = new UICollectionViewFlowLayout
			{
				SectionInset = new UIEdgeInsets(10, 10, 10, 10),
				MinimumInteritemSpacing = 10,
				MinimumLineSpacing = 5,
				ItemSize = new CoreGraphics.CGSize(screenWidth - 20, cellHeight)
			};

			CoreGraphics.CGRect rect = new CoreGraphics.CGRect(0, 108, screenWidth, UIScreen.MainScreen.Bounds.Height - 108);

			collectionView = new UICollectionView(rect, layout);
			collectionView.BackgroundColor = UIColor.White;
			collectionView.ContentSize = View.Frame.Size;
			collectionView.AlwaysBounceVertical = true;

			source = new EventListSource(this);
			AutomaticallyAdjustsScrollViewInsets = false;

			collectionView.RegisterClassForCell(typeof(EventListCell), EventListCell.CellId);
			collectionView.Source = source;

			refresh.ValueChanged += refreshEventList;

			collectionView.AddSubview(refresh);
		}

		public override void LoadView()
		{
			base.LoadView();

			View = new UIView(UIScreen.MainScreen.Bounds);
			View.BackgroundColor = UIColor.White;

			UINavigationBar bar = new UINavigationBar(new CoreGraphics.CGRect(0, 64, screenWidth, 44));
			bar.BackgroundColor = UIColor.White;
			bar.BarTintColor = UIColor.FromRGB(0, 40, 85);
			bar.SetItems(new UINavigationItem[] { new UINavigationItem(UserInfo.IOS_SELECTED_DATE.ToString("dddd MMM dd, yyyy"))}, false );

			UIBarButtonItem prevDay = new UIBarButtonItem();
			prevDay.Title = "←";
			prevDay.Clicked += async (sender, eventArgs) => 
			{
				DateTime d = UserInfo.IOS_SELECTED_DATE.AddDays(-1);
				UserInfo.IOS_SELECTED_DATE = d;
				await DynamoDbService.getEvents();
				events.Clear();
				foreach (ServiceEvent e in ClubInfo.events)
				{
					if (e.date == UserInfo.IOS_SELECTED_DATE)
						events.Add(e);
				}
				collectionView.ReloadData();

				bar.Items[0].Title = UserInfo.IOS_SELECTED_DATE.ToString("dddd MMM dd, yyyy");
			};

			UIBarButtonItem nextDay = new UIBarButtonItem();
			nextDay.Title = "→";
			nextDay.Clicked += async (sender, eventArgs) =>
			{
				DateTime d = UserInfo.IOS_SELECTED_DATE.AddDays(1);
				UserInfo.IOS_SELECTED_DATE = d;
				await DynamoDbService.getEvents();
				events.Clear();
				foreach (ServiceEvent e in ClubInfo.events)
				{
					if (e.date == UserInfo.IOS_SELECTED_DATE)
						events.Add(e);
				}
				collectionView.ReloadData();

				bar.Items[0].Title = UserInfo.IOS_SELECTED_DATE.ToString("dddd MMM dd, yyyy");
			};


			bar.Items[0].LeftBarButtonItem = prevDay;
			bar.Items[0].RightBarButtonItem = nextDay;

			View.Add(bar);
			View.Add(collectionView);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public async void refreshEventList(object o, EventArgs eArgs)
		{
			await DynamoDbService.getEvents();

			events.Clear();
			foreach (ServiceEvent e in ClubInfo.events)
			{
				if (e.date == UserInfo.IOS_SELECTED_DATE)
					events.Add(e);
			}

			collectionView.ReloadData();
			refresh.EndRefreshing();
		}

		class EventListSource : UICollectionViewSource
		{
			public static UIViewController view;

			public EventListSource(UIViewController masterView)
			{
				view = masterView;
			}

			public override nint GetItemsCount(UICollectionView collectionView, nint section)
			{
				return events.Count;
			}

			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
			{
				var cell = (EventListCell)collectionView.DequeueReusableCell(EventListCell.CellId, indexPath);

				cell.Layer.CornerRadius = 4;

				cell.Title = events[indexPath.Row].title;
				cell.Times = events[indexPath.Row].timeStart.ToShortTimeString() + " - " + events[indexPath.Row].timeEnd.ToShortTimeString();

				return cell;
			}

			public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
			{
				ServiceEvent e = events[indexPath.Row];
				EventDetailsViewController details = (EventDetailsViewController)UIStoryboard.FromName("Main", null).InstantiateViewController("EventDetailsViewController");
				details.Title = "Event Details";
				details.e = e;
				view.NavigationController.PushViewController((UIViewController)details, true);
			}

			public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
			{
				return true;
			}
		}

		class EventListCell : UICollectionViewCell
		{
			public UILabel titleLabel;
			public UILabel timesLabel;
			public UILabel tapForInfoLabel;

			public static readonly NSString CellId = new NSString("EventCell");

			public string Title
			{
				get { return titleLabel.Text; }
				set
				{
					titleLabel.Text = value;
					SetNeedsDisplay();
				}
			}

			public string Times
			{
				get { return timesLabel.Text; }
				set
				{
					timesLabel.Text = value;
					SetNeedsDisplay();
				}
			}

			[Export("initWithFrame:")]
			EventListCell(RectangleF frame) : base(frame)
			{
				titleLabel = new UILabel(new CoreGraphics.CGRect(10, 5, screenWidth/2, 20))
				{
					TextColor = UIColor.White,
					Font = UIFont.SystemFontOfSize(12, UIFontWeight.Bold)
				};

				timesLabel = new UILabel(new CoreGraphics.CGRect(screenWidth/2, 15, screenWidth/2 - 25, 20))
				{
					TextColor = UIColor.White,
					Font = UIFont.SystemFontOfSize(12),
					TextAlignment = UITextAlignment.Right
				};

				tapForInfoLabel = new UILabel(new CoreGraphics.CGRect(10, 25, screenWidth/2, 20))
				{
					TextColor = UIColor.DarkGray,
					Font = UIFont.SystemFontOfSize(11),
					Text = "Tap for details"
				};

				ContentView.AddSubview(titleLabel);
				ContentView.AddSubview(timesLabel);
				ContentView.AddSubview(tapForInfoLabel);

				BackgroundColor = UIColor.FromRGB(0, 173, 239);
			}
		}
	}
}