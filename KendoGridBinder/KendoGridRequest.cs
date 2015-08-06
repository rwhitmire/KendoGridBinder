using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using KendoGridBinder.Containers;

namespace KendoGridBinder
{
    [ModelBinder(typeof(KendoGridModelBinder))]
    public class KendoGridRequest
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Logic { get; set; }

        public FilterObjectWrapper FilterObjectWrapper { get; set; }
        public IEnumerable<SortObject> SortObjects { get; set; }
    }

    public static class Extensions
    {

        public static string GetSorting(this KendoGridRequest request)
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

        public static string GetFiltering<T>(this KendoGridRequest request)
        {
            var finalExpression = "";

            foreach (var filterObject in request.FilterObjectWrapper.FilterObjects)
            {
                if (finalExpression.Length > 0)
                    finalExpression += " " + request.FilterObjectWrapper.LogicToken + " ";


                if (filterObject.IsConjugate)
                {
                    var expression1 = GetExpression<T>(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    var expression2 = GetExpression<T>(filterObject.Field2, filterObject.Operator2, filterObject.Value2);
					var combined = string.Format("({0} {1} {2})", expression1, filterObject.LogicToken, expression2);
					finalExpression += combined;
                }
                else
                {
                    var expression = GetExpression<T>(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    finalExpression += expression;
                }
            }

            if (finalExpression.Length == 0)
                return "true";

            return finalExpression;
        }

        private static string GetExpression<T>(string field, string op, string param)
        {
            var p = typeof(T).GetProperty(field);

            var dataType = (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ?
                p.PropertyType.GetGenericArguments()[0].Name.ToLower() : p.PropertyType.Name.ToLower();


            var caseMod = "";
            if (dataType == "string")
            {
                param = @"""" + param.ToLower() + @"""";
                caseMod = ".ToLower()";
            }

            if (dataType == "datetime")
            {
                var i = param.IndexOf("GMT", StringComparison.Ordinal);
                if (i > 0)
                {
                    param = param.Remove(i);
                }
                var date = DateTime.Parse(param, new CultureInfo("en-US"));

                var str = string.Format("DateTime({0}, {1}, {2})", date.Year, date.Month, date.Day);
                param = str;
            }

            switch (op)
            {
                case "eq":
                    return string.Format("{0}{2} == {1}", field, param, caseMod);

                case "neq":
                    return string.Format("{0}{2} != {1}", field, param, caseMod);

                case "contains":
                    return string.Format("{0}{2}.Contains({1})", field, param, caseMod);

                case "startswith":
                    return string.Format("{0}{2}.StartsWith({1})", field, param, caseMod);

                case "endswith":
                    return string.Format("{0}{2}.EndsWith({1})", field, param, caseMod);

                case "gte":
                    return string.Format("{0}{2} >= {1}", field, param, caseMod);

                case "gt":
                    return string.Format("{0}{2} > {1}", field, param, caseMod);

                case "lte":
                    return string.Format("{0}{2} <= {1}", field, param, caseMod);
                    
                case "lt":
                    return string.Format("{0}{2} < {1}", field, param, caseMod);
                    
                default:
                    return "";
            }
        }
    }
}
