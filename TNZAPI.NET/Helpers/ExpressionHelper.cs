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

        //public static Expression<Func<TNew, object>> ConvertExpressionParameterType<TOld, TNew>(Expression<Func<TOld, object>> expression)
        //{
        //    var oldParameter = expression.Parameters[0];
        //    var newParameter = Expression.Parameter(typeof(TNew), oldParameter.Name);
        //    var converter = new ParameterConverter(oldParameter, newParameter);
        //    var newBody = converter.Visit(expression.Body);
        //    var newLambda = Expression.Lambda<Func<TNew, object>>(newBody, newParameter);

        //    return newLambda;
        //}
    }
}
