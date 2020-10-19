using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;

namespace APIUsers.Library.Helpers.Datos
{
    public class SqlConexion : IDisposable
    {
        private SqlConnection _conn = null;
        string _ConnectionString = "";
        bool _Conectado = false;

        string _NombreProcedimiento = "";
        List<SqlParameter> _Parametros = new List<SqlParameter>();
        bool _Preparado = false;

        SqlConexion()
        {

        }

        public SqlConexion AutoDesconexion { get; set; }

        public static SqlConexion Conectar(string ConnectionString)
        {
            SqlConexion sql = new SqlConexion()
            {
                _conn = new SqlConnection(ConnectionString)
            };

            try
            {
                sql._conn.Open();
                sql._Conectado = true;
                //_Rsp = true;
            }
            catch (Exception ex)
            {
                //_Rsp = false;
            }
            return sql;
        }

        public void Desconectar()
        {
            try
            {
                _conn.Close();
            }
            catch { }
        }

        public void PrepararProcedimiento(string NombreProcedimiento, List<SqlParameter> Parametros)
        {
            if (_Conectado)
            {
                _NombreProcedimiento = "";
                _Parametros.Clear();

                _NombreProcedimiento = NombreProcedimiento;
                _Parametros = Parametros;
                _Preparado = true;
            }
            else
            {
                throw new Exception("No hay conexion con la bd");
            }
        }

        public int EjecutarProcedimiento()
        {
            if (_Preparado)
            {
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                _Preparado = false;
                return cmm.ExecuteNonQuery();

            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }

        public DataTableReader EjecutarTableReader()
        {
            if (_Preparado)
            {
                DataTable dt = new DataTable();
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 120000;
                //cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                SqlDataAdapter adt = new SqlDataAdapter(cmm);
                adt.Fill(dt);
                _Preparado = false;
                return dt.CreateDataReader();
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }

        public DataTableReader EjecutarTableReader(System.Data.CommandType TipoComando)
        {
            if (_Preparado)
            {
                DataTable dt = new DataTable();
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = TipoComando;
                cmm.CommandTimeout = 120000; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                SqlDataAdapter adt = new SqlDataAdapter(cmm);
                adt.Fill(dt);
                _Preparado = false;
                return dt.CreateDataReader();
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }


        public XmlTextReader EjecutarXmlReader()
        {
            object valor = null;
            if (_Preparado)
            {
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 10000; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                _Preparado = false;
                cmm.ExecuteScalar();

                foreach (SqlParameter sp in cmm.Parameters)
                {
                    if (sp.Direction == ParameterDirection.ReturnValue || sp.Direction == ParameterDirection.Output)
                    {
                        valor = sp.Value;
                        break;
                    }
                }

                if (valor != null)
                {

                    XmlParserContext context =
    new XmlParserContext(null, null, string.Empty, XmlSpace.None, Encoding.UTF8);
                    XmlTextReader xmr = new XmlTextReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes(valor.ToString())), XmlNodeType.Document, context);
                    return xmr;
                }
                else
                {
                    _Preparado = false;
                    throw new Exception("No es un xml valido");
                }
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }


        public object EjecutarProcedimientoOutput()
        {
            object valor = null;
            if (_Preparado)
            {
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                //cmm.CommandTimeout = 1000; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                _Preparado = false;
                cmm.ExecuteScalar();

                foreach (SqlParameter sp in cmm.Parameters)
                {
                    if (sp.Direction == ParameterDirection.ReturnValue || sp.Direction == ParameterDirection.Output)
                    {
                        valor = sp.Value;
                        break;
                    }
                }
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
            return valor;
        }
        public object EjecutarProcedimientoOutputODP()
        {
            object valor = null;
            if (_Preparado)
            {
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 1000000; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                _Preparado = false;
                cmm.ExecuteScalar();

                foreach (SqlParameter sp in cmm.Parameters)
                {
                    if (sp.Direction == ParameterDirection.ReturnValue || sp.Direction == ParameterDirection.Output)
                    {
                        valor = sp.Value;
                        break;
                    }
                }
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
            return valor;
        }
        public object[] EjecutarProcedimientoMultiOutput()
        {
            object[] valor = new object[0];
            if (_Preparado)
            {
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                _Preparado = false;
                cmm.ExecuteScalar();
                int Contador = 0;
                foreach (SqlParameter sp in cmm.Parameters)
                {
                    if (sp.Direction == ParameterDirection.ReturnValue || sp.Direction == ParameterDirection.Output)
                    {
                        Contador++;
                        Array.Resize(ref valor, Contador);
                        valor[Contador - 1] = sp.Value;
                    }
                }
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
            return valor;
        }
        public object EjecutarScalar()
        {
            if (_Preparado)
            {
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                _Preparado = false;
                return cmm.ExecuteScalar();
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }

        public DataTable EjecutarTable()
        {
            if (_Preparado)
            {
                DataTable dt = new DataTable();
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                SqlDataAdapter adt = new SqlDataAdapter(cmm);
                adt.Fill(dt);
                _Preparado = false;
                return dt.Copy();
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }

        public DataTable EjecutarTableGenerico()
        {
            if (_Preparado)
            {
                DataTable dt = new DataTable();
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                //cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                dt.Load(cmm.ExecuteReader());
                //SqlDataAdapter adt = new SqlDataAdapter(cmm);
                //adt.Fill(dt);
                _Preparado = false;
                return dt.Copy();
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }

        public DataTable EjecutarTableReportes()
        {
            if (_Preparado)
            {
                DataTable dt = new DataTable();
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 1000000; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                SqlDataAdapter adt = new SqlDataAdapter(cmm);
                adt.Fill(dt);
                _Preparado = false;
                return dt.Copy();
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }

        //************************Sobrecargué este método para que reciba el tipo de comando********************
        public DataTable EjecutarTable(System.Data.CommandType tipoComando)
        {
            if (_Preparado)
            {
                DataTable dt = new DataTable();
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = tipoComando;
                cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                SqlDataAdapter adt = new SqlDataAdapter(cmm);
                adt.Fill(dt);
                _Preparado = false;
                return dt.Copy();
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }

        public object[] EjecutarObject()
        {
            if (_Preparado)
            {
                object[] Resp = new object[0];
                SqlCommand cmm = new SqlCommand(_NombreProcedimiento, _conn);
                cmm.CommandType = System.Data.CommandType.StoredProcedure;
                cmm.CommandTimeout = 120; //SEGUNDOS DE ESPERA PARA EJECUTAR UNA CONSULTA EN SQL(60 SEGUNDOS)
                cmm.Parameters.AddRange(_Parametros.ToArray());
                _Preparado = false;
                SqlDataReader dtr = cmm.ExecuteReader();
                dtr.Read(); // Solo vamos a regresar la primera
                dtr.GetValues(Resp);
                return Resp;
            }
            else
            {
                _Preparado = false;
                throw new Exception("Procedimiento no preparado");
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                _conn.Dispose();
                _Parametros.Clear();
                _Preparado = false;
            }
            catch { }
        }

        #endregion
    }
}