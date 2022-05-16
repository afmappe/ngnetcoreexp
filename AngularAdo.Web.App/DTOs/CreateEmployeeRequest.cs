using System.ComponentModel.DataAnnotations;

namespace AngularAdo.Web.App.DTOs
{
    public class CreateEmployeeRequest
    {
        /// <summary>
        /// Appellidos
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Apellidos { get; set; }

        /// <summary>
        /// Direccion de residencia
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Direccion { get; set; }

        /// <summary>
        /// Direccion de correo electronico
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Fecha de Nacimiento
        /// </summary>
        [Required]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Nombres
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nombres { get; set; }

        /// <summary>
        /// Salario Mensual
        /// </summary>
        [Required]
        public double Sueldo { get; set; }

        /// <summary>
        /// Numero de contacto
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Telefono { get; set; }
    }
}