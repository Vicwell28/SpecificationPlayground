using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPlayground
{
    public class PrecioMinimoSpecification : Specification<Producto>
    {
        private readonly decimal _precioMinimo;

        public PrecioMinimoSpecification(decimal precioMinimo)
        {
            _precioMinimo = precioMinimo;
        }

        public override bool IsSatisfiedBy(Producto producto)
        {
            return producto.Precio >= _precioMinimo;
        }
    }

    public class CategoriaSpecification : Specification<Producto>
    {
        private readonly string _categoria;

        public CategoriaSpecification(string categoria)
        {
            _categoria = categoria;
        }

        public override bool IsSatisfiedBy(Producto producto)
        {
            return producto.Categoria.Equals(_categoria, StringComparison.OrdinalIgnoreCase);
        }
    }

}
