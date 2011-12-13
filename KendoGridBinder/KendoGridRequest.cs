namespace KendoGridBinder
{
    public class KendoGridRequest
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Filtering { get; set; }
        public string Sorting { get; set; }
    }
}
