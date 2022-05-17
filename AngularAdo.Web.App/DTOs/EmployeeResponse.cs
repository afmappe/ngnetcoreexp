using AngularAdo.Web.App.Employee;
using System.ComponentModel.DataAnnotations;

namespace AngularAdo.Web.App.DTOs
{
    public class EmployeeResponse : EmployeeDto
    {
        [Required]
        public int Id { get; set; }

        public static EmployeeResponse Convert(EmpleadoEntity entity)
        {
            EmployeeResponse result = null;
            if (entity != null)
            {
                result = new EmployeeResponse()
                {
                    Apellidos = entity.ApellidosEmpleado,
                    Direccion = entity.DireccionEmpleado,
                    Email = entity.EmailEmpleado,
                    FechaNacimiento = entity.FechaNacimientoEmpleado,
                    Id = entity.CodigoEmpleado,
                    Nombres = entity.NombresEmpleado,
                    Sueldo = entity.SueldoEmpleado,
                    Telefono = entity.TelefonoEmpleado
                };
            }
            return result;
        }
    }
}