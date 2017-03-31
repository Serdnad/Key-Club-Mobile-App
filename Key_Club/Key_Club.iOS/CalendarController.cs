using Foundation;
using System;
using UIKit;
using System.Drawing;
using System.Collections.Generic;

namespace Key_Club.iOS
{
	public partial class CalendarController : UIViewController
	{
		public static List<DateTime> days = new List<DateTime>(42);
		CalendarSource source;
		UICollectionViewFlowLayout layout;
		public UICollectionView collectionView;
		UIRefreshControl refresh = new UIRefreshControl();

		static float screenWidth = (float)UIScreen.MainScreen.Bounds.Width;
		static float cellWidth = screenWidth / 8;
		static float cellHeight = (float)(UIScreen.MainScreen.Bounds.Height / 9f);
		static float imagePosY = cellHeight / 2 - (cellWidth - 10) / 2;

		public CalendarController(IntPtr handle) : base(handle)
		{
			UserInfo.IOS_SELECTED_DATE = DateTime.Today;
			init(UserInfo.IOS_SELECTED_DATE);

			layout = new UICollectionViewFlowLayout
			{
				SectionInset = new UIEdgeInsets(5, 5, 5, 5),
				MinimumInteritemSpacing = 5,
				MinimumLineSpacing = 5,
				ItemSize = new CoreGraphics.CGSize(cellWidth, cellHeight)
			};

			CoreGraphics.CGRect rect = new CoreGraphics.CGRect(0, 123, screenWidth, UIScreen.MainScreen.Bounds.Height - 123);

			collectionView = new UICollectionView(rect, layout);
			collectionView.BackgroundColor = UIColor.White;
			collectionView.ContentSize = View.Frame.Size;
			collectionView.AlwaysBounceVertical = true;
			collectionView.ScrollEnabled = false;

			source = new CalendarSource();
			AutomaticallyAdjustsScrollViewInsets = false;

			collectionView.RegisterClassForCell(typeof(CalendarCell), CalendarCell.CellId);
			collectionView.Source = source;
			collectionView.Delegate = new CalendarCollectionViewDelegate(this);

			refresh.ValueChanged += refreshCalendar;

			collectionView.AddSubview(refresh);
		}

		public override void ViewDidLoad()
		{
			//base.LoadView();

			View = new UIView(UIScreen.MainScreen.Bounds);
			View.BackgroundColor = UIColor.White;

			UINavigationBar bar = new UINavigationBar(new CoreGraphics.CGRect(0, 64, screenWidth, 44));
			bar.BackgroundColor = UIColor.White;
			bar.BarTintColor = UIColor.FromRGB(0, 40, 85);
			bar.SetItems(new UINavigationItem[] { new UINavigationItem(UserInfo.IOS_SELECTED_DATE.ToString("MMMM yyyy")) }, false);

			UIBarButtonItem prevMonth = new UIBarButtonItem();
			prevMonth.Title = "←";
			prevMonth.Clicked += async (sender, eventArgs) =>
			{
				await DynamoDbService.getEvents();

				DateTime dt = UserInfo.IOS_SELECTED_DATE;
				UserInfo.IOS_SELECTED_DATE = dt.AddMonths(-1);
				init(UserInfo.IOS_SELECTED_DATE);
				collectionView.ReloadData();
				bar.Items[0].Title = UserInfo.IOS_SELECTED_DATE.ToString("MMMM yyyy");
			};

			UIBarButtonItem nextMonth = new UIBarButtonItem();
			nextMonth.Title = "→";
			nextMonth.Clicked += async (sender, eventArgs) =>
			{
				await DynamoDbService.getEvents();

				DateTime dt = UserInfo.IOS_SELECTED_DATE;
				UserInfo.IOS_SELECTED_DATE = dt.AddMonths(1);
				init(UserInfo.IOS_SELECTED_DATE);
				collectionView.ReloadData();
				bar.Items[0].Title = UserInfo.IOS_SELECTED_DATE.ToString("MMMM yyyy");
			};

			bar.Items[0].LeftBarButtonItem = prevMonth;
			bar.Items[0].RightBarButtonItem = nextMonth;

			bar.SetNeedsLayout();

			string[] weekdays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

			for (int i = 0; i < 7; i++)
			{
				UILabel dateLabel = new UILabel(new CoreGraphics.CGRect((screenWidth/7)*i, 111, screenWidth / 7, 12));
				dateLabel.Text = weekdays[i];
				dateLabel.TextAlignment = UITextAlignment.Center;
				dateLabel.Font = UIFont.SystemFontOfSize(12);

				View.Add(dateLabel);
			}

			View.Add(bar);
			View.Add(collectionView);

			base.ViewDidLoad();
		}

		public async void refreshCalendar(object o, EventArgs e)
		{
			await DynamoDbService.getEvents();
			collectionView.ReloadData();
			refresh.EndRefreshing();
		}

		class CalendarSource : UICollectionViewSource
		{
			public override nint GetItemsCount(UICollectionView collectionView, nint section)
			{
				return days.Count;
			}

			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
			{
				int eventNum = 0;
				DateTime date = days[indexPath.Row];

				var cell = (CalendarCell)collectionView.DequeueReusableCell(CalendarCell.CellId, indexPath);
				cell.Date = date.Day.ToString();

				foreach (ServiceEvent e in ClubInfo.events)
				{
					if (e.date == date)
						eventNum++;
				}

				cell.eventCount = eventNum.ToString();

				return cell;
			}

			public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
			{
				return true;
			}
		}

		class CalendarCollectionViewDelegate : UICollectionViewDelegate
		{
			public UIViewController view;

			public CalendarCollectionViewDelegate(UIViewController view)
			{
				this.view = view;
			}

			public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
			{
				UserInfo.IOS_SELECTED_DATE = days[indexPath.Row];

				UIViewController eventList = view.Storyboard.InstantiateViewController("EventListController");
				eventList.Title = "Events";
				eventList.View.AddSubview(new UILabel(new CoreGraphics.CGRect(0, 0, 100, 100)));

				view.NavigationController.PushViewController(eventList, true);
			}
		}

		class CalendarCell : UICollectionViewCell
		{
			UILabel dateLabel;
			UILabel eventCountLabel;
			UIImageView imageView;

			public static readonly NSString CellId = new NSString("CalendarCell");

			public string Date
			{
				get { return dateLabel.Text; }
				set
				{
					//if (Date != null && DateTime.Parse(value).Month != UserInfo.IOS_SELECTED_DATE.Month)
					//	dateLabel.TextColor = UIColor.LightGray;

					dateLabel.Text = value;
					SetNeedsDisplay();
				}
			}

			public new string eventCount
			{
				get { return eventCountLabel.Text; }
				set
				{
					eventCountLabel.Text = value;
					SetNeedsDisplay();

					if (int.Parse(value) == 0)
					{
						eventCountLabel.Hidden = true;
						imageView.Hidden = true;
					}
					else
					{
						eventCountLabel.Hidden = false;
						imageView.Hidden = false;
					}
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
			CalendarCell(RectangleF frame) : base(frame)
			{
				dateLabel = new UILabel(new CoreGraphics.CGRect(0, 0, cellWidth, 20))
				{
					TextColor = UIColor.Black,
					Font = UIFont.SystemFontOfSize(12),
					//make bold
					TextAlignment = UITextAlignment.Center
				};

				imageView = new UIImageView(new CoreGraphics.CGRect(5, imagePosY, cellWidth-10, cellWidth-10)); //width account for margins
				imageView.Image = UIImage.FromBundle("Images/Circle.png");

				eventCountLabel = new UILabel(new CoreGraphics.CGRect(5, imagePosY, cellWidth-10, cellWidth-10))
				{
					TextColor = UIColor.White,
					Font = UIFont.SystemFontOfSize(14, UIFontWeight.Bold),

					TextAlignment = UITextAlignment.Center,

				};

				ContentView.AddSubview(imageView);
				ContentView.AddSubview(eventCountLabel);
				ContentView.AddSubview(dateLabel);
			}
		}

		public void init(DateTime dt)
		{
			int daysToAdd = 42;
			days.Clear();
			days = new List<DateTime>(42);
			dt = new DateTime(dt.Year, dt.Month, 1);

			switch (dt.DayOfWeek)
			{
				case DayOfWeek.Saturday:
					days.Add(dt.AddDays(-6));
					daysToAdd--;
					goto case DayOfWeek.Friday;
				case DayOfWeek.Friday:
					days.Add(dt.AddDays(-5));
					daysToAdd--;
					goto case DayOfWeek.Thursday;
				case DayOfWeek.Thursday:
					days.Add(dt.AddDays(-4));
					daysToAdd--;
					goto case DayOfWeek.Wednesday;
				case DayOfWeek.Wednesday:
					days.Add(dt.AddDays(-3));
					daysToAdd--;
					goto case DayOfWeek.Tuesday;
				case DayOfWeek.Tuesday:
					days.Add(dt.AddDays(-2));
					daysToAdd--;
					goto case DayOfWeek.Monday;
				case DayOfWeek.Monday:
					days.Add(dt.AddDays(-1));
					daysToAdd--;
					break;
			}

			for (int i = 0; i < daysToAdd; i++)
				days.Add(dt.AddDays(i));
		}
	}
}