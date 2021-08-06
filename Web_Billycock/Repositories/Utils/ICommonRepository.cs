using Billycock.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Utils
{
    public interface ICommonRepository<T> where T : class
    {
        public Task<string> DeleteLogicoObjeto(T t, BillycockServiceContext context);
        public Task<string> DeleteObjeto(T t, BillycockServiceContext context);
        public Task<string> InsertObjeto(T t, BillycockServiceContext context);
        public Task<string> UpdateObjeto(T t, BillycockServiceContext context);
        public Task<string> ExceptionMessage(T t, string MessageType);
        public string ObtenerFechaFacturacionUsuario();
        public int reprocesoUsuario(int cuenta, double monto);
        public string SetearFecha(DateTime fecha);
        public string SetearFechaTiempo();
    }
}
