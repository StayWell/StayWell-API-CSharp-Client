namespace StayWell.Interface
{
	public class PagedSearchRequest : PagedRequest
	{
		public string Query { get; set; }
        public LogicalOperator LogicalOperator { get; set; }
}
}
