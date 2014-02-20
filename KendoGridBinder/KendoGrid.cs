using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace KendoGridBinder
{
    public class KendoGrid<T>
    {
        /// <summary>
        /// KendoGrid constructor - IQueryable set
        /// </summary>
        /// <param name="request">KendoGridRequest object</param>
        /// <param name="query">IQueryable object representing unrealized set of data</param>
        public KendoGrid(KendoGridRequest request, IQueryable<T> query)
        {
            // call another method here to get filtering and sorting.

            var filtering = request.GetFiltering<T>();
            var sorting = request.GetSorting();

    		var tempQuery = query
				.Where(filtering)
				.OrderBy(sorting);

			Total = tempQuery
			    .Count();

			Data = tempQuery
				.Skip(request.Skip)
				.Take(request.Take);
        }

        /// <summary>
        /// KendoGrid constructor - IEnumerable set, will evaluate set AsQueryable()
        /// </summary>
        /// <param name="request">KendoGridRequest object</param>
        /// <param name="list">IEnumerable set representing a page of data</param>
        public KendoGrid(KendoGridRequest request, IEnumerable<T> list)
        {
            var filtering = request.GetFiltering<T>();
            var sorting = request.GetSorting();

            Data = list.AsQueryable()
                .Where(filtering)
                .OrderBy(sorting).ToList();

            Total = Data.Count();

            Data = Data
                .Skip(request.Skip)
                .Take(request.Take);
        }
        

        /// <summary>
        /// KendoGrid constructor - IEnumerable set, no extra evaluation (pre-paged or static)
        /// </summary>
        /// <param name="list">IEnumerable set representing a page of data</param>
        /// <param name="totalCount">Total Count of items in set</param>
        public KendoGrid(IEnumerable<T> list, int totalCount)
        {
            // Just use the request as a container
            Data = list;
            Total = totalCount;
        }

        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }

    }
}
