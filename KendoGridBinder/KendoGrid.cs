using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace KendoGridBinder
{
    public class KendoGrid<T>
    {
        public KendoGrid(KendoGridRequest request, IQueryable<T> query)
        {
            // call another method here to get filtering and sorting.

            var filtering = GetFiltering(request);
            var sorting = GetSorting(request);

            data = query
                .Where(filtering)
                .OrderBy(sorting).ToList();

            total = data.Count();

            data = data
                .Skip(request.Skip)
                .Take(request.Take);
        }

        public KendoGrid(KendoGridRequest request, IEnumerable<T> list)
        {
            var filtering = GetFiltering(request);
            var sorting = GetSorting(request);

            data = list.AsQueryable()
                .Where(filtering)
                .OrderBy(sorting).ToList();

            total = data.Count();

            data = data
                .Skip(request.Skip)
                .Take(request.Take);
        }

        public IEnumerable<T> data { get; set; }
        public int total { get; set; }


        private static string GetSorting(KendoGridRequest request)
        {
            var expression = "";

            foreach (var sortObject in request.SortObjects)
            {
                expression += sortObject.Field + " " + sortObject.Direction + ", ";
            }

            if (expression.Length < 2)
                return "true";

            expression = expression.Substring(0, expression.Length - 2);

            return expression;
        }

        private string GetFiltering(KendoGridRequest request)
        {
            var finalExpression = "";

            foreach (var filterObject in request.FilterObjectWrapper.FilterObjects)
            {
                if (finalExpression.Length > 0)
                    finalExpression += " " + request.FilterObjectWrapper.LogicToken + " ";


                if (filterObject.IsConjugate)
                {
                    var expression1 = GetExpression(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    var expression2 = GetExpression(filterObject.Field2, filterObject.Operator2, filterObject.Value2);
                    var combined = string.Format("({0} {1} {2})", expression1, request.FilterObjectWrapper.LogicToken, expression2);
                    finalExpression += combined;
                }
                else
                {
                    var expression = GetExpression(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    finalExpression += expression;
                }
            }

            if (finalExpression.Length == 0)
                return "true";

            return finalExpression;
        }

        private static string GetExpression(string field, string op, string param)
        {
            var dataType = typeof (T).GetProperty(field).PropertyType.Name.ToLower();
            var caseMod = "";

            if (dataType == "string")
            {
                param = @"""" + param.ToLower() + @"""";
                caseMod = ".ToLower()";
            }

            if(dataType == "datetime")
            {
                var date = DateTime.Parse(param);
                var str = string.Format("DateTime({0}, {1}, {2})", date.Year, date.Month, date.Day);
                param = str;
            }

            string exStr;

            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0}{2} == {1}", field, param, caseMod);
                    break;

                case "neq":
                    exStr = string.Format("{0}{2} != {1}", field, param, caseMod);
                    break;

                case "contains":
                    exStr = string.Format("{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "startswith":
                    exStr = string.Format("{0}{2}.StartsWith({1})", field, param, caseMod);
                    break;

                case "endswith":
                    exStr = string.Format("{0}{2}.EndsWith({1})", field, param, caseMod);
                    break;
                case "gte":
                    exStr = string.Format("{0}{2} >= {1}", field, param, caseMod);
                    break;
                case "gt":
                    exStr = string.Format("{0}{2} > {1}", field, param, caseMod);
                    break;
                case "lte":
                    exStr = string.Format("{0}{2} <= {1}", field, param, caseMod);
                    break;
                case "lt":
                    exStr = string.Format("{0}{2} < {1}", field, param, caseMod);
                    break;
                default:
                    exStr = "";
                    break;
            }

            return exStr;
        }
    }
}
