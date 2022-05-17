using System.Data;
using System.Data.SqlClient;

namespace AngularAdo.Web.App.Employee
{
    public class EmpleadoAccessData
    {
        private readonly IConfiguration _configuration;
        private readonly string _ConnectionString;

        public EmpleadoAccessData(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnectionString = _configuration.GetValue<string>(SettingKeys.Database_ConnectionString);
        }

        public string Create(EmpleadoEntity entity)
        {
            string result = null;

            if (entity != null)
            {
                using (var conn = this.OpenConnection())
                {
                    using SqlCommand command = new SqlCommand("usp_Empleado_Registrar", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.Add(new SqlParameter("@Nombres_Empleado", SqlDbType.VarChar, 100)).Value = entity.NombresEmpleado;
                    command.Parameters.Add(new SqlParameter("@Apellidos_Empleado", SqlDbType.VarChar, 100)).Value = entity.ApellidosEmpleado;
                    command.Parameters.Add(new SqlParameter("@Direccion_Empleado", SqlDbType.VarChar, 200)).Value = entity.DireccionEmpleado;
                    command.Parameters.Add(new SqlParameter("@Telefono_Empleado", SqlDbType.VarChar, 200)).Value = entity.TelefonoEmpleado;
                    command.Parameters.Add(new SqlParameter("@Email_Empleado", SqlDbType.VarChar, 200)).Value = entity.EmailEmpleado;
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento_Empleado", SqlDbType.DateTime)).Value = entity.FechaNacimientoEmpleado;
                    command.Parameters.Add(new SqlParameter("@Sueldo_Empleado", SqlDbType.Real)).Value = entity.SueldoEmpleado;
                    command.Parameters.Add(new SqlParameter("@Activo", SqlDbType.VarChar, 100)).Value = entity.Activo;

                    command.ExecuteNonQuery();

                    result = $"User {entity.EmailEmpleado} Created";
                }
            }
            return result;
        }

        public EmpleadoEntity GetById(int id)
        {
            EmpleadoEntity result = null;
            using (var conn = this.OpenConnection())
            {
                using SqlCommand command = new SqlCommand("usp_Empleado_Filtar_Id", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Codi_Empleado", SqlDbType.Int)).Value = id;
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    result= new EmpleadoEntity
                    {
                        CodigoEmpleado = Convert.ToInt32(reader["Codi_Empleado"]),
                        NombresEmpleado = reader["Nombres_Empleado"].ToString(),
                        ApellidosEmpleado = reader["Apellidos_Empleado"].ToString().Trim(),
                        DireccionEmpleado = reader["Direccion_Empleado"].ToString().Trim(),
                        TelefonoEmpleado = reader["Telefono_Empleado"].ToString().Trim(),
                        EmailEmpleado = reader["Email_Empleado"].ToString().Trim(),
                        FechaNacimientoEmpleado = Convert.ToDateTime(reader["FechaNacimiento_Empleado"]),
                        SueldoEmpleado = Convert.ToDouble(reader["Sueldo_Empleado"]),
                        Activo = Convert.ToBoolean(reader["Activo"])
                    };
                }
                return result;
            }
        }

        public List<EmpleadoEntity> ListAll()
        {
            var result = new List<EmpleadoEntity>();
            using (var conn = this.OpenConnection())
            {
                using SqlCommand command = new SqlCommand("usp_Empleado_ListarTodos", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    EmpleadoEntity item = new EmpleadoEntity
                    {
                        CodigoEmpleado = Convert.ToInt32(reader["Codi_Empleado"]),
                        NombresEmpleado = reader["Nombres_Empleado"].ToString(),
                        ApellidosEmpleado = reader["Apellidos_Empleado"].ToString().Trim(),
                        DireccionEmpleado = reader["Direccion_Empleado"].ToString().Trim(),
                        TelefonoEmpleado = reader["Telefono_Empleado"].ToString().Trim(),
                        EmailEmpleado = reader["Email_Empleado"].ToString().Trim(),
                        FechaNacimientoEmpleado = Convert.ToDateTime(reader["FechaNacimiento_Empleado"]),
                        SueldoEmpleado = Convert.ToDouble(reader["Sueldo_Empleado"]),
                        Activo = Convert.ToBoolean(reader["Activo"])
                    };

                    result.Add(item);
                }
            }
            return result;
        }

        public string Update(EmpleadoEntity entity)
        {
            string result = null;

            if (entity != null)
            {
                using (var conn = this.OpenConnection())
                {
                    using SqlCommand command = new SqlCommand("usp_Empleado_Modificar", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(new SqlParameter("@Codi_Empleado", SqlDbType.Int)).Value = entity.CodigoEmpleado;
                    command.Parameters.Add(new SqlParameter("@Nombres_Empleado", SqlDbType.VarChar, 100)).Value = entity.NombresEmpleado;
                    command.Parameters.Add(new SqlParameter("@Apellidos_Empleado", SqlDbType.VarChar, 100)).Value = entity.ApellidosEmpleado;
                    command.Parameters.Add(new SqlParameter("@Direccion_Empleado", SqlDbType.VarChar, 200)).Value = entity.DireccionEmpleado;
                    command.Parameters.Add(new SqlParameter("@Telefono_Empleado", SqlDbType.VarChar, 200)).Value = entity.TelefonoEmpleado;
                    command.Parameters.Add(new SqlParameter("@Email_Empleado", SqlDbType.VarChar, 200)).Value = entity.EmailEmpleado;
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento_Empleado", SqlDbType.DateTime)).Value = entity.FechaNacimientoEmpleado;
                    command.Parameters.Add(new SqlParameter("@Sueldo_Empleado", SqlDbType.Real)).Value = entity.SueldoEmpleado;
                    command.Parameters.Add(new SqlParameter("@Activo", SqlDbType.VarChar, 100)).Value = entity.Activo;

                    command.ExecuteNonQuery();

                    result = $"User {entity.CodigoEmpleado} Updated";
                }
            }
            return result;
        }

        private SqlConnection OpenConnection()
        {
            var conn = new SqlConnection(this._ConnectionString);
            conn.Open();
            return conn;
        }
    }
}