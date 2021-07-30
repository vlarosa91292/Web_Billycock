using Billycock.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Utils
{
    public interface ICommonRepository<T> where T : class
    {
        public Task<string> DeleteLogicoObjeto(T traker,T t, BillycockServiceContext context);
        public Task<string> DeleteObjeto(T traker,T t, BillycockServiceContext context);
        public Task<string> InsertObjeto(T traker,T t, BillycockServiceContext context);
        public Task<string> UpdateObjeto(T traker,T t, BillycockServiceContext context);
        public string ExceptionMessage(T t, string MessageType);
    }
}
