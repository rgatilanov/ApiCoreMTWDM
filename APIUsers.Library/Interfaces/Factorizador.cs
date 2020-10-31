using System;
using System.Collections.Generic;
using System.Text;

namespace APIUsers.Library.Interfaces
{
    using Helpers.Datos;
    using Services;
    public static class Factorizador
    {
        public static IUser CrearConexionServicio(Models.ConnectionType type, string connectionString)
        {
            IUser nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = UserService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }

        public static ILogin CrearConexionServicioLogin(Models.ConnectionType type, string connectionString)
        {
            ILogin nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = LoginService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }
    }
}
