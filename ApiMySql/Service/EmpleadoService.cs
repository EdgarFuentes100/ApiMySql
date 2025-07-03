using ApiWeb.Context;
using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Services
{
    public class EmpleadoService
    {
        private readonly AppDBContext _context;

        public EmpleadoService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Empleado> CrearEmpleadoAsync(Empleado empleado)
        {
            if (string.IsNullOrWhiteSpace(empleado.Nombre) || empleado.Nombre.Length < 3)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");

            if (empleado.Edad <= 0)
                throw new ArgumentException("La edad debe ser un número positivo.");

            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();

            return empleado;
        }

        public async Task<List<Empleado>> ObtenerEmpleadosAsync(int? edadMin, int? edadMax, string? puesto, string? departamento)
        {
            var query = _context.Empleado.AsQueryable();

            if (edadMin.HasValue)
                query = query.Where(e => e.Edad >= edadMin.Value);

            if (edadMax.HasValue)
                query = query.Where(e => e.Edad <= edadMax.Value);

            if (!string.IsNullOrWhiteSpace(puesto))
                query = query.Where(e => e.Puesto.ToLower() == puesto.ToLower());

            if (!string.IsNullOrWhiteSpace(departamento))
                query = query.Where(e => e.Departamento.ToLower() == departamento.ToLower());

            return await query.ToListAsync();
        }

        public async Task<List<Empleado>> ObtenerMayoresDe30Async()
        {
            return await _context.Empleado
                .Where(e => e.Edad > 30)
                .ToListAsync();
        }

        public async Task<Empleado?> ObtenerPorIdAsync(int id)
        {
            return await _context.Empleado.FindAsync(id);
        }

        public async Task<Empleado> ActualizarEmpleadoAsync(int id, Empleado datos)
        {
            var empleado = await _context.Empleado.FindAsync(id);

            if (empleado == null)
                throw new KeyNotFoundException($"Empleado con ID {id} no encontrado.");

            if (string.IsNullOrWhiteSpace(datos.Nombre) || datos.Nombre.Length < 3)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");

            if (datos.Edad <= 0)
                throw new ArgumentException("La edad debe ser un número positivo.");

            empleado.Nombre = datos.Nombre;
            empleado.Edad = datos.Edad;
            empleado.Puesto = datos.Puesto;
            empleado.Departamento = datos.Departamento;

            await _context.SaveChangesAsync();

            return empleado;
        }

        public async Task EliminarEmpleadoAsync(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
                throw new KeyNotFoundException($"Empleado con ID {id} no encontrado.");

            _context.Empleado.Remove(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task<object> ObtenerEstadisticasAsync()
        {
            var total = await _context.Empleado.CountAsync();

            if (total == 0)
            {
                return new
                {
                    TotalEmpleados = 0,
                    PromedioEdad = 0,
                    CantidadPorPuesto = Array.Empty<object>(),
                    CantidadPorDepartamento = Array.Empty<object>()
                };
            }

            var promedioEdad = await _context.Empleado.AverageAsync(e => e.Edad);

            var cantidadPorPuesto = await _context.Empleado
                .GroupBy(e => e.Puesto)
                .Select(g => new { Puesto = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var cantidadPorDepartamento = await _context.Empleado
                .GroupBy(e => e.Departamento)
                .Select(g => new { Departamento = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            return new
            {
                TotalEmpleados = total,
                PromedioEdad = Math.Round(promedioEdad, 2),
                CantidadPorPuesto = cantidadPorPuesto,
                CantidadPorDepartamento = cantidadPorDepartamento
            };
        }
    }
}
