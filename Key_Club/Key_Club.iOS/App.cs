﻿using System;
using EventKit;

namespace Key_Club.iOS
{
	public class App
	{
		public static App Current
		{
			get { return current; }
		}
		private static App current;

		public EKEventStore EventStore
		{
			get { return eventStore; }
		}
		protected EKEventStore eventStore;

		static App()
		{
			current = new App();
		}
		protected App()
		{
			eventStore = new EKEventStore();
		}
	}
}