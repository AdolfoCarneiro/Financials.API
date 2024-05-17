using System.Linq.Expressions;

namespace Financials.Infrastructure.Helper
{
    public class ExpressionHelper(Expression from, Expression to) : ExpressionVisitor
    {
        private readonly Expression from = from, to = to;

        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }

        public static Expression<Func<T, bool>> CombinaFiltrosAnd<T>(Expression<Func<T, bool>> filtro1, Expression<Func<T, bool>> filtro2)
        {
            var rewrittenBody1 = new ExpressionHelper(
                filtro1.Parameters[0], filtro2.Parameters[0]).Visit(filtro1.Body);
            var newFilter = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(rewrittenBody1, filtro2.Body), filtro2.Parameters);
            return newFilter;
        }
        public static Expression<Func<T, bool>> CombinaFiltrosOR<T>(Expression<Func<T, bool>> filtro1, Expression<Func<T, bool>> filtro2)
        {
            var rewrittenBody1 = new ExpressionHelper(
                filtro1.Parameters[0], filtro2.Parameters[0]).Visit(filtro1.Body);
            var newFilter = Expression.Lambda<Func<T, bool>>(
                Expression.Or(rewrittenBody1, filtro2.Body), filtro2.Parameters);
            return newFilter;
        }
    }
}
