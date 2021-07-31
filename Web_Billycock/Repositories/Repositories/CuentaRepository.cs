using Billycock.Data;
using Billycock.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billycock.Repositories.Interfaces;
using Billycock.Utils;
using Billycock.DTO;

namespace Billycock.Repositories.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly BillycockServiceContext _context;
        private readonly ICommonRepository<Cuenta> _commonRepository;
        private readonly IPlataformaCuentaRepository _plataformaCuentaRepository;
        private readonly IPlataformaRepository _plataformaRepository;
        public CuentaRepository(BillycockServiceContext context, ICommonRepository<Cuenta> commonRepository,
            IPlataformaCuentaRepository plataformaCuentaRepository, IPlataformaRepository plataformaRepository)
        {
            _context = context;
            _commonRepository = commonRepository;
            _plataformaCuentaRepository = plataformaCuentaRepository;
            _plataformaRepository = plataformaRepository;
        }
        #region Metodos Principales
        public async Task<string> DeleteCuenta(CuentaDTO cuenta)
        {
            CuentaDTO account = await GetCuentabyId(cuenta.idCuenta);
            try
            {
                return await _commonRepository.DeleteLogicoObjeto(new Cuenta()
                {
                    idCuenta = account.idCuenta,
                    diminutivo = account.diminutivo,
                    correo = account.correo,
                    //netflix = account.netflix,
                    //amazon = account.amazon,
                    //disney = account.disney,
                    //hbo = account.hbo,
                    //youtube = account.youtube,
                    //spotify = account.spotify,
                    idEstado = 2
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return _commonRepository.ExceptionMessage(cuenta, "D");
            }
        }
        public async Task<string> InsertCuenta(CuentaDTO cuenta)
        {
            CuentaDTO account;
            string mensaje = string.Empty;
            List<int> idPlataformas = new List<int>();
            int contador = 0;

            try
            {
                mensaje = await _commonRepository.InsertObjeto(new Cuenta()
                {
                    diminutivo = cuenta.diminutivo,
                    correo = cuenta.correo,
                    idEstado = 1
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = _commonRepository.ExceptionMessage(cuenta, "C");
            }
            if (mensaje.Contains("CORRECTA"))
            {
                mensaje += Environment.NewLine;
                try
                {
                    if (cuenta.netflix) idPlataformas.Add(1);
                    if (cuenta.amazon) idPlataformas.Add(2);
                    if (cuenta.disney) idPlataformas.Add(3);
                    if (cuenta.hbo) idPlataformas.Add(4);
                    if (cuenta.youtube) idPlataformas.Add(5);
                    if (cuenta.spotify) idPlataformas.Add(6);
                    account = await GetCuentabyName(cuenta.correo);
                    foreach (var item in idPlataformas)
                    {
                        if (contador >= 1) mensaje += Environment.NewLine;
                        mensaje += await _plataformaCuentaRepository.InsertPlataformaCuenta(new PlataformaCuentaDTO()
                        {
                            idCuenta = account.idCuenta,
                            idPlataforma = item,
                            fechaPago = DateTime.Now.ToShortDateString(),
                            usuariosdisponibles = _plataformaRepository.GetPlataformabyId(item).Result.numeroMaximoUsuarios
                        });
                        contador++;
                    }
                }
                catch
                {
                    mensaje = _commonRepository.ExceptionMessage(cuenta, "C");
                }
            }
            return mensaje;
        }
        public async Task<string> UpdateCuenta(CuentaDTO cuenta)
        {
            string mensaje = string.Empty;
            List<int> idPlataformasAgregar = new List<int>();
            List<int> idPlataformasEliminar = new List<int>();

            try
            {
                mensaje = await _commonRepository.UpdateObjeto(new Cuenta()
                {
                    idCuenta = cuenta.idCuenta ,
                    diminutivo = cuenta.diminutivo,
                    correo = cuenta.correo,
                    idEstado = cuenta.idEstado
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = _commonRepository.ExceptionMessage(cuenta, "U");
            }

            if (mensaje.Contains("CORRECTA"))
            {
                CuentaDTO account = await GetCuentabyId(cuenta.idCuenta);
                try
                {
                    if (cuenta.netflix != account.netflix)
                    {
                        if (cuenta.netflix == false) idPlataformasEliminar.Add(1);
                        else idPlataformasAgregar.Add(1);
                    }
                    if (cuenta.amazon != account.amazon)
                    {
                        if (cuenta.amazon == false) idPlataformasEliminar.Add(2);
                        else idPlataformasAgregar.Add(2);
                    }
                    if (cuenta.disney != account.disney)
                    {
                        if (cuenta.disney == false) idPlataformasEliminar.Add(3);
                        else idPlataformasAgregar.Add(3);
                    }
                    if (cuenta.hbo != account.hbo)
                    {
                        if (cuenta.hbo == false) idPlataformasEliminar.Add(4);
                        else idPlataformasAgregar.Add(4);
                    }
                    if (cuenta.youtube != account.youtube)
                    {
                        if (cuenta.youtube == false) idPlataformasEliminar.Add(5);
                        else idPlataformasAgregar.Add(5);
                    }
                    if (cuenta.spotify != account.spotify)
                    {
                        if (cuenta.spotify == false) idPlataformasEliminar.Add(6);
                        else idPlataformasAgregar.Add(6);
                    }
                    foreach (var item in idPlataformasAgregar)
                    {
                        mensaje += Environment.NewLine;
                        mensaje += await _plataformaCuentaRepository.InsertPlataformaCuenta(new PlataformaCuentaDTO()
                        {
                            idPlataforma = item,
                            idCuenta = cuenta.idCuenta,
                            fechaPago = DateTime.Now.ToShortDateString(),
                            usuariosdisponibles = _plataformaRepository.GetPlataformabyId(item).Result.numeroMaximoUsuarios
                        });
                    }
                    foreach (var item in idPlataformasEliminar)
                    {
                        mensaje += Environment.NewLine;
                        mensaje += await _plataformaCuentaRepository.DeletePlataformaCuenta(new PlataformaCuentaDTO()
                        {
                            idCuenta = cuenta.idCuenta,
                            idPlataforma = item
                        });
                    }
                }
                catch
                {
                    mensaje += "ERROR EN LA ACTUALIZACION DE PLATAFORMAS EN CUENTA-SERVER";
                }
            }
            return mensaje;
        }
        public async Task<List<CuentaDTO>> GetCuentas()
        {
            return await ObtenerCuentas(1, "");
        }
        public async Task<CuentaDTO> GetCuentabyId(int? id)
        {
            return (await ObtenerCuentas(2, id.ToString()))[0];
        }
        public async Task<CuentaDTO> GetCuentabyName(string name)
        {
            return (await ObtenerCuentas(3, name))[0];
        }
        public async Task<bool> CuentaExists(int id)
        {
            return await _context.CUENTA.AnyAsync(e => e.idCuenta == id);
        }
        public async Task<List<CuentaDTO>> ObtenerCuentas(int tipo, string dato)
        {
            List<CuentaDTO> cuentas;
            List<PlataformaCuenta> plataformaCuentas = new List<PlataformaCuenta>();
            if (tipo == 1)
            {
                cuentas = await (from c in _context.CUENTA
                                 where c.idEstado != 2
                                 select new CuentaDTO()
                                 {
                                     idCuenta = c.idCuenta,
                                     idEstado = c.idEstado,
                                     descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                     correo = c.correo,
                                     diminutivo = c.diminutivo
                                 }).AsNoTracking().ToListAsync();
            }
            else if (tipo == 2)
            {
                cuentas = await (from c in _context.CUENTA
                                 where c.idEstado != 2 && c.idCuenta == int.Parse(dato)
                                 select new CuentaDTO()
                                 {
                                     idCuenta = c.idCuenta,
                                     idEstado = c.idEstado,
                                     descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                     correo = c.correo,
                                     diminutivo = c.diminutivo
                                 }).AsNoTracking().ToListAsync();
            }
            else
            {
                cuentas = await (from c in _context.CUENTA
                                 where c.idEstado != 2 && c.correo == dato
                                 select new CuentaDTO()
                                 {
                                     idCuenta = c.idCuenta,
                                     idEstado = c.idEstado,
                                     descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                     correo = c.correo,
                                     diminutivo = c.diminutivo
                                 }).AsNoTracking().ToListAsync();
            }
            foreach (var _cuenta in cuentas)
            {
                foreach (var _plataforma in await _plataformaRepository.GetPlataformas())
                {
                    if (await _plataformaCuentaRepository.PlataformaCuentaExists(_plataforma.idPlataforma + "-" + _cuenta.idCuenta))
                    {
                        if (_plataforma.idPlataforma == 1) _cuenta.netflix = true;
                        else if (_plataforma.idPlataforma == 2) _cuenta.amazon = true;
                        else if (_plataforma.idPlataforma == 3) _cuenta.disney = true;
                        else if (_plataforma.idPlataforma == 4) _cuenta.hbo = true;
                        else if (_plataforma.idPlataforma == 5) _cuenta.youtube = true;
                        else _cuenta.spotify = true;
                        plataformaCuentas.Add(await _plataformaCuentaRepository.GetPlataformaCuentabyIds(_plataforma.idPlataforma + "-" + _cuenta.idCuenta));
                    }
                }
                _cuenta.plataformaCuentas = plataformaCuentas;
            }
            return cuentas;
        }
        #endregion
        #region Metodos secundarios
        #endregion

    }
}
