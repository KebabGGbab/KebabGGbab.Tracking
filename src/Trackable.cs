namespace KebabGGbab.Tracking
{
	public class Trackable<T> : IObservable<T>
	{
		private protected readonly List<IObserver<T>> _observers = [];
		private protected readonly Queue<T> _events;
		private readonly TrackingOptions _options;

		public Trackable() : this (new TrackingOptions()) { }

		public Trackable(TrackingOptions options)
		{
			_options = options;
			_events = new Queue<T>(options.Capacity);
		}

		public virtual IDisposable Subscribe(IObserver<T> observer)
		{
			if (!_observers.Contains(observer))
			{
				_observers.Add(observer);

				foreach (T @event in _events)
				{
					observer.OnNext(@event);
				}
			}

			return new Unsubscriber<T>(_observers, observer);
		}

		public virtual void Track(T @event)
		{
			if (_events.Count == _options.Capacity)
			{
				_events.Dequeue();
			}

			_events.Enqueue(@event);

			foreach (IObserver<T> observer in _observers)
			{
				observer.OnNext(@event);
			}
		}

		public virtual void CompleteTracking()
		{
			foreach (IObserver<T> observer in _observers)
			{
				observer.OnCompleted();
			}

			_observers.Clear();
			_events.Clear();
		}
	}
}
