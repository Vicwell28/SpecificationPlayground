using System.Linq.Expressions;

namespace SpecificationPlayground
{
    /// <summary>
    /// Define un contrato para las especificaciones que pueden convertir criterios a expresiones y combinarse lógicamente.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Obtiene la expresión que representa el criterio de la especificación.
        /// </summary>
        Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Combina esta especificación con otra utilizando el operador lógico AND.
        /// </summary>
        /// <param name="spec">Otra especificación.</param>
        /// <returns>Una nueva especificación combinada.</returns>
        ISpecification<T> And(ISpecification<T> spec);

        /// <summary>
        /// Combina esta especificación con otra utilizando el operador lógico OR.
        /// </summary>
        /// <param name="spec">Otra especificación.</param>
        /// <returns>Una nueva especificación combinada.</returns>
        ISpecification<T> Or(ISpecification<T> spec);

        /// <summary>
        /// Negación de esta especificación.
        /// </summary>
        /// <returns>Una nueva especificación negada.</returns>
        ISpecification<T> Not();
    }
}
