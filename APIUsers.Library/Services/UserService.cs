using APIUsers.Library.Helpers.Datos;
using APIUsers.Library.Interfaces;
using APIUsers.Library.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace APIUsers.Library.Services
{
    public class UserService: IUser, IDisposable
    {
        #region Constructor y Variables
        SqlConexion sql = null;
        MySqlConexion mysql = null;
        ConnectionType type = ConnectionType.NONE;

        UserService()
        {

        }

        public static UserService CrearInstanciaSQL(SqlConexion sql)
        {
            UserService log = new UserService
            {
                sql = sql,
                type = ConnectionType.MSSQL
            };

            return log;
        }
        public static UserService CrearInstanciaMySQL(MySqlConexion mysql)
        {
            UserService log = new UserService
            {
                mysql = mysql,
                type = ConnectionType.MYSQL
            };

            return log;
        }
        #endregion

        public List<User> GetUsers()
        {
            List<User> list = new List<User>();
            User user = new User();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                sql.PrepararProcedimiento("dbo.[USER.GetAllJSON]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Usuario"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new User()
                                {
                                    ID = Convert.ToInt32(jsonOperaciones["Id"].ToString()),
                                    Name = jsonOperaciones["Name"].ToString(),
                                    CreateDate = DateTime.Parse(jsonOperaciones["CreateDate"].ToString())
                                });

                            }

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return list;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (sql != null)
                    {
                        sql.Desconectar();
                        sql.Dispose();
                    }// TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~HidraService()
        // {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
