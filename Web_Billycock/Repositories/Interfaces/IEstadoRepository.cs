using Billycock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Billycock.Repositories.Interfaces
{
    public interface IEstadoRepository
    {
        Task<List<Estado>> GetEstados();
        Task<Estado> GetEstadobyId(int? id);
        Task<Estado> GetEstadobyName(string Name);
        //Operaciones Transaccionales
        Task<string> InsertEstado(Estado estado);
        Task<string> UpdateEstado(Estado estado);
        Task<string> DeleteEstado(Estado estado);
        Task<bool> EstadoExists(int id);
    }
}
