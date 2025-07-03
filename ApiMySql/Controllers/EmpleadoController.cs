using ApiWeb.Context;
using ApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly AppDBContext _context;

    public EmpleadosController(AppDBContext context)
    {
        _context = context;
    }

    // POST api/empleados/CrearEmpleado
    [HttpPost("CrearEmpleado")]
    public async Task<IActionResult> CrearEmpleado(Empleado empleado)
    {
        try
        {
            if (empleado == null)
                return BadRequest(new { mensaje = "El cuerpo de la petición está vacío." });

            if (string.IsNullOrWhiteSpace(empleado.Nombre) || empleado.Nombre.Length < 3)
                return BadRequest(new { mensaje = "El nombre debe tener al menos 3 caracteres." });

            if (empleado.Edad <= 0)
                return BadRequest(new { mensaje = "La edad debe ser un número positivo." });

            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerEmpleadoPorId), new { id = empleado.Id }, empleado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al crear el empleado.", detalle = ex.Message });
        }
    }

    // GET api/empleados/ObtenerEmpleadoPorId/{id}
    [HttpGet("ObtenerEmpleadoPorId/{id}")]
    public async Task<IActionResult> ObtenerEmpleadoPorId(int id)
    {
        try
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
                return NotFound(new { mensaje = $"Empleado con ID {id} no encontrado." });

            return Ok(empleado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al obtener el empleado.", detalle = ex.Message });
        }
    }

    // GET api/empleados/ListarEmpleados
    [HttpGet("ListarEmpleados")]
    public async Task<IActionResult> ObtenerEmpleados(
        [FromQuery] int? edadMin,
        [FromQuery] int? edadMax,
        [FromQuery] string? puesto,
        [FromQuery] string? departamento)
    {
        try
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

            var empleados = await query.ToListAsync();

            if (empleados.Count == 0)
                return NotFound(new { mensaje = "No se encontraron empleados con los filtros aplicados." });

            return Ok(empleados);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al listar empleados.", detalle = ex.Message });
        }
    }

    // GET api/empleados/ListarMayoresDe30
    [HttpGet("ListarMayoresDe30")]
    public async Task<IActionResult> ObtenerMayoresDe30()
    {
        try
        {
            var empleados = await _context.Empleado
                .Where(e => e.Edad > 30)
                .ToListAsync();

            if (empleados.Count == 0)
                return NotFound(new { mensaje = "No se encontraron empleados mayores de 30 años." });

            return Ok(empleados);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al listar empleados mayores de 30 años.", detalle = ex.Message });
        }
    }

    // PUT api/empleados/ActualizarEmpleado/{id}
    [HttpPut("ActualizarEmpleado/{id}")]
    public async Task<IActionResult> ActualizarEmpleado(int id, Empleado datos)
    {
        try
        {
            if (datos == null)
                return BadRequest(new { mensaje = "El cuerpo de la petición está vacío." });

            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
                return NotFound(new { mensaje = $"Empleado con ID {id} no encontrado." });

            if (string.IsNullOrWhiteSpace(datos.Nombre) || datos.Nombre.Length < 3)
                return BadRequest(new { mensaje = "El nombre debe tener al menos 3 caracteres." });

            if (datos.Edad <= 0)
                return BadRequest(new { mensaje = "La edad debe ser un número positivo." });

            empleado.Nombre = datos.Nombre;
            empleado.Edad = datos.Edad;
            empleado.Puesto = datos.Puesto;
            empleado.Departamento = datos.Departamento;

            await _context.SaveChangesAsync();

            return Ok(empleado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al actualizar el empleado.", detalle = ex.Message });
        }
    }

    // DELETE api/empleados/EliminarEmpleado/{id}
    [HttpDelete("EliminarEmpleado/{id}")]
    public async Task<IActionResult> EliminarEmpleado(int id)
    {
        try
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
                return NotFound(new { mensaje = $"Empleado con ID {id} no encontrado." });

            _context.Empleado.Remove(empleado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al eliminar el empleado.", detalle = ex.Message });
        }
    }

    // GET api/empleados/ObtenerEstadisticas
    [HttpGet("ObtenerEstadisticas")]
    public async Task<IActionResult> ObtenerEstadisticas()
    {
        try
        {
            var total = await _context.Empleado.CountAsync();

            if (total == 0)
                return Ok(new
                {
                    TotalEmpleados = 0,
                    PromedioEdad = 0,
                    CantidadPorPuesto = Array.Empty<object>(),
                    CantidadPorDepartamento = Array.Empty<object>()
                });

            var promedioEdad = await _context.Empleado.AverageAsync(e => e.Edad);

            var cantidadPorPuesto = await _context.Empleado
                .GroupBy(e => e.Puesto)
                .Select(g => new { Puesto = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var cantidadPorDepartamento = await _context.Empleado
                .GroupBy(e => e.Departamento)
                .Select(g => new { Departamento = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var estadisticas = new
            {
                TotalEmpleados = total,
                PromedioEdad = Math.Round(promedioEdad, 2),
                CantidadPorPuesto = cantidadPorPuesto,
                CantidadPorDepartamento = cantidadPorDepartamento
            };

            return Ok(estadisticas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno al obtener estadísticas.", detalle = ex.Message });
        }
    }
}
