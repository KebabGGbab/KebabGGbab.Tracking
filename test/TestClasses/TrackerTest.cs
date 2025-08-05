namespace KebabGGbab.Tracking.Test.TestClasses
{
	[TestClass]
	public sealed class TrackerTest
	{
		[TestMethod]
		public void Subscribe_TrackableIsNull_Throw()
		{
			Trackable<int>? trackable = null;
			Tracker<int> tracker = new();

			Assert.Throws<ArgumentNullException>(() => 
			{ 
				tracker.Subscribe(trackable!); 
			});
		}

		[TestMethod]
		public void Unsubscribe_UnsubscribeBetweenTracks_TracksAfterUnsubscribeIgnored()
		{
			Trackable<int> trackable = new();
			Tracker<int> tracker = new();
			tracker.Subscribe(trackable);
			int calls = 0;
			tracker.Actions.Next = (number) => calls++;

			trackable.Track(0);
			tracker.Unsubscribe();
			trackable.Track(0);

			Assert.AreEqual(1, calls);
		}

		[TestMethod] 
		public void OnNext_ActionNextIsNull_NotThrow()
		{
			Tracker<int> tracker = new() { };

			tracker.OnNext(0);

			Assert.IsNull(tracker.Actions.Next);
		}

		[TestMethod]
		public void OnCompleted_ActionCompletedIsNull_NotThrow()
		{
			Tracker<int> tracker = new() { };

			tracker.OnCompleted();

			Assert.IsNull(tracker.Actions.Completed);
		}

		[TestMethod]
		public void OnError_ActionErrorIsNull_NotThrow()
		{
			Tracker<int> tracker = new() { };

			tracker.OnError(new ArgumentNullException());

			Assert.IsNull(tracker.Actions.Error);
		}
	}
}
