using Microsoft.Extensions.Configuration;
using ModernHttpClient;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web_Billycock.Repositories.Utils;

namespace Web_Billycock.Data
{
    public class DBSqlRepository : IDBSqlRepository
    {

        // Variables.
        private SqlConnection Sql_Connection;
        private SqlCommand Sql_Command;
        private SqlTransaction Sql_Transaction;
        public SqlDataReader Sql_DataReader;
        private readonly IConfiguration _Config;

        private struct stConnDB
        {
            public string ErrorDesc;
            public int ErrorNum;
        }
        private stConnDB info;

        // Indica el numero de intentos de conectar a la BD sin exito.
        public byte Sql_intentos = 0;
        // Indica el tiempo de espera de conexion a la BD.
        public int timeout = 300;

        #region "Propiedades"

        /// <summary>
        /// Devuelve la descripcion de error de la clase.
        /// </summary>
        public string ErrDesc
        {
            get { return this.info.ErrorDesc; }
        }

        /// <summary>
        /// Devuelve el numero de error de la clase.
        /// </summary>
        public string ErrNum
        {
            get { return info.ErrorNum.ToString(); }
        }

        #endregion


        /// <summary>
        /// Constructor.
        /// </summary>
        public DBSqlRepository(IConfiguration Config)
        {
            _Config = Config;
            // Creamos la cadena de conexión de la base de datos.

            // Instanciamos objeto conecction.
            Sql_Connection = new SqlConnection();
        }

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<string> GetConnectionStrings(string endpoint)
        {
            string cadenaConexion=string.Empty;
            string urlDB = _Config["urlDB"];
            HttpClient client = new HttpClient(new NativeMessageHandler());
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage res = new HttpResponseMessage();
            CancellationToken cancellationToken = CancellationToken.None;
            var policyRetry = Policy.Handle<HttpRequestException>().Retry(6);
            var policyTimeout = Policy.Timeout(60);
            var policyWrap = Policy.Wrap(policyRetry, policyTimeout);

            try
            {
                res = await policyWrap.Execute(async ct => await client.GetAsync(urlDB + endpoint), cancellationToken);

                res.EnsureSuccessStatusCode();

                if(res.StatusCode == HttpStatusCode.OK)
                {
                    BD bD = JsonConvert.DeserializeObject<BD>(res.Content.ReadAsStringAsync().Result);
                    cadenaConexion = bD.Server + bD.Database + bD.UserId + bD.Password + bD.Others;
                }
            }
            catch (Exception ex)
            {
                res.ReasonPhrase = ex.Message;
                Console.WriteLine("Error " + res + " Error " +
                ex.ToString());
            }

            Console.WriteLine("Response: {0}", "");
            return cadenaConexion;
        }

        /// <summary>
        /// Dispose de la clase.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Liberamos objetos manejados.
            }

            try
            {
                // Liberamos los obtetos no manejados.
                if (Sql_DataReader != null)
                {
                    Sql_DataReader.Close();
                    Sql_DataReader.Dispose();
                }

                // Cerramos la conexión a DB.
                if (!Desconectar())
                {
                    // Grabamos Log de Error...
                }

            }
            catch (Exception ex)
            {
                // Asignamos error.
                AsignarError(ref ex);
            }

        }


        /// <summary>
        /// Destructor.
        /// </summary>
        ~DBSqlRepository()
        {
            Dispose(false);
        }


        /// <summary>
        /// Se conecta a una base de datos de Sql.
        /// </summary>
        /// <returns>True si se conecta bien.</returns>
        private async Task<bool> Conectar(string endpoint)
        {

            bool ok = false;

            try
            {
                if (Sql_Connection != null)
                {
                    // Fijamos la cadena de conexión de la base de datos.
                    Sql_Connection.ConnectionString = await GetConnectionStrings(endpoint);
                    _ = Sql_Connection.OpenAsync();
                    ok = true;
                }
            }
            catch (Exception ex)
            {
                // Desconectamos y liberamos memoria.
                //Desconectar();
                // Asignamos error.
                AsignarError(ref ex);
                // Asignamos error de función
                ok = false;
            }

            return ok;

        }


        /// <summary>
        /// Cierra la conexión de BBDD.
        /// </summary>
        public bool Desconectar()
        {
            try
            {
                // Cerramos la conexion
                if (Sql_Connection != null)
                {
                    if (Sql_Connection.State != ConnectionState.Closed)
                    {
                        Sql_Connection.Close();
                    }
                }
                // Liberamos su memoria.
                Sql_Connection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                AsignarError(ref ex);
                return false;
            }
        }


        /// <summary>
        /// Ejecuta un procedimiento almacenado de Sql.
        /// </summary>
        /// <param name="SqlCommand">Objeto Command con los datos del procedimiento.</param>
        /// <param name="SpName">Nombre del procedimiento almacenado.</param>
        /// <returns>True si el procedimiento se ejecuto bien.</returns>
        public async Task<bool> EjecutaSP(string endpoint, string SpName, bool conresultado)
        {
            bool ok = true;

            try
            {
                // Si no esta conectado, se conecta.
                if (!IsConected())
                {
                    ok = await Conectar(endpoint);
                }

                if (ok)
                {
                    Sql_Transaction = Sql_Connection.BeginTransaction();
                    Sql_Command.Connection = Sql_Connection;
                    Sql_Command.CommandText = SpName;
                    Sql_Command.CommandType = CommandType.StoredProcedure;
                    Sql_Command.CommandTimeout = timeout;
                    if (conresultado) Sql_DataReader = Sql_Command.ExecuteReader();
                    else Sql_Command.ExecuteNonQuery();
                    Sql_Transaction.Commit();
                }
                else
                {
                    ok = false;
                }
            }
            catch (Exception ex)
            {
                Sql_Transaction.Rollback();
                AsignarError(ref ex);
                //ok = false;
            }

            return ok;
        }



        /// <summary>
        /// Ejecuta una sql que rellenar un DataReader (sentencia select).
        /// </summary>
        /// <param name="SqlQuery">sentencia sql a ejecutar</param>
        /// <returns></returns> 
        public async Task<bool> EjecutaSQL(string endpoint,string SqlQuery)
        {

            bool ok = true;

            SqlCommand Sql_Command = new SqlCommand();

            try
            {

                // Si no esta conectado, se conecta.
                if (!IsConected())
                {
                    ok = await Conectar(endpoint);
                }

                if (ok)
                {
                    // Cerramos cursores abiertos, para evitar el error ORA-1000
                    if ((Sql_DataReader != null))
                    {
                        Sql_DataReader.Close();
                        Sql_DataReader.Dispose();
                    }

                    Sql_Transaction = Sql_Connection.BeginTransaction();
                    Sql_Command.Connection = Sql_Connection;
                    Sql_Command.CommandType = CommandType.Text;
                    Sql_Command.CommandText = SqlQuery;
                    Sql_Command.CommandTimeout = timeout;

                    // Ejecutamos sql.
                    Sql_DataReader = Sql_Command.ExecuteReader();
                    Sql_Transaction.Commit();
                }
                else
                {
                    ok = false;
                }
            }
            catch (Exception ex)
            {
                AsignarError(ref ex);
                Sql_Transaction.Rollback();
                ok = false;
            }
            finally
            {
                if (Sql_Command != null)
                {
                    Sql_Command.Dispose();
                }
            }

            return ok;

        }


        /// <summary>
        /// Captura Excepciones
        /// </summary>
        /// <param name="ex">Excepcion producida.</param>
        private void AsignarError(ref Exception ex)
        {
            // Si es una excepcion de Sql.
            if (ex is SqlException)
            {
                info.ErrorNum = ((SqlException)ex).Number;
                info.ErrorDesc = ex.Message;
            }
            else
            {
                info.ErrorNum = 0;
                info.ErrorDesc = ex.Message;
            }
            // Grabamos Log de Error...
        }



        /// <summary>
        /// Devuelve el estado de la base de datos
        /// </summary>
        /// <returns>True si esta conectada.</returns>
        public bool IsConected()
        {

            bool ok = false;

            try
            {
                // Si el objeto conexion ha sido instanciado
                if (Sql_Connection != null)
                {
                    // Segun el estado de la Base de Datos.
                    switch (Sql_Connection.State)
                    {
                        case ConnectionState.Closed:
                        case ConnectionState.Broken:
                        case ConnectionState.Connecting:
                            ok = false;
                            break;
                        case ConnectionState.Open:
                        case ConnectionState.Fetching:
                        case ConnectionState.Executing:
                            ok = true;
                            break;
                    }
                }
                else
                {
                    ok = false;
                }

            }
            catch (Exception ex)
            {
                AsignarError(ref ex);
                ok = false;
            }

            return ok;

        }

    }
}
