using Billycock.Data;
using Billycock.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Billycock.DTO;

namespace Web_Billycock.Repositories.Interfaces
{
    public interface IBillycock_WebRepository<T> where T : class
    {
        #region Metodos Principales
            #region Create
                Task<string> InsertUsuario(UsuarioDTO usuario);
                Task<string> InsertPlataforma(PlataformaDTO usuario);
                Task<string> InsertCuenta(CuentaDTO cuenta);
                Task<string> InsertPlataformaCuenta(PlataformaCuentaDTO plataformaCuenta);
                Task<string> InsertUsuarioPlataformaCuenta(UsuarioPlataformaCuentaDTO usuarioPlataformaCuenta);
                Task<string> InsertEstado(EstadoDTO estado);
            #endregion
            #region Read
                Task<List<UsuarioDTO>> GetUsuarios(bool complemento);
                Task<UsuarioDTO> GetUsuariobyId(int? id, bool complemento);
                Task<UsuarioDTO> GetUsuariobyName(string name, bool complemento);
                Task<bool> UsuarioExists(int id);

                Task<List<PlataformaDTO>> GetPlataformas(bool complemento);
                Task<PlataformaDTO> GetPlataformabyId(int? id, bool complemento);
                Task<PlataformaDTO> GetPlataformabyName(string name, bool complemento);
                Task<bool> PlataformaExists(int id);

                Task<List<CuentaDTO>> GetCuentas(bool complemento);
                Task<CuentaDTO> GetCuentabyId(int? id, bool complemento);
                Task<CuentaDTO> GetCuentabyName(string Name, bool complemento);
                Task<bool> CuentaExists(int id);

                Task<List<PlataformaCuentaDTO>> GetPlataformaCuentas(bool complemento);
                Task<PlataformaCuentaDTO> GetPlataformaCuentabyIds(string id, bool complemento);
                Task<List<PlataformaCuentaDTO>> GetPlataformaCuentasbyIdPlataforma(string id, bool complemento);
                Task<List<PlataformaCuentaDTO>> GetPlataformaCuentasbyIdCuenta(string id, bool complemento);
                Task<bool> PlataformaCuentaExists(string idPlataformaCuenta);

                Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentas(bool complemento);
                Task<UsuarioPlataformaCuentaDTO> GetUsuarioPlataformaCuentabyIds(string id, bool complemento);
                Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentasbyIdUsuario(string id, bool complemento);
                Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentasbyIdPlataforma(string id, bool complemento);
                Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentasbyIdCuenta(string id, bool complemento);
                Task<bool> UsuarioPlataformaCuentaExists(string idPlataformaCuenta);

                Task<List<EstadoDTO>> GetEstados();
                Task<EstadoDTO> GetEstadobyId(int? id);
                Task<EstadoDTO> GetEstadobyName(string Name);
                Task<bool> EstadoExists(int id);
        #endregion
            #region Update
                Task<string> UpdateUsuario(UsuarioDTO usuario);
                Task<string> UpdatePlataforma(PlataformaDTO usuario);
                Task<string> UpdateCuenta(CuentaDTO cuenta);
                Task<string> UpdatePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta);
                Task<string> UpdateEstado(EstadoDTO estado);
            #endregion
            #region Delete
                Task<string> DeleteUsuario(UsuarioDTO usuario);
                Task<string> DeletePlataforma(PlataformaDTO usuario);
                Task<string> DeleteCuenta(CuentaDTO cuenta);
                Task<string> DeletePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta);
                Task<string> DeleteUsuarioPlataformaCuenta(UsuarioPlataformaCuentaDTO usuarioPlataformaCuenta);
                Task<string> DeleteEstado(EstadoDTO estado);
            #endregion
        #endregion
    }
}
