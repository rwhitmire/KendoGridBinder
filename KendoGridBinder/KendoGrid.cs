using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace KendoGridBinder
{
    public class KendoGrid<T>
    {
        public KendoGrid(KendoGridRequest request, IQueryable<T> query)
        {
            data = query
                .Where(request.Filtering)
                .OrderBy(request.Sorting).ToList();

            total = data.Count();

            data = data
                .Skip(request.Skip)
                .Take(request.Take);
        }

        public KendoGrid(KendoGridRequest request, IEnumerable<T> list)
        {
            data = list.AsQueryable()
                .Where(request.Filtering)
                .OrderBy(request.Sorting).ToList();

            total = data.Count();

            data = data
                .Skip(request.Skip)
                .Take(request.Take);
        }

        public IEnumerable<T> data { get; set; }
        public int total { get; set; }
    }
}
