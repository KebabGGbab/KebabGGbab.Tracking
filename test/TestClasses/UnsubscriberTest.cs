namespace KebabGGbab.Tracking.Test.TestClasses
{
	[TestClass]
	public sealed class UnsubscriberTest
	{
		[TestMethod]
		public void Dispose_DeleteItemFromCollection_ItemNotInCollection()
		{
			List<IObserver<int>> trackers = [new Tracker<int>(), new Tracker<int>(), new Tracker<int>()];
			IObserver<int> tracker = trackers[2];
			Unsubscriber<int> unsubscriber = new(trackers, tracker);

			unsubscriber.Dispose();

			CollectionAssert.DoesNotContain(trackers, tracker);
		}

		[TestMethod]
		public void Dispose_DisposeBetweenEvents_EventsAfterDisposeIgnored()
		{
			int score = 0;
			Trackable<int> trackable = new();
			Tracker<int> tracker = new()
			{
				Actions = new ActionSet<int>
				{
					Next = (number) => score += number
				}
			};
			IDisposable cancellation = trackable.Subscribe(tracker);

			trackable.Track(1);
			trackable.Track(2);
			cancellation.Dispose();
			trackable.Track(3);

			Assert.AreEqual(3, score);
		}
	}
}
