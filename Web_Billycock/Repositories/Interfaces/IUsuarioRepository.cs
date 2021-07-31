using Billycock.DTO;
using Billycock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioDTO>> GetUsuarios(string tipoSalida);
        Task<UsuarioDTO> GetUsuariobyId(int? id, string tipoSalida);
        Task<UsuarioDTO> GetUsuariobyName(string name, string tipoSalida);
        //Operaciones Transaccionales
        Task<string> InsertUsuario(UsuarioDTO usuario);    
        Task<string> UpdateUsuario(UsuarioDTO usuario, string tipoSalida);
        Task<string> DeleteUsuario(UsuarioDTO usuario, string tipoSalida);
        Task<bool> UsuarioExists(int id);
    }
}
