using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KebabGGbab.Tracking
{
	public static class DiExtensions
	{
		public static IServiceCollection AddTracking<T>(this IServiceCollection services)
		{
			return services.AddTracking<T>(options: new TrackingOptions());
		}

		public static IServiceCollection AddTracking<T>(this IServiceCollection services, TrackingOptions options)
		{
			ArgumentNullException.ThrowIfNull(services, nameof(services));

			services.TryAddSingleton<Trackable<T>>((s) => new Trackable<T>(options));
			services.TryAddTransient<Tracker<T>>((s) =>
			{
				Tracker<T> tracker = new();
				Trackable<T> trackable = s.GetRequiredService<Trackable<T>>();
				tracker.Subscribe(trackable);

				return tracker;
			});

			return services;
		}
	}
}
