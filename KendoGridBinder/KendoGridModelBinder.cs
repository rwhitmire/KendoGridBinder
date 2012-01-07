using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KendoGridBinder.Containers;

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

            var filtering = GetFilterObjects(filterKeys, filterLogic);
            var sorting = GetSortObjects(sortKeys);

            var gridObject = new KendoGridRequest
            {
                Take = take,
                Skip = skip,
                Page = page,
                PageSize = pageSize,
                FilterObjectWrapper = filtering,
                SortObjects = sorting
            };

            return gridObject;
        }

        private IEnumerable<SortObject> GetSortObjects(IEnumerable<string> sortKeys)
        {
            var list = new List<SortObject>();

            var fields = new List<string>();
            var directions = new List<string>();

            foreach (var sortKey in sortKeys)
            {
                if (sortKey.Contains("field"))
                    fields.Add(GetQueryStringValue(sortKey));

                if (sortKey.Contains("dir"))
                    directions.Add(GetQueryStringValue(sortKey));
            }

            foreach (var field in fields)
            {
                var index = fields.IndexOf(field);
                var direction = directions[index];
                var obj = new SortObject(field, direction);
                list.Add(obj);
            }

            return list;
        }

        private FilterObjectWrapper GetFilterObjects(IList<string> filterKeys, string filterLogic)
        {
            var list = new List<FilterObject>();

            var fieldKeys = from x in filterKeys
                            where x.Contains("field")
                            select x;

            var indexList = GetIndexArr(fieldKeys);

            foreach (var index in indexList)
            {
                var group = (from x in filterKeys
                             where GetFilterIndex(x) == index &&
                                   !x.Contains("logic")
                             select x).ToList();

                var filterObject = new FilterObject
                {
                    Field1 = GetQueryStringValue(group[0]),
                    Operator1 = GetQueryStringValue(group[1]),
                    Value1 = GetQueryStringValue(group[2])
                };

                if (group.Count == 6)
                {
                    filterObject.Field2 = GetQueryStringValue(group[3]);
                    filterObject.Operator2 = GetQueryStringValue(group[4]);
                    filterObject.Value2 = GetQueryStringValue(group[5]);
                    filterObject.Logic = GetValue(filterKeys, index, "logic");
                }

                list.Add(filterObject);
            }

            return new FilterObjectWrapper(filterLogic, list);
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

                if (!existing.Any())
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
