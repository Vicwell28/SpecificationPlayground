using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPlayground
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);

        // Permite combinar especificaciones con operadores lógicos
        ISpecification<T> And(ISpecification<T> other);
        ISpecification<T> Or(ISpecification<T> other);
        ISpecification<T> Not();
    }
}
