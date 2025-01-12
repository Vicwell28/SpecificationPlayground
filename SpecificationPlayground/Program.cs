using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SpecificationPlayground
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configuración de los servicios y la cadena de conexión
            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(""))
                .AddScoped<ProductoService>()
                .BuildServiceProvider();

            // Obtener una instancia del servicio de producto
            var productoService = serviceProvider.GetRequiredService<ProductoService>();

            // Ejecutar el caso de uso: Buscar productos de "Electrónica", precio > 500 y en stock
            var productosFiltrados = await productoService.ObtenerProductosFiltradosAsync();

            // Mostrar los resultados
            Console.WriteLine("Productos Filtrados:");
            foreach (var producto in productosFiltrados)
            {
                Console.WriteLine($"ID: {producto.Id}, Nombre: {producto.Nombre}, Categoría: {producto.Categoria}, Precio: {producto.Precio}, En Stock: {producto.EnStock}");
            }
        }
    }
}
