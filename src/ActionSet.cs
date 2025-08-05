namespace KebabGGbab.Tracking
{
	public class ActionSet<T>
	{
		public Action<T>? Next { get; set; }
		public Action? Completed { get; set; }
		public Action? Error { get; set; }
	}
}
