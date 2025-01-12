using System.Linq.Expressions;

namespace SpecificationPlayground
{
    /// <summary>
    /// Especificación para filtrar productos por categoría.
    /// </summary>
    public class CategoriaSpecification : Specification<Producto>
    {
        private readonly string _categoria;

        public CategoriaSpecification(string categoria)
        {
            _categoria = categoria;
        }

        public override Expression<Func<Producto, bool>> ToExpression()
        {
            return p => p.Categoria.Equals(_categoria, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Especificación para filtrar productos dentro de un rango de precios.
    /// </summary>
    public class PrecioRangeSpecification : Specification<Producto>
    {
        private readonly decimal? _precioMin;
        private readonly decimal? _precioMax;

        public PrecioRangeSpecification(decimal? precioMin, decimal? precioMax)
        {
            _precioMin = precioMin;
            _precioMax = precioMax;
        }

        public override Expression<Func<Producto, bool>> ToExpression()
        {
            if (_precioMin.HasValue && _precioMax.HasValue)
            {
                return p => p.Precio >= _precioMin.Value && p.Precio <= _precioMax.Value;
            }
            else if (_precioMin.HasValue)
            {
                return p => p.Precio >= _precioMin.Value;
            }
            else if (_precioMax.HasValue)
            {
                return p => p.Precio <= _precioMax.Value;
            }
            else
            {
                // Si no se especifica rango, no filtra por precio
                return p => true;
            }
        }
    }

    /// <summary>
    /// Especificación para filtrar productos que están en stock.
    /// </summary>
    public class EnStockSpecification : Specification<Producto>
    {
        private readonly bool _enStock;

        public EnStockSpecification(bool enStock)
        {
            _enStock = enStock;
        }

        public override Expression<Func<Producto, bool>> ToExpression()
        {
            return p => p.EnStock == _enStock;
        }
    }
}
