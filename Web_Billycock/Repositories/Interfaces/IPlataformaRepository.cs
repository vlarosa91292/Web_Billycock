using Billycock.DTO;
using Billycock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Repositories.Interfaces
{
    public interface IPlataformaRepository
    {
        Task<List<PlataformaDTO>> GetPlataformas();
        Task<PlataformaDTO> GetPlataformabyId(int? id);
        Task<PlataformaDTO> GetPlataformabyName(string name);
        //Operaciones Transaccionales
        Task<string> InsertPlataforma(PlataformaDTO usuario);
        Task<string> UpdatePlataforma(PlataformaDTO usuario);
        Task<string> DeletePlataforma(PlataformaDTO usuario);
        Task<bool> PlataformaExists(int id);
        Task<double> GetPricePlataforma(int id);
    }
}
