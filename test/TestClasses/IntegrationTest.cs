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
		public void AddTracking_GetSomeTrackers_DoEachActions()
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
	}
}
