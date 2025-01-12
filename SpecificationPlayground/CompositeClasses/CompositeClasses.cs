namespace SpecificationPlayground.CompositeClasses
{
    // Specification.cs
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
    }

    public class NameSpecification : ISpecification<string>
    {
        private readonly string _name;

        public NameSpecification(string name)
        {
            _name = name;
        }

        public bool IsSatisfiedBy(string entity)
        {
            return entity.Equals(_name, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class CategoriaEspecificacion : ISpecification<Producto>
    {
        private readonly string _categoria;

        public CategoriaEspecificacion(string categoria)
        {
            _categoria = categoria;
        }

        public bool IsSatisfiedBy(Producto producto)
        {
            return producto.Categoria.Equals(_categoria, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class PrecioEspecificacion : ISpecification<Producto>
    {
        private readonly decimal _precioMin;

        public PrecioEspecificacion(decimal precioMin)
        {
            _precioMin = precioMin;
        }

        public bool IsSatisfiedBy(Producto producto)
        {
            return producto.Precio >= _precioMin;
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

        public bool IsSatisfiedBy(T entity)
        {
            return _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
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

        public bool IsSatisfiedBy(T entity)
        {
            return _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
        }
    }

    public class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return !_specification.IsSatisfiedBy(entity);
        }
    }

    class ProgramCompositeClasses
    {
        static void MainProgramCompositeClasses()
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
            var especificacionCategoria = new CategoriaEspecificacion("Electrónica");
            var especificacionPrecio = new PrecioEspecificacion(500m);
            var especificacionEnStock = new EnStockEspecificacion();

            // Combinar especificaciones
            var especificacionCombinada = new AndSpecification<Producto>(
                especificacionCategoria,
                new AndSpecification<Producto>(especificacionPrecio, especificacionEnStock)
            );

            // Filtrar productos
            var productosFiltrados = productos.Where(p => especificacionCombinada.IsSatisfiedBy(p));

            // Mostrar resultados
            Console.WriteLine("Productos que cumplen con las especificaciones:");
            foreach (var producto in productosFiltrados)
            {
                Console.WriteLine($"- {producto.Nombre} ({producto.Categoria}) - ${producto.Precio} - En stock: {producto.EnStock}");
            }
        }
    }

    // Especificación adicional para EnStock
    public class EnStockEspecificacion : ISpecification<Producto>
    {
        public bool IsSatisfiedBy(Producto producto)
        {
            return producto.EnStock;
        }
    }
}
