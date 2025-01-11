using SpecificationPlayground;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecificationPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            // Crear una lista de productos
            var productos = new List<Producto>
            {
                new Producto { Nombre = "Laptop", Precio = 1500, Categoria = "Electrónica" },
                new Producto { Nombre = "Silla", Precio = 100, Categoria = "Muebles" },
                new Producto { Nombre = "Smartphone", Precio = 800, Categoria = "Electrónica" },
                new Producto { Nombre = "Mesa", Precio = 200, Categoria = "Muebles" }
            };

            // Definir especificaciones
            var especificacionPrecioMinimo = new PrecioMinimoSpecification(500);
            var especificacionCategoria = new CategoriaSpecification("Electrónica");

            // Combinar especificaciones usando AND
            var especificacionCombinada = especificacionPrecioMinimo.And(especificacionCategoria);

            // Filtrar productos que cumplen con la especificación combinada
            var productosFiltrados = productos.Where(p => especificacionCombinada.IsSatisfiedBy(p));

            // Mostrar resultados
            Console.WriteLine("Productos que cumplen con el precio mínimo y la categoría 'Electrónica':");
            foreach (var producto in productosFiltrados)
            {
                Console.WriteLine($"- {producto.Nombre} (${producto.Precio})");
            }

            // Esperar entrada para cerrar
            Console.ReadLine();
        
        }
    }
}
