namespace KebabGGbab.Tracking
{
	public class Unsubscriber<T> : IDisposable
	{
		private protected readonly ICollection<IObserver<T>> _observers;
		private protected readonly IObserver<T> _observer;
		private protected bool _disposed;

		public Unsubscriber(ICollection<IObserver<T>> observers, IObserver<T> observer)
		{
			_observers = observers;
			_observer = observer;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				if (_observers != null && _observer != null)
				{
					_observers.Remove(_observer);
				}
			}

			_disposed = true;
		}

		~Unsubscriber()
		{
			Dispose(false);
		}
	}
}
