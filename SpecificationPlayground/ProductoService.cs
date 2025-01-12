using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace SpecificationPlayground
{
    /// <summary>
    /// Servicio para manejar operaciones relacionadas con productos.
    /// </summary>
    public class ProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca productos que cumplan con las especificaciones proporcionadas.
        /// </summary>
        /// <param name="spec">Especificación que define los criterios de búsqueda.</param>
        /// <returns>Lista de productos que cumplen con los criterios.</returns>
        public async Task<List<Producto>> BuscarProductosAsync(ISpecification<Producto> spec)
        {
            // Utilizamos AsExpandable para permitir la expansión de expresiones complejas
            return await _context.Productos
                                 .AsNoTracking()
                                 .AsExpandable()
                                 .Where(spec.ToExpression())
                                 .ToListAsync();
        }

        /// <summary>
        /// Método de ejemplo que combina especificaciones para filtrar productos.
        /// </summary>
        /// <returns>Lista de productos filtrados.</returns>
        public async Task<List<Producto>> ObtenerProductosFiltradosAsync()
        {
            // Definimos las especificaciones individuales
            var categoriaSpec = new CategoriaSpecification("Electrónica");
            var precioSpec = new PrecioRangeSpecification(500m, null);
            var enStockSpec = new EnStockSpecification(true);

            // Combinamos las especificaciones usando operadores lógicos
            var combinadaSpec = categoriaSpec.And(precioSpec).And(enStockSpec);

            // Ejecutamos la consulta con la especificación combinada
            return await BuscarProductosAsync(combinadaSpec);
        }
    }
}
