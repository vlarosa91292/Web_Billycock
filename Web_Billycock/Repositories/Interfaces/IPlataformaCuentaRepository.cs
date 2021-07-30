using Billycock.DTO;
using Billycock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Repositories.Interfaces
{
    public interface IPlataformaCuentaRepository
    {
        Task<List<PlataformaCuentaDTO>> GetPlataformaCuentas();
        Task<PlataformaCuentaDTO> GetPlataformaCuentabyIdPlataforma(string id);
        Task<PlataformaCuentaDTO> GetPlataformaCuentabyIdCuenta(string id);
        Task<PlataformaCuentaDTO> GetPlataformaCuentabyIds(string id);
        //Operaciones Transaccionales
        Task<string> InsertPlataformaCuenta(PlataformaCuentaDTO plataformaCuenta);
        Task<string> UpdatePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta);
        Task<string> DeletePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta);
        Task<bool> PlataformaCuentaExists(string idPlataformaCuenta);
        public Task<PlataformaCuentaDTO> GetPlataformaCuentaDisponible(int idPlataforma, int? cantidad);
    }
}
