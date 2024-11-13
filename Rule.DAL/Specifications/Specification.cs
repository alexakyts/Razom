using Rule.DAL.Specifications.Interfaces;
using System.Linq.Expressions;

namespace Rule.DAL.Specifications
{
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; private set; } = new();
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected Specification<TEntity> Where(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
            return this;
        }
        protected Specification<TEntity> Include(Expression<Func<TEntity, object>> include)
        {
            Includes.Add(include);
            return this;
        }
        protected Specification<TEntity> OrderByFunc(Expression<Func<TEntity, object>> orederBy)
        {
            OrderBy = orederBy;
            return this;
        }
        protected Specification<TEntity> OrderByDescendingFunc(Expression<Func<TEntity, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
            return this;
        }

        protected Specification<TEntity> And(Specification<TEntity> specification)
        {
            if (specification is not null)
            {
                if (Criteria is null)
                    Criteria = specification.Criteria;
                else
                    Criteria = Criteria.And(specification.Criteria);
            }

            Includes.AddRange(specification.Includes);
            OrderBy ??= specification.OrderBy;
            OrderByDescending ??= specification.OrderByDescending;

            return this;
        }

        protected Specification<TEntity> Or(Specification<TEntity> specification)
        {
            if (specification is not null)
            {
                if (Criteria is null)
                    Criteria = specification.Criteria;
                else
                    Criteria = Criteria.Or(specification.Criteria);
            }

            Includes.AddRange(specification.Includes);
            OrderBy ??= specification.OrderBy;
            OrderByDescending ??= specification.OrderByDescending;

            return this;
        }
    }
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));
            var combined = Expression.AndAlso(Expression.Invoke(left, parameter), Expression.Invoke(right, parameter));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));
            var combined = Expression.OrElse(Expression.Invoke(left, parameter), Expression.Invoke(right, parameter));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}
