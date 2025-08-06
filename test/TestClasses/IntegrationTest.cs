using Microsoft.Extensions.DependencyInjection;

namespace KebabGGbab.Tracking.Test.TestClasses
{
	[TestClass]
	public sealed class IntegrationTest
	{
		[TestMethod]
		public void AddTracking_ServiceCollectionIsNull_Throws()
		{
			IServiceCollection services = null!;

			Assert.Throws<ArgumentNullException>(() => services.AddTracking<int>());
		}

		[TestMethod]
		public void AddTracking_GetSomeTrackers_AllTrackersDifferentObjects()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddTracking<int>(new TrackingOptions());
			IServiceProvider provider = services.BuildServiceProvider();
			int scoreA = 0;
			int scoreB = 0;

			Trackable<int> trackable = provider.GetRequiredService<Trackable<int>>();
			Tracker<int> trackerA = provider.GetRequiredService<Tracker<int>>();
			Tracker<int> trackerB = provider.GetRequiredService<Tracker<int>>();
			trackerA.Actions.Next = (number) => scoreA += number;
			trackerB.Actions.Next = (number) => scoreB -= number;
			trackable.Track(1);
			trackable.Track(2);
			trackable.Track(3);

			Assert.AreEqual(6, scoreA);
			Assert.AreEqual(-6, scoreB);
		}

		[TestMethod]
		public void AddTracking_GetTrackableSomeTimes_LinksOneObject()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddTracking<int>();
			IServiceProvider provider = services.BuildServiceProvider();

			Trackable<int> trackableA = provider.GetRequiredService<Trackable<int>>();
			Trackable<int> trackableB = provider.GetRequiredService<Trackable<int>>();

			Assert.AreEqual(trackableA, trackableB);
		}

		[TestMethod]
		public void AddTracking_KeyIsNull_Throws()
		{
			IServiceCollection services = new ServiceCollection();

			Assert.Throws<ArgumentNullException>(() => services.AddTracking<int>(key: null!));
		}

		[TestMethod]
		public void AddTracking_GeyKeyedTrackableAndTrackers_EachKeyReturnsDifferentObject()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddTracking<int>(1);
			services.AddTracking<int>(2);
			IServiceProvider provider = services.BuildServiceProvider();
			int scoreA = 0;
			int scoreB = 0;
			int scoreC = 10;
			int scoreD = 10;

			Trackable<int> trackableA = provider.GetRequiredKeyedService<Trackable<int>>(1);
			Trackable<int> trackableB = provider.GetRequiredKeyedService<Trackable<int>>(2);
			Tracker<int> trackerA = provider.GetRequiredKeyedService<Tracker<int>>(1);
			Tracker<int> trackerB = provider.GetRequiredKeyedService<Tracker<int>>(1);
			Tracker<int> trackerC = provider.GetRequiredKeyedService<Tracker<int>>(2);
			Tracker<int> trackerD = provider.GetRequiredKeyedService<Tracker<int>>(2);
			trackerA.Actions.Next = (number) => scoreA += number;
			trackerB.Actions.Next = (number) => scoreB -= number;
			trackerC.Actions.Next = (number) => scoreC *= number;
			trackerD.Actions.Next = (number) => scoreD /= number;
			trackableA.Track(1);
			trackableA.Track(2);
			trackableA.Track(5);
			trackableB.Track(1);
			trackableB.Track(2);
			trackableB.Track(5);

			Assert.AreEqual(8, scoreA);
			Assert.AreEqual(-8, scoreB);
			Assert.AreEqual(100, scoreC);
			Assert.AreEqual(1, scoreD);
		}

		[TestMethod]
		public void AddTracking_GetKeyedTrackableSomeTimes_LinksOneObject()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddTracking<int>(nameof(Int32));
			IServiceProvider provider = services.BuildServiceProvider();

			Trackable<int> trackableA = provider.GetRequiredKeyedService<Trackable<int>>(nameof(Int32));
			Trackable<int> trackableB = provider.GetRequiredKeyedService<Trackable<int>>(nameof(Int32));

			Assert.AreEqual(trackableA, trackableB);
		}

		[TestMethod]
		public void AddTracking_GetDifferentKeyedTrackableSomeTimes_LinksDifferentObject()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddTracking<int>(nameof(Int32));
			services.AddTracking<int>(services);
			IServiceProvider provider = services.BuildServiceProvider();

			Trackable<int> trackableA = provider.GetRequiredKeyedService<Trackable<int>>(nameof(Int32));
			Trackable<int> trackableB = provider.GetRequiredKeyedService<Trackable<int>>(services);

			Assert.AreNotEqual(trackableA, trackableB);
		}
	}
}
