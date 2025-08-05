namespace KebabGGbab.Tracking
{
	public class Tracker<T> : IObserver<T>
	{
		private protected IDisposable? _cancellation;

		public ActionSet<T> Actions { get; init; } = new();

		public Tracker() { }

		public Tracker(ActionSet<T> actions)
		{
			Actions = actions;
		}

		public virtual void Subscribe(IObservable<T> observable)
		{
			ArgumentNullException.ThrowIfNull(observable);

			_cancellation = observable.Subscribe(this);
		}

		public virtual void Unsubscribe()
		{
			_cancellation?.Dispose();
		}

		public virtual void OnCompleted()
		{
			Actions.Completed?.Invoke();
		}

		public virtual void OnError(Exception error)
		{
			Actions.Error?.Invoke();
		}

		public virtual void OnNext(T value)
		{
			Actions.Next?.Invoke(value);
		}
	}
}
