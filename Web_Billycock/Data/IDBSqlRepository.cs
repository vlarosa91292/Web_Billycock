using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Billycock.Data
{
    public interface IDBSqlRepository
    {
        Task<bool> EjecutaSP(string endpoint, string SpName, bool conresultado);
        Task<bool> EjecutaSQL(string endpoint, string SqlQuery);
        Task<string> GetConnectionStrings(string endpoint);
    }
}
