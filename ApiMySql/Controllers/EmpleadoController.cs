using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly EmpleadoService _service;

        public EmpleadosController(EmpleadoService service)
        {
            _service = service;
        }

        [HttpPost("CrearEmpleado")]
        public async Task<IActionResult> CrearEmpleado(Empleado empleado)
        {
            try
            {
                var nuevo = await _service.CrearEmpleadoAsync(empleado);
                return CreatedAtAction(nameof(ObtenerEmpleadoPorId), new { id = nuevo.Id }, nuevo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al crear empleado.", detalle = ex.Message });
            }
        }

        [HttpGet("ListarEmpleados")]
        public async Task<IActionResult> ObtenerEmpleados(
            [FromQuery] int? edadMin,
            [FromQuery] int? edadMax,
            [FromQuery] string? puesto,
            [FromQuery] string? departamento)
        {
            try
            {
                var empleados = await _service.ObtenerEmpleadosAsync(edadMin, edadMax, puesto, departamento);
                if (empleados.Count == 0)
                    return NotFound(new { mensaje = "No se encontraron empleados con los filtros aplicados." });

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al listar empleados.", detalle = ex.Message });
            }
        }

        [HttpGet("ListarMayoresDe30")]
        public async Task<IActionResult> ObtenerMayoresDe30()
        {
            try
            {
                var empleados = await _service.ObtenerMayoresDe30Async();
                if (empleados.Count == 0)
                    return NotFound(new { mensaje = "No se encontraron empleados mayores de 30 años." });

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al listar empleados mayores de 30.", detalle = ex.Message });
            }
        }

        [HttpGet("ObtenerEmpleadoPorId/{id}")]
        public async Task<IActionResult> ObtenerEmpleadoPorId(int id)
        {
            try
            {
                var empleado = await _service.ObtenerPorIdAsync(id);
                if (empleado == null)
                    return NotFound(new { mensaje = $"Empleado con ID {id} no encontrado." });

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener empleado.", detalle = ex.Message });
            }
        }

        [HttpPut("ActualizarEmpleado/{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, Empleado datos)
        {
            try
            {
                var actualizado = await _service.ActualizarEmpleadoAsync(id, datos);
                return Ok(actualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar empleado.", detalle = ex.Message });
            }
        }

        [HttpDelete("EliminarEmpleado/{id}")]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            try
            {
                await _service.EliminarEmpleadoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al eliminar empleado.", detalle = ex.Message });
            }
        }

        [HttpGet("ObtenerEstadisticas")]
        public async Task<IActionResult> ObtenerEstadisticas()
        {
            try
            {
                var stats = await _service.ObtenerEstadisticasAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener estadísticas.", detalle = ex.Message });
            }
        }
    }
}
