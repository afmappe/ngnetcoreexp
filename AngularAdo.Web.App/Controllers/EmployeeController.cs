using AngularAdo.Web.App.DTOs;
using AngularAdo.Web.App.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularAdo.Web.App.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]", Name = "EmployeeV1")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmpleadoAccessData _empleadoAccessData;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(EmpleadoAccessData empleadoAccessData, ILogger<EmployeeController> logger)
        {
            _empleadoAccessData = empleadoAccessData;
            _logger = logger;
        }

        /// <summary>
        /// Crear nuevos empleados en la aplicación
        /// </summary>
        /// <param name="request">Parámetros para crear el empleado</param>
        /// <returns>Indica si se pudo crear el empleado</returns>
        [HttpPost]
        public ActionResult<string> Create(CreateEmployeeRequest request)
        {
            var entity = new EmpleadoEntity
            {
                TelefonoEmpleado = request.Telefono,
                Activo = true,
                ApellidosEmpleado = request.Apellidos,
                DireccionEmpleado = request.Direccion,
                EmailEmpleado = request.Email,
                FechaNacimientoEmpleado = request.FechaNacimiento,
                NombresEmpleado = request.Nombres,
                SueldoEmpleado = request.Sueldo
            };

            string result = _empleadoAccessData.Create(entity);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Obtiene un empleado utilizando el Identificador único
        /// </summary>
        /// <param name="id">Identificador único</param>
        /// <returns>Información del usuario</returns>
        [HttpGet("{id}")]
        public ActionResult<EmployeeResponse> Get(string id)
        {
            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int intId))
            {
                var entity = _empleadoAccessData.GetById(intId);
                var result = EmployeeResponse.Convert(entity);
                return new OkObjectResult(result);
            }
            else
            {
                return new BadRequestObjectResult("invalid id");
            }
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<EmployeeResponse>> GetAll()
        {
            var list = _empleadoAccessData.ListAll()
                .Select(x => EmployeeResponse.Convert(x))
                .ToList();

            return new OkObjectResult(list);
        }

        /// <summary>
        /// Actualiza la información de un empleado
        /// </summary>
        /// <param name="id">Identificador único</param>
        /// <param name="request">Parámetros para crear el empleado</param>
        /// <returns>Indica si se pudo actualizar el usuario</returns>
        [HttpPut("{id}")]
        public ActionResult<string> Updated(string id, CreateEmployeeRequest request)
        {
            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int intId))
            {
                var entity = new EmpleadoEntity
                {
                    Activo = true,
                    ApellidosEmpleado = request.Apellidos,
                    CodigoEmpleado = intId,
                    DireccionEmpleado = request.Direccion,
                    EmailEmpleado = request.Email,
                    FechaNacimientoEmpleado = request.FechaNacimiento,
                    NombresEmpleado = request.Nombres,
                    SueldoEmpleado = request.Sueldo,
                    TelefonoEmpleado = request.Telefono
                };

                string result = _empleadoAccessData.Update(entity);
                return new OkObjectResult(result);
            }
            else
            {
                return new BadRequestObjectResult("invalid id");
            }
        }
    }
}