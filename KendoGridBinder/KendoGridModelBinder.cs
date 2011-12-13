using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendoGridBinder
{
    internal class KendoGridModelBinder : IModelBinder
    {
        private HttpRequestBase _request;

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            _request = controllerContext.HttpContext.Request;

            var take = Convert.ToInt32(GetQueryStringValue("take"));
            var page = Convert.ToInt32(GetQueryStringValue("page"));
            var skip = Convert.ToInt32(GetQueryStringValue("skip"));
            var pageSize = Convert.ToInt32(GetQueryStringValue("pageSize"));
            var filterLogic = GetQueryStringValue("filter[logic]");

            var sortKeys = (from x in GetRequestKeys()
                            where x.StartsWith("sort")
                            select x).ToList();

            var filterKeys = (from x in GetRequestKeys()
                              where x.StartsWith("filter") &&
                                    x != "filter[logic]"
                              select x).ToList();

            var filtering = GetFiltering(filterKeys, filterLogic);
            var sorting = GetSorting(sortKeys);

            var gridObject = new KendoGridRequest
            {
                Take = take,
                Skip = skip,
                Page = page,
                PageSize = pageSize,
                Filtering = filtering,
                Sorting = sorting
            };

            return gridObject;
        }

        private string GetSorting(IEnumerable<string> sortKeys)
        {
            var expression = "";

            foreach (var sortKey in sortKeys)
            {
                if (sortKey.Contains("field"))
                    expression += GetQueryStringValue(sortKey) + " ";

                if (sortKey.Contains("dir"))
                    expression += GetQueryStringValue(sortKey) + ", ";
            }

            if (expression.Length > 2)
                return expression.Substring(0, expression.Length - 2);

            return "true";
        }

        private string GetFiltering(IList<string> filterKeys, string filterLogic)
        {
            var filter = "";

            var fieldKeys = from x in filterKeys
                            where x.Contains("field")
                            select x;

            var iteration = 0;

            var indexList = GetIndexArr(fieldKeys);

            foreach (var index in indexList)
            {
                var group = (from x in filterKeys
                             where GetFilterIndex(x) == index &&
                                   !x.Contains("logic")
                             select x).ToList();

                var filterQueryObject = new FilterQueryObject
                {
                    Field1 = GetQueryStringValue(group[0]),
                    Operator1 = GetQueryStringValue(group[1]),
                    Value1 = GetQueryStringValue(group[2])
                };

                if (group.Count == 6)
                {
                    filterQueryObject.Field2 = GetQueryStringValue(group[3]);
                    filterQueryObject.Operator2 = GetQueryStringValue(group[4]);
                    filterQueryObject.Value2 = GetQueryStringValue(group[5]);
                    filterQueryObject.Logic = GetValue(filterKeys, index, "logic");
                }

                if (iteration != 0)
                    filter = filter + " " + filterLogic + " " + filterQueryObject.Query;
                else
                    filter = filterQueryObject.Query;

                iteration++;
            }

            if (string.IsNullOrEmpty(filter))
                filter = "true";

            return filter;
        }

        private IEnumerable<int> GetIndexArr(IEnumerable<string> fieldKeys)
        {
            var list = new List<int>();

            foreach (var fieldKey in fieldKeys)
            {
                var index = GetFilterIndex(fieldKey);

                var existing = from x in list
                               where x == index
                               select x;

                if (existing.Count() == 0)
                {
                    list.Add(index);
                }
            }

            return list;
        }

        public int GetFilterIndex(string qString)
        {
            var splitArr = qString.Split('[');

            foreach (var s in splitArr)
            {
                float result;
                var strippedVal = s.Replace("]", "");
                if (float.TryParse(strippedVal, out result))
                {
                    return (int)result;
                }
            }

            return 0;
        }

        public string GetValue(IList<string> filterKeys, int index, string type)
        {
            var fieldKeys = from x in filterKeys
                            where x.Contains(type)
                            select x;

            foreach (var fieldKey in fieldKeys)
            {
                var filterIndex = GetFilterIndex(fieldKey);
                if (filterIndex == index)
                {
                    return GetQueryStringValue(fieldKey);
                }
            }

            return null;
        }

        private string GetQueryStringValue(string key)
        {
            if (_request.HttpMethod.ToUpper() == "POST")
                return _request.Form[key];
        
            return _request.QueryString[key];
        }

        private IEnumerable<string> GetRequestKeys()
        {
            if (_request.HttpMethod.ToUpper() == "POST")
                return _request.Form.AllKeys;

            return _request.QueryString.AllKeys;
        }
    }
}
