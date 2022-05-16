using AngularAdo.Web.App.DTOs;
using AngularAdo.Web.App.Employee;
using Microsoft.AspNetCore.Mvc;

namespace AngularAdo.Web.App.Controllers
{
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
        /// Crear nuevos empleados en la aplicacion
        /// </summary>
        /// <param name="request">Parametros para crear el usuario</param>
        /// <returns>Indica si se pudo crear el usuario</returns>
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

        [HttpGet]
        public ActionResult<IEnumerable<EmpleadoEntity>> Get()
        {
            var list = _empleadoAccessData.ListAll();
            return new OkObjectResult(list);
        }
    }
}