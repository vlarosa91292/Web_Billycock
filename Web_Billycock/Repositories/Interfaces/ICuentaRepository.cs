using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Billycock.DTO;
using Billycock.Models;

namespace Billycock.Repositories.Interfaces
{
    public interface ICuentaRepository
    {
        Task<List<CuentaDTO>> GetCuentas();
        Task<CuentaDTO> GetCuentabyId(int? id);
        Task<CuentaDTO> GetCuentabyName(string Name);
        //Operaciones Transaccionales
        Task<string> InsertCuenta(CuentaDTO cuenta);
        Task<string> UpdateCuenta(CuentaDTO cuenta);
        Task<string> DeleteCuenta(CuentaDTO cuenta);
        Task<bool> CuentaExists(int id);
    }
}
