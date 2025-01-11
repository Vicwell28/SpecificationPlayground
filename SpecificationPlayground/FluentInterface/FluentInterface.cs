using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationPlayground.FluentInterface
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
        bool IsSatisfiedBy(T entity);
    }

    public class CategoriaEspecificacion : ISpecification<Producto>
    {
        private readonly string _categoria;

        public CategoriaEspecificacion(string categoria)
        {
            _categoria = categoria;
        }

        public Expression<Func<Producto, bool>> ToExpression()
        {
            return producto => producto.Categoria.Equals(_categoria, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsSatisfiedBy(Producto producto)
        {
            return ToExpression().Compile()(producto);
        }
    }

    public class PrecioEspecificacion : ISpecification<Producto>
    {
        private readonly decimal _precioMin;

        public PrecioEspecificacion(decimal precioMin)
        {
            _precioMin = precioMin;
        }

        public Expression<Func<Producto, bool>> ToExpression()
        {
            return producto => producto.Precio >= _precioMin;
        }

        public bool IsSatisfiedBy(Producto producto)
        {
            return ToExpression().Compile()(producto);
        }
    }

    public class EnStockEspecificacion : ISpecification<Producto>
    {
        public Expression<Func<Producto, bool>> ToExpression()
        {
            return producto => producto.EnStock;
        }

        public bool IsSatisfiedBy(Producto producto)
        {
            return ToExpression().Compile()(producto);
        }
    }

    public class SpecificationBuilder<T>
    {
        private Expression<Func<T, bool>> _expression;

        public SpecificationBuilder<T> Where(ISpecification<T> specification)
        {
            if (_expression == null)
            {
                _expression = specification.ToExpression();
            }
            else
            {
                _expression = ExpressionCombiner.And(_expression, specification.ToExpression());
            }
            return this;
        }

        public Expression<Func<T, bool>> Build()
        {
            return _expression ?? (x => true);
        }
    }

    public static class ExpressionCombiner
    {
        public static Expression<Func<T, bool>> And<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var combined = new ReplaceParameterVisitor(expr1.Parameters[0], parameter)
                .Visit(expr1.Body);

            var body2 = new ReplaceParameterVisitor(expr2.Parameters[0], parameter)
                .Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(combined, body2), parameter);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly ParameterExpression _newParam;

            public ReplaceParameterVisitor(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParam = oldParam;
                _newParam = newParam;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == _oldParam)
                    return _newParam;
                return base.VisitParameter(node);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // Lista de productos de ejemplo
            var productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Laptop", Categoria = "Electrónica", Precio = 1500m, EnStock = true },
                new Producto { Id = 2, Nombre = "Smartphone", Categoria = "Electrónica", Precio = 800m, EnStock = false },
                new Producto { Id = 3, Nombre = "Cafetera", Categoria = "Hogar", Precio = 120m, EnStock = true },
                new Producto { Id = 4, Nombre = "Silla", Categoria = "Muebles", Precio = 200m, EnStock = true },
                new Producto { Id = 5, Nombre = "Mesa", Categoria = "Muebles", Precio = 350m, EnStock = false },
            };

            // Definir especificaciones
            var categoriaEspec = new CategoriaEspecificacion("Muebles");
            var precioEspec = new PrecioEspecificacion(150m);
            var enStockEspec = new EnStockEspecificacion();

            // Construir especificación fluida
            var builder = new SpecificationBuilder<Producto>();
            builder
                .Where(categoriaEspec)
                .Where(precioEspec)
                .Where(enStockEspec);

            var especificacion = builder.Build();

            // Filtrar productos usando la expresión
            var productosFiltrados = productos.AsQueryable().Where(especificacion).ToList();

            // Mostrar resultados
            Console.WriteLine("Productos que cumplen con las especificaciones (Fluent):");
            foreach (var producto in productosFiltrados)
            {
                Console.WriteLine($"- {producto.Nombre} ({producto.Categoria}) - ${producto.Precio} - En stock: {producto.EnStock}");
            }
        }
    }


}
