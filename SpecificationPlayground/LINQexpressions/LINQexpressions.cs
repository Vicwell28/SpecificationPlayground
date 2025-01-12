using System.Linq.Expressions;

namespace SpecificationPlayground.LINQexpressions
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();

        // Operadores lógicos
        ISpecification<T> And(ISpecification<T> other);
        ISpecification<T> Or(ISpecification<T> other);
        ISpecification<T> Not();
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

        public ISpecification<Producto> And(ISpecification<Producto> other)
        {
            return new AndSpecification<Producto>(this, other);
        }

        public ISpecification<Producto> Or(ISpecification<Producto> other)
        {
            return new OrSpecification<Producto>(this, other);
        }

        public ISpecification<Producto> Not()
        {
            return new NotSpecification<Producto>(this);
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

        public ISpecification<Producto> And(ISpecification<Producto> other)
        {
            return new AndSpecification<Producto>(this, other);
        }

        public ISpecification<Producto> Or(ISpecification<Producto> other)
        {
            return new OrSpecification<Producto>(this, other);
        }

        public ISpecification<Producto> Not()
        {
            return new NotSpecification<Producto>(this);
        }
    }

    public class EnStockEspecificacion : ISpecification<Producto>
    {
        public Expression<Func<Producto, bool>> ToExpression()
        {
            return producto => producto.EnStock;
        }

        public ISpecification<Producto> And(ISpecification<Producto> other)
        {
            return new AndSpecification<Producto>(this, other);
        }

        public ISpecification<Producto> Or(ISpecification<Producto> other)
        {
            return new OrSpecification<Producto>(this, other);
        }

        public ISpecification<Producto> Not()
        {
            return new NotSpecification<Producto>(this);
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var leftExpr = _left.ToExpression();
            var rightExpr = _right.ToExpression();
            return leftExpr.And(rightExpr);
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    public class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var leftExpr = _left.ToExpression();
            var rightExpr = _right.ToExpression();
            return leftExpr.Or(rightExpr);
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    public class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var expr = _specification.ToExpression();
            var parameter = expr.Parameters[0];
            var body = Expression.Not(expr.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceParameterVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceParameterVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
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

    class ProgramLINQexpressions
    {
        static void MainProgramLINQexpressions()
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
            var categoriaEspec = new CategoriaEspecificacion("Electrónica");
            var precioEspec = new PrecioEspecificacion(100m);
            var enStockEspec = new EnStockEspecificacion();

            // Combinar especificaciones usando operadores lógicos
            var especificacionCombinada = categoriaEspec.And(precioEspec).And(enStockEspec);

            // Obtener la expresión combinada
            var expresion = especificacionCombinada.ToExpression();

            // Filtrar productos usando la expresión
            var productosFiltrados = productos.AsQueryable().Where(expresion).ToList();

            // Mostrar resultados
            Console.WriteLine("Productos que cumplen con las especificaciones (LINQ):");
            foreach (var producto in productosFiltrados)
            {
                Console.WriteLine($"- {producto.Nombre} ({producto.Categoria}) - ${producto.Precio} - En stock: {producto.EnStock}");
            }
        }
    }
}
