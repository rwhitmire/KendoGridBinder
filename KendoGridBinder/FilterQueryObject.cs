namespace KendoGridBinder
{
    internal class FilterQueryObject
    {
        public int Index { get; set; }

        public string Field1 { get; set; }
        public string Operator1 { get; set; }
        public string Value1 { get; set; }

        public string Field2 { get; set; }
        public string Operator2 { get; set; }
        public string Value2 { get; set; }

        public string Logic { get; set; }

        public string GetQuery()
        {
                if (!string.IsNullOrEmpty(Field2) && string.IsNullOrEmpty(Logic))
                    Logic = "and";

                string expr1 = null;
                string expr2 = null;

                if (!string.IsNullOrEmpty(Field1))
                    expr1 = GetExpression(Field1, Operator1, Value1);


                if (!string.IsNullOrEmpty(Field2))
                    expr2 = GetExpression(Field2, Operator2, Value2);

                var query = "";

                if (!string.IsNullOrEmpty(expr1))
                    query += expr1;

                if (!string.IsNullOrEmpty(expr2))
                    query += " " + Logic + " " + expr2;

                query = string.Format("({0})", query);

                return query;
        }

        private static string GetExpression(string field, string op, string param)
        {
            param = @"""" + param + @"""";

            var exStr = string.Empty;

            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0} == {1}", field, param);
                    break;

                case "neq":
                    exStr = string.Format("{0} != {1}", field, param);
                    break;

                case "contains":
                    exStr = string.Format("{0}.Contains({1})", field, param);
                    break;

                case "startswith":
                    exStr = string.Format("{0}.StartsWith({1})", field, param);
                    break;

                case "endswith":
                    exStr = string.Format("{0}.EndsWith({1})", field, param);
                    break;
            }

            return exStr;
        }
    }
}
