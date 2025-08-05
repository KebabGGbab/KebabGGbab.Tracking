namespace KebabGGbab.Tracking.Test.TestClasses
{
	[TestClass]
	public sealed class TrackableTest
	{
		[TestMethod]
		public void Subscribe_SubscribeAfterAddSomeEvent_NewSubscriberCallsAllEvent()
		{
			TrackingOptions options = new()
			{
				Capacity = 3
			};
			Trackable<int> trackable = new(options);
			trackable.Track(1);
			trackable.Track(2);
			trackable.Track(3);
			Tracker<int> tracker = new();
			int score = 0;
			tracker.Actions.Next = (number) => score += number;

			trackable.Subscribe(tracker);

			Assert.AreEqual(6, score);
		}

		[TestMethod]
		public void CompleteTracking_StopTrackingBetweenCalls_NothandlerAfterCompleteTraking()
		{
			Trackable<int> trackable = new();
			int score = 0;
			Tracker<int> tracker = new(new()
			{
				Next = (number) => score += number,
				Completed = () => score = 0
			});
			trackable.Subscribe(tracker);

			trackable.Track(1);
			trackable.Track(2);
			trackable.CompleteTracking();
			trackable.Track(3);

			Assert.AreEqual(0, score);
		}

		[TestMethod]
		public void Track_SomeTracker_EveryTrackerInvokeYourAction()
		{
			Trackable<int> trackable = new();
			int scoreA = 0;
			int scoreB = 0;
			Tracker<int> trackerA = new()
			{
				Actions = new()
				{
					Next = (number) => scoreA += number,
				}
			};
			Tracker<int> trackerB = new()
			{
				Actions = new()
				{
					Next = (number) => scoreB -= number,
				}
			};
			trackerA.Subscribe(trackable);
			trackerB.Subscribe(trackable);

			trackable.Track(1);
			trackable.Track(2);
			trackable.Track(3);

			Assert.AreEqual(6, scoreA);
			Assert.AreEqual(-6, scoreB);
		}
	}
}
