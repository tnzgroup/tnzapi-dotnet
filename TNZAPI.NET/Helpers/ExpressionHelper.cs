using System.Linq.Expressions;

namespace TNZAPI.NET.Helpers
{
    public class ExpressionHelper
    {
        private class ParameterTypeVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression parameterExpression;

            public ParameterTypeVisitor(ParameterExpression parameterExpression)
            {
                this.parameterExpression = parameterExpression;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return parameterExpression;
            }
        }

        public static Expression<Func<TNew, object>> ConvertExpressionParameterType<TOld, TNew>(Expression<Func<TOld, object>> expression)
        {
            var parameter = Expression.Parameter(typeof(TNew), "x");
            var visitor = new ParameterTypeVisitor(parameter);
            var newBody = visitor.Visit(expression.Body);
            return Expression.Lambda<Func<TNew, object>>(newBody, parameter);
        }
    }
}
