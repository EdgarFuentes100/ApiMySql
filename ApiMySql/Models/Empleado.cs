using System.ComponentModel.DataAnnotations;

namespace ApiWeb.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        public string Nombre { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La edad debe ser un número positivo.")]
        public int Edad { get; set; }

        [Required]
        public string Puesto { get; set; }

        [Required]
        public string Departamento { get; set; }
    }
}
