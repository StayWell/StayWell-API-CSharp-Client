namespace StayWell.Interface
{
	public class PagedResultList<T> : ResultList<T>
	{
		public int Total { get; set; }
		public int Offset { get; set; }
	}
}
