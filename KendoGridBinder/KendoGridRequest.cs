using System.Collections.Generic;
using KendoGridBinder.Containers;

namespace KendoGridBinder
{
    public class KendoGridRequest
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Logic { get; set; }

        internal FilterObjectWrapper FilterObjectWrapper { get; set; }
        internal IEnumerable<SortObject> SortObjects { get; set; }
    }
}
