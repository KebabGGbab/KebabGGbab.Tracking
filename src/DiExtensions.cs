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

		public static IServiceCollection AddTracking<T>(this IServiceCollection services, object key)
		{
			return services.AddTracking<T>(options: new TrackingOptions(), key: key);
		}

		public static IServiceCollection AddTracking<T>(this IServiceCollection services, TrackingOptions options, object key)
		{
			ArgumentNullException.ThrowIfNull(services, nameof(services));
			ArgumentNullException.ThrowIfNull(key, nameof(key));

			services.TryAddKeyedSingleton<Trackable<T>>(key, (s, o) => new Trackable<T>(options));
			services.TryAddKeyedTransient<Tracker<T>>(key, (s, o) =>
			{
				Tracker<T> tracker = new();
				Trackable<T> trackable = s.GetRequiredKeyedService<Trackable<T>>(key);
				tracker.Subscribe(trackable);

				return tracker;
			});

			return services;
		}
	}
}
