using LinqKit;
using System.Linq.Expressions;

namespace SpecificationPlayground
{
    /// <summary>
    /// Clase base abstracta que implementa la interfaz ISpecification y proporciona métodos para combinar especificaciones.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// Obtiene la expresión que representa el criterio de la especificación.
        /// </summary>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Combina esta especificación con otra utilizando el operador lógico AND.
        /// </summary>
        /// <param name="spec">Otra especificación.</param>
        /// <returns>Una nueva especificación combinada.</returns>
        public ISpecification<T> And(ISpecification<T> spec)
        {
            return new AndSpecification<T>(this, spec);
        }

        /// <summary>
        /// Combina esta especificación con otra utilizando el operador lógico OR.
        /// </summary>
        /// <param name="spec">Otra especificación.</param>
        /// <returns>Una nueva especificación combinada.</returns>
        public ISpecification<T> Or(ISpecification<T> spec)
        {
            return new OrSpecification<T>(this, spec);
        }

        /// <summary>
        /// Negación de esta especificación.
        /// </summary>
        /// <returns>Una nueva especificación negada.</returns>
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    /// <summary>
    /// Especificación que combina dos especificaciones con un operador lógico AND.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    internal class AndSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpr = _left.ToExpression();
            var rightExpr = _right.ToExpression();

            return leftExpr.And(rightExpr);
        }
    }

    /// <summary>
    /// Especificación que combina dos especificaciones con un operador lógico OR.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    internal class OrSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpr = _left.ToExpression();
            var rightExpr = _right.ToExpression();

            return leftExpr.Or(rightExpr);
        }
    }

    /// <summary>
    /// Especificación que niega otra especificación.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    internal class NotSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _spec;

        public NotSpecification(ISpecification<T> spec)
        {
            _spec = spec;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expr = _spec.ToExpression();
            return expr.Not();
        }
    }
}
