﻿using Billycock.Data;
using Billycock.DTO;
using Billycock.Models;
using Billycock.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Billycock.DTO;
using Web_Billycock.Repositories.Interfaces;

namespace Web_Billycock.Repositories.Repositories
{
    public class Billycock_WebRepository<T>:IBillycock_WebRepository<T> where T:class
    {
        private readonly BillycockServiceContext _context;
        private readonly ICommonRepository<Usuario> _commonRepository_U;
        private readonly ICommonRepository<Plataforma> _commonRepository_P;
        private readonly ICommonRepository<Cuenta> _commonRepository_C;
        private readonly ICommonRepository<PlataformaCuenta> _commonRepository_PC;
        private readonly ICommonRepository<EstadoDTO> _commonRepository_E;
        private readonly ICommonRepository<UsuarioPlataformaCuenta> _commonRepository_UPC;
        private readonly ICommonRepository<Historia> _commonRepository_H;
        
        public Billycock_WebRepository(BillycockServiceContext context, ICommonRepository<Usuario> commonRepository_U,
            ICommonRepository<Plataforma> commonRepository_P, ICommonRepository<Cuenta> commonRepository_C,
            ICommonRepository<EstadoDTO> commonRepository_E, ICommonRepository<PlataformaCuenta> commonRepository_PC,
            ICommonRepository<UsuarioPlataformaCuenta> commonRepository_UPC, ICommonRepository<Historia> commonRepository_H)
        {
            _context = context;
            _commonRepository_U = commonRepository_U;
            _commonRepository_P = commonRepository_P;
            _commonRepository_C = commonRepository_C;
            _commonRepository_E = commonRepository_E;
            _commonRepository_PC = commonRepository_PC;
            _commonRepository_UPC = commonRepository_UPC;
            _commonRepository_H = commonRepository_H;
        }
        #region Metodos Principales
            #region Create
                public async Task<string> InsertUsuario(UsuarioDTO usuario)
                {
                    List<UsuarioPlataformaCuenta> plataformasxusuario = new List<UsuarioPlataformaCuenta>();
                    List<PlataformaCuenta> plataformacuentasTemporal = new List<PlataformaCuenta>();
                    List<PlataformaCuenta> plataformacuentas = new List<PlataformaCuenta>();
                    PlataformaCuenta plataformacuenta = new PlataformaCuenta();
                    List<string> resultadonulo = new List<string>();
                    try
                    {
                        if (usuario.netflix > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 1, cantidad = usuario.netflix });
                        if (usuario.amazon > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 2, cantidad = usuario.amazon });
                        if (usuario.disney > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 3, cantidad = usuario.disney });
                        if (usuario.hbo > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 4, cantidad = usuario.hbo });
                        if (usuario.youtube > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 5, cantidad = usuario.youtube });
                        if (usuario.spotify > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 6, cantidad = usuario.spotify });
                        foreach (var item in plataformasxusuario)
                        {
                            plataformacuentas = new List<PlataformaCuenta>();
                            plataformacuenta = await GetPlataformaCuentaDisponible(item.idPlataforma, item.cantidad);
                            if (plataformacuenta == null)
                            {
                                for (int i = 0; i < item.cantidad; i++)
                                {
                                    plataformacuenta = await GetPlataformaCuentaDisponible(item.idPlataforma, 1);
                                    if (plataformacuenta != null)
                                    {
                                        plataformacuentas.Add(plataformacuenta);

                                        plataformacuenta = await GetPlataformaCuentabyIds(plataformacuenta.idPlataforma.ToString() + "-" + plataformacuenta.idCuenta.ToString(),false);

                                        await UpdatePlataformaCuenta(new PlataformaCuentaDTO()
                                        {
                                            idCuenta = plataformacuenta.idCuenta,
                                            idPlataforma = plataformacuenta.idPlataforma,
                                            fechaPago = plataformacuenta.fechaPago,
                                            usuariosdisponibles = plataformacuenta.usuariosdisponibles - 1
                                        });
                                    }
                                }
                                if (item.cantidad > plataformacuentas.Count)
                                {
                                    resultadonulo.Add(item.cantidad + "-" + (GetPlataformabyId(item.idPlataforma, false)).Result.descripcion);

                                    foreach (var pfc in plataformacuentas)
                                    {
                                        plataformacuenta = await GetPlataformaCuentabyIds(pfc.idPlataforma.ToString() + "-" + pfc.idCuenta.ToString(),false);

                                        await InsertPlataformaCuenta(new PlataformaCuentaDTO()
                                        {
                                            idCuenta = plataformacuenta.idCuenta,
                                            idPlataforma = plataformacuenta.idPlataforma,
                                            fechaPago = plataformacuenta.fechaPago,
                                            usuariosdisponibles = plataformacuenta.usuariosdisponibles + 1
                                        });
                                    }
                                }
                            }
                            else
                            {
                                plataformacuentas.Add(plataformacuenta);

                                plataformacuenta = await GetPlataformaCuentabyIds(plataformacuenta.idPlataforma.ToString() + "-" + plataformacuenta.idCuenta.ToString(),false);

                                await InsertPlataformaCuenta(new PlataformaCuentaDTO()
                                {
                                    idCuenta = plataformacuenta.idCuenta,
                                    idPlataforma = plataformacuenta.idPlataforma,
                                    fechaPago = plataformacuenta.fechaPago,
                                    usuariosdisponibles = plataformacuenta.usuariosdisponibles - item.cantidad
                                });
                            }
                        }
                        if (resultadonulo.Count != 0)
                        {
                            string mensaje = "NO HAY SUFICIENTES USUARIOS DISPONIBLES: " + Environment.NewLine;
                            for (int i = 0; i < resultadonulo.Count; i++)
                            {
                                mensaje += resultadonulo[i];
                                if (i < resultadonulo.Count - 1) mensaje += Environment.NewLine;
                            }
                            return mensaje;
                        }
                        return await _commonRepository_U.InsertObjeto(new Usuario()
                        {
                            descripcion = usuario.descripcion,
                            fechaInscripcion = DateTime.Now,
                            idEstado = 1,
                            facturacion = ObtenerFechaFacturacionUsuario(),
                            pago = await ObtenerMontoPagoUsuario(plataformasxusuario)
                        }, _context);
                        //plataformacuentasTemporal = plataformacuentas.GroupBy(x => x.idPlataformaCuenta)
                        //                    .Select(group => group.First()).ToList();

                        //foreach (var plataformasxusuario in usuario.plataformasxusuario)
                        //{
                        //    for (int i = 0; i < plataformacuentasTemporal.Count; i++)
                        //    {
                        //        user = await GetUsuariobyName(usuario.descripcion);
                        //        await _context.UsuarioPlataformaCuenta.AddAsync(new UsuarioPlataformaCuenta()
                        //        {
                        //            idUsuario = user.idUsuario,
                        //            idPlataforma = plataformasxusuario.idPlataforma,
                        //            cantidad = plataformasxusuario.cantidad,
                        //            idCuenta = plataformacuentasTemporal[i].idCuenta
                        //        });
                        //        await Save();
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return _commonRepository_U.ExceptionMessage(usuario, "C");
                    }
                }
                public async Task<string> InsertPlataforma(PlataformaDTO plataforma)
                {
                    try
                    {
                        return await _commonRepository_P.InsertObjeto(new PlataformaDTO()
                        {
                            descripcion = plataforma.descripcion,
                            idEstado = 1,
                            numeroMaximoUsuarios = plataforma.numeroMaximoUsuarios,
                            precio = plataforma.precio
                        }, _context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return _commonRepository_P.ExceptionMessage(plataforma, "C");
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
                        mensaje = await _commonRepository_C.InsertObjeto(new Cuenta()
                        {
                            diminutivo = cuenta.diminutivo,
                            correo = cuenta.correo,
                            idEstado = 1
                        }, _context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        mensaje = _commonRepository_C.ExceptionMessage(cuenta, "C");
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
                            account = await GetCuentabyName(cuenta.correo, false);
                            foreach (var item in idPlataformas)
                            {
                                if (contador >= 1) mensaje += Environment.NewLine;
                                mensaje += await InsertPlataformaCuenta(new PlataformaCuentaDTO()
                                {
                                    idCuenta = account.idCuenta,
                                    idPlataforma = item,
                                    fechaPago = DateTime.Now.ToShortDateString(),
                                    usuariosdisponibles = GetPlataformabyId(item, false).Result.numeroMaximoUsuarios
                                });
                                contador++;
                            }
                        }
                        catch
                        {
                            mensaje = _commonRepository_C.ExceptionMessage(cuenta, "C");
                        }
                    }
                    return mensaje;
                }
                public async Task<string> InsertPlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
                {
                    plataformaCuenta.idPlataformaCuenta = plataformaCuenta.idPlataforma + "-" + plataformaCuenta.idCuenta;
                    return await _commonRepository_PC.InsertObjeto(plataformaCuenta, _context);
                }
                public async Task<string> InsertEstado(EstadoDTO estado)
        {
            try
            {
                return await _commonRepository_E.InsertObjeto(new EstadoDTO()
                {
                    descripcion = estado.descripcion
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return _commonRepository_E.ExceptionMessage(estado, "C");
            }
        }
                public async Task InsertHistory(T t, string response, BillycockServiceContext _context)
        {
            try
            {
                await _commonRepository_H.InsertObjeto(new Historia()
                {
                    Request = JsonConvert.SerializeObject(t),
                    Response = response,
                    fecha = DateTime.Now
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
            #endregion
            #region Read
                public async Task<List<UsuarioDTO>> GetUsuarios(bool complemento)
                {
                    return await ObtenerUsuarios(1, null, complemento);
                }
                public async Task<UsuarioDTO> GetUsuariobyId(int? id, bool complemento)
                {
                    return (await ObtenerUsuarios(2, id.ToString(), complemento))[0];
                }
                public async Task<bool> UsuarioExists(int id)
                {
                    return await _context.USUARIO.AnyAsync(e => e.idUsuario == id);
                }
                public async Task<List<UsuarioDTO>> ObtenerUsuarios(int tipo, string dato, bool complemento)
                {
                    List<UsuarioDTO> usuarios = new List<UsuarioDTO>();
                    try
                    {
                        if (tipo == 1)
                        {
                            usuarios = await (from u in _context.USUARIO
                                                select new UsuarioDTO()
                                                {
                                                    idUsuario = u.idUsuario,
                                                    descripcion = u.descripcion,
                                                    fechaInscripcion = u.fechaInscripcion,
                                                    idEstado = u.idEstado,
                                                    descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
                                                    facturacion = u.facturacion,
                                                    //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
                                                    //                            where up.idUsuario == u.idUsuario
                                                    //                      orderby up.idUsuario
                                                    //                      select new UsuarioPlataformaCuenta()
                                                    //                      {
                                                    //                          idUsuario = up.idUsuario,
                                                    //                          idPlataforma = up.idPlataforma,
                                                    //                          descPlataforma = (from p in _context.PLATAFORMA
                                                    //                                            where p.idPlataforma == up.idPlataforma
                                                    //                                            select p.descripcion).FirstOrDefault(),
                                                    //                          cantidad = up.cantidad,
                                                    //                          idCuenta = up.idCuenta,
                                                    //                          credencial = (from c in _context.CUENTA
                                                    //                                        where c.idCuenta == up.idCuenta
                                                    //                                        select new UsuarioPlataformaCuenta.Credencial()
                                                    //                                        {
                                                    //                                            usuario = c.descripcion,
                                                    //                                            clave = c.password
                                                    //                                        }).FirstOrDefault()
                                                    //                      }).ToList(),
                                                    pago = u.pago
                                                }).ToListAsync();

                        }
                        else if (tipo == 2)
                        {
                            usuarios = await (from u in _context.USUARIO
                                                where u.idUsuario == int.Parse(dato)
                                                select new UsuarioDTO()
                                                {
                                                    idUsuario = u.idUsuario,
                                                    descripcion = u.descripcion,
                                                    fechaInscripcion = u.fechaInscripcion,
                                                    idEstado = u.idEstado,
                                                    descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
                                                    facturacion = u.facturacion,
                                                    //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
                                                    //                      where up.idUsuario == u.idUsuario
                                                    //                      orderby up.idUsuario
                                                    //                      select new UsuarioPlataformaCuenta()
                                                    //                      {
                                                    //                          idUsuario = up.idUsuario,
                                                    //                          idPlataforma = up.idPlataforma,
                                                    //                          descPlataforma = (from p in _context.PLATAFORMA
                                                    //                                            where p.idPlataforma == up.idPlataforma
                                                    //                                            select p.descripcion).FirstOrDefault(),
                                                    //                          cantidad = up.cantidad,
                                                    //                          idCuenta = up.idCuenta,
                                                    //                          credencial = (from c in _context.CUENTA
                                                    //                                        where c.idCuenta == up.idCuenta
                                                    //                                        select new UsuarioPlataformaCuenta.Credencial()
                                                    //                                        {
                                                    //                                            usuario = c.descripcion,
                                                    //                                            clave = c.password
                                                    //                                        }).FirstOrDefault()
                                                    //                      }).ToList(),
                                                    pago = u.pago
                                                }).ToListAsync();

                        }
                        else
                        {
                            usuarios = await (from u in _context.USUARIO
                                                where u.descripcion == dato
                                                select new UsuarioDTO()
                                                {
                                                    idUsuario = u.idUsuario,
                                                    descripcion = u.descripcion,
                                                    fechaInscripcion = u.fechaInscripcion,
                                                    idEstado = u.idEstado,
                                                    descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
                                                    facturacion = u.facturacion,
                                                    //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
                                                    //                      where up.idUsuario == u.idUsuario
                                                    //                      orderby up.idUsuario
                                                    //                      select new UsuarioPlataformaCuenta()
                                                    //                      {
                                                    //                          idUsuario = up.idUsuario,
                                                    //                          idPlataforma = up.idPlataforma,
                                                    //                          descPlataforma = (from p in _context.PLATAFORMA
                                                    //                                            where p.idPlataforma == up.idPlataforma
                                                    //                                            select p.descripcion).FirstOrDefault(),
                                                    //                          cantidad = up.cantidad,
                                                    //                          idCuenta = up.idCuenta,
                                                    //                          credencial = (from c in _context.CUENTA
                                                    //                                        where c.idCuenta == up.idCuenta
                                                    //                                        select new UsuarioPlataformaCuenta.Credencial()
                                                    //                                        {
                                                    //                                            usuario = c.descripcion,
                                                    //                                            clave = c.password
                                                    //                                        }).FirstOrDefault()
                                                    //                      }).ToList(),
                                                    pago = u.pago
                                                }).ToListAsync();

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    return usuarios;
                }

                public async Task<List<CuentaDTO>> GetCuentas(bool complemento)
                {
                    return await ObtenerCuentas(1, null, complemento);
                }
                public async Task<CuentaDTO> GetCuentabyId(int? id, bool complemento)
                {
                    return (await ObtenerCuentas(2, id.ToString(), complemento))[0];
                }
                public async Task<UsuarioDTO> GetUsuariobyName(string name, bool complemento)
                {
                    return (await ObtenerUsuarios(3, name, complemento))[0];
                }
                public async Task<CuentaDTO> GetCuentabyName(string name, bool complemento)
                {
                    return (await ObtenerCuentas(3, name, complemento))[0];
                }
                public async Task<bool> CuentaExists(int id)
                {
                    return await _context.CUENTA.AnyAsync(e => e.idCuenta == id);
                }
                public async Task<List<CuentaDTO>> ObtenerCuentas(int tipo, string dato, bool complemento)
                {
                    List<CuentaDTO> cuentas;
                    List<PlataformaCuenta> plataformaCuentas = new List<PlataformaCuenta>();
                    if (tipo == 1)
                    {
                        cuentas = await (from c in _context.CUENTA
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
                                         where c.idCuenta == int.Parse(dato)
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
                                         where c.correo == dato
                                         select new CuentaDTO()
                                         {
                                             idCuenta = c.idCuenta,
                                             idEstado = c.idEstado,
                                             descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                             correo = c.correo,
                                             diminutivo = c.diminutivo
                                         }).AsNoTracking().ToListAsync();
                    }
                    if (complemento)
                    {
                        foreach (var _cuenta in cuentas)
                        {
                            foreach (var _plataforma in await GetPlataformas(false))
                            {
                                if (await PlataformaCuentaExists(_plataforma.idPlataforma + "-" + _cuenta.idCuenta))
                                {
                                    if (_plataforma.idPlataforma == 1) _cuenta.netflix = true;
                                    else if (_plataforma.idPlataforma == 2) _cuenta.amazon = true;
                                    else if (_plataforma.idPlataforma == 3) _cuenta.disney = true;
                                    else if (_plataforma.idPlataforma == 4) _cuenta.hbo = true;
                                    else if (_plataforma.idPlataforma == 5) _cuenta.youtube = true;
                                    else _cuenta.spotify = true;
                                    plataformaCuentas.Add(await GetPlataformaCuentabyIds(_plataforma.idPlataforma + "-" + _cuenta.idCuenta,false));
                                }
                            }
                            _cuenta.plataformaCuentas = plataformaCuentas;
                        }
                    }
                    return cuentas;
                }

                public async Task<List<PlataformaDTO>> GetPlataformas(bool complemento)
                {
                    return await ObtenerPlataformas(1, null, complemento);
                }
                public async Task<PlataformaDTO> GetPlataformabyId(int? id, bool complemento)
                {
                    return (await ObtenerPlataformas(2, id.ToString(), complemento))[0];
                }
                public async Task<PlataformaDTO> GetPlataformabyName(string name, bool complemento)
                {
                    return (await ObtenerPlataformas(3, name, complemento))[0];
                }
                public async Task<bool> PlataformaExists(int id)
                {
                    return await _context.PLATAFORMA.AnyAsync(e => e.idPlataforma == id);
                }
                public async Task<List<PlataformaDTO>> ObtenerPlataformas(int tipo, string dato, bool complemento)
                {
                    List<PlataformaDTO> plataformas;
                    List<PlataformaCuenta> plataformaCuentas = new List<PlataformaCuenta>();
                    if (tipo == 1)
                    {
                        plataformas = await (from c in _context.PLATAFORMA
                                             select new PlataformaDTO()
                                             {
                                                 idPlataforma = c.idPlataforma,
                                                 descripcion = c.descripcion,
                                                 idEstado = c.idEstado,
                                                 descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                                 numeroMaximoUsuarios = c.numeroMaximoUsuarios,
                                                 precio = c.precio
                                             }).ToListAsync();
                    }
                    else if (tipo == 2)
                    {
                        plataformas = await (from c in _context.PLATAFORMA
                                             where c.idPlataforma == int.Parse(dato)
                                             select new PlataformaDTO()
                                             {
                                                 idPlataforma = c.idPlataforma,
                                                 descripcion = c.descripcion,
                                                 idEstado = c.idEstado,
                                                 descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                                 numeroMaximoUsuarios = c.numeroMaximoUsuarios,
                                                 precio = c.precio
                                             }).ToListAsync();
                    }
                    else
                    {
                        plataformas = await (from c in _context.PLATAFORMA
                                             where c.descripcion == dato
                                             select new PlataformaDTO()
                                             {
                                                 idPlataforma = c.idPlataforma,
                                                 descripcion = c.descripcion,
                                                 idEstado = c.idEstado,
                                                 descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                                 numeroMaximoUsuarios = c.numeroMaximoUsuarios,
                                                 precio = c.precio
                                             }).ToListAsync();
                    }
                    if (complemento)
                    {
                        foreach (var _plataforma in plataformas)
                        {
                            foreach (var _cuenta in await GetCuentas(false))
                            {
                                if (await PlataformaCuentaExists(_plataforma.idPlataforma + "-" + _cuenta.idCuenta))
                                {
                                    plataformaCuentas.Add(await GetPlataformaCuentabyIds(_plataforma.idPlataforma + "-" + _cuenta.idCuenta,false));
                                }
                            }
                            _plataforma.plataformaCuentas = plataformaCuentas;
                        }
                    }
                    return plataformas;
                }

                public async Task<List<PlataformaCuentaDTO>> GetPlataformaCuentas(bool complemento)
                {
                    return await ObtenerPlataformaCuentas(1, null,complemento);
                }
                public async Task<PlataformaCuentaDTO> GetPlataformaCuentabyIds(string id, bool complemento)
                {
                    return (await ObtenerPlataformaCuentas(2, id,complemento))[0];
                }
                public async Task<PlataformaCuentaDTO> GetPlataformaCuentabyIdPlataforma(string id, bool complemento)
                {
                    return (await ObtenerPlataformaCuentas(3, id,complemento))[0];
                }
                public async Task<PlataformaCuentaDTO> GetPlataformaCuentabyIdCuenta(string id, bool complemento)
                {
                    return (await ObtenerPlataformaCuentas(4, id,complemento))[0];
                }
                public async Task<bool> PlataformaCuentaExists(string idPlataformaCuenta)
                {
                    int idPlataforma = int.Parse(idPlataformaCuenta.Split("-")[0]);
                    int idCuenta = int.Parse(idPlataformaCuenta.Split("-")[1]);
                    return await _context.PLATAFORMACUENTA.AnyAsync(e => e.idPlataforma == idPlataforma
                                                                        && e.idCuenta == idCuenta);
                }
                public async Task<List<PlataformaCuentaDTO>> ObtenerPlataformaCuentas(int tipo, string dato,bool complemento)
                {
                    List<PlataformaCuentaDTO> plataformaCuentas;
                    string[] array;
                    if (tipo == 1)
                    {
                        plataformaCuentas = await (from pc in _context.PLATAFORMACUENTA
                                      select new PlataformaCuentaDTO()
                                      {
                                          idPlataformaCuenta = pc.idPlataformaCuenta,
                                          idPlataforma = pc.idPlataforma,
                                          idCuenta = pc.idCuenta,
                                          clave = pc.clave,
                                          fechaPago = pc.fechaPago,
                                          usuariosdisponibles = pc.usuariosdisponibles
                                      }).ToListAsync();
                    }
                    else if (tipo == 2)
                    {
                        array = dato.Split("-");
                        plataformaCuentas =  await (from pc in _context.PLATAFORMACUENTA
                                      where pc.idPlataforma == int.Parse(array[0]) && pc.idCuenta == int.Parse(array[1])
                                      select new PlataformaCuentaDTO()
                                      {
                                          idPlataformaCuenta = pc.idPlataformaCuenta,
                                          idPlataforma = pc.idPlataforma,
                                          idCuenta = pc.idCuenta,
                                          clave = pc.clave,
                                          fechaPago = pc.fechaPago,
                                          usuariosdisponibles = pc.usuariosdisponibles
                                      }).ToListAsync();
                    }
                    else if (tipo == 3)
                    {
                        plataformaCuentas = await(from pc in _context.PLATAFORMACUENTA
                                      where pc.idPlataforma == int.Parse(dato)
                                      select new PlataformaCuentaDTO()
                                      {
                                          idPlataformaCuenta = pc.idPlataformaCuenta,
                                          idPlataforma = pc.idPlataforma,
                                          idCuenta = pc.idCuenta,
                                          clave = pc.clave,
                                          fechaPago = pc.fechaPago,
                                          usuariosdisponibles = pc.usuariosdisponibles
                                      }).ToListAsync();
                    }
                    else
                    {
                        plataformaCuentas = await(from pc in _context.PLATAFORMACUENTA
                                      where pc.idCuenta == int.Parse(dato)
                                      select new PlataformaCuentaDTO()
                                      {
                                          idPlataformaCuenta = pc.idPlataformaCuenta,
                                          idPlataforma = pc.idPlataforma,
                                          idCuenta = pc.idCuenta,
                                          clave = pc.clave,
                                          fechaPago = pc.fechaPago,
                                          usuariosdisponibles = pc.usuariosdisponibles
                                      }).ToListAsync();
                    }
                    if (complemento)
                    {
                        foreach (var _plataformaCuenta in plataformaCuentas)
                        {
                            _plataformaCuenta.Cuenta = await GetCuentabyId(_plataformaCuenta.idCuenta,false);
                            _plataformaCuenta.Plataforma = await GetPlataformabyId(_plataformaCuenta.idPlataforma,false);
                        }
                    }
                    return plataformaCuentas;
                }

                public async Task<List<EstadoDTO>> GetEstados()
                {
                    return await ObtenerEstados(1, null);
                }
                public async Task<EstadoDTO> GetEstadobyId(int? id)
                {
                    return (await ObtenerEstados(2, id.ToString()))[0];
                }
                public async Task<EstadoDTO> GetEstadobyName(string name)
                {
                    return (await ObtenerEstados(3, name))[0];
                }
                public async Task<bool> EstadoExists(int id)
                {
                    return await _context.ESTADO.AnyAsync(e => e.idEstado == id);
                }
                public async Task<List<EstadoDTO>> ObtenerEstados(int tipo, string dato)
                {
                    if (tipo == 1)
                    {
                        return await (from c in _context.ESTADO
                                      select new EstadoDTO()
                                      {
                                          idEstado = c.idEstado,
                                          descripcion = c.descripcion
                                      }).ToListAsync();
                    }
                    else if (tipo == 2)
                    {
                        return await (from c in _context.ESTADO
                                      where c.idEstado == int.Parse(dato)
                                      select new EstadoDTO()
                                      {
                                          idEstado = c.idEstado,
                                          descripcion = c.descripcion
                                      }).ToListAsync();
                    }
                    else
                    {
                        return await (from c in _context.ESTADO
                                      where c.descripcion == dato
                                      select new EstadoDTO()
                                      {
                                          idEstado = c.idEstado,
                                          descripcion = c.descripcion
                                      }).ToListAsync();
                    }
                }
            #endregion
            #region Update
            public async Task<string> UpdateUsuario(UsuarioDTO usuario)
                {
                    UsuarioDTO user = await GetUsuariobyId(usuario.idUsuario, false);
                    try
                    {
                        return await _commonRepository_U.UpdateObjeto(new Usuario()
                        {
                            idUsuario = user.idUsuario,
                            descripcion = usuario.descripcion,
                            idEstado = usuario.idEstado,
                            fechaInscripcion = user.fechaInscripcion,
                            facturacion = usuario.facturacion,
                            pago = usuario.pago
                        }, _context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return _commonRepository_U.ExceptionMessage(usuario, "U");
                    }
                }
            public async Task<string> UpdatePlataforma(PlataformaDTO plataforma)
            {
                PlataformaDTO platform = await GetPlataformabyId(plataforma.idPlataforma, false);
                try
                {
                    return await _commonRepository_P.UpdateObjeto(new PlataformaDTO()
                    {
                        idPlataforma = platform.idPlataforma,
                        descripcion = plataforma.descripcion,
                        idEstado = plataforma.idEstado,
                        numeroMaximoUsuarios = plataforma.numeroMaximoUsuarios,
                        precio = plataforma.precio
                    }, _context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return _commonRepository_P.ExceptionMessage(plataforma, "U");
                }
            }
            public async Task<string> UpdateCuenta(CuentaDTO cuenta)
            {
            string mensaje = string.Empty;
            List<int> idPlataformasAgregar = new List<int>();
            List<int> idPlataformasEliminar = new List<int>();

            CuentaDTO account = await GetCuentabyId(cuenta.idCuenta, true);
            try
            {
                mensaje = await _commonRepository_C.UpdateObjeto(new Cuenta()
                {
                    idCuenta = account.idCuenta,
                    diminutivo = cuenta.diminutivo,
                    correo = cuenta.correo,
                    idEstado = cuenta.idEstado
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = _commonRepository_C.ExceptionMessage(cuenta, "U");
            }

            if (mensaje.Contains("CORRECTA"))
            {
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
                        mensaje += await InsertPlataformaCuenta(new PlataformaCuentaDTO()
                        {
                            idPlataforma = item,
                            idCuenta = cuenta.idCuenta,
                            fechaPago = DateTime.Now.ToShortDateString(),
                            usuariosdisponibles = GetPlataformabyId(item, false).Result.numeroMaximoUsuarios
                        });
                    }
                    foreach (var item in idPlataformasEliminar)
                    {
                        mensaje += Environment.NewLine;
                        mensaje += await DeletePlataformaCuenta(new PlataformaCuentaDTO()
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
            public async Task<string> UpdatePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
            {
                PlataformaCuentaDTO platformAccount = await GetPlataformaCuentabyIds(plataformaCuenta.idPlataformaCuenta,false);
                string mensaje = string.Empty;
                try
                {
                    mensaje = await _commonRepository_PC.UpdateObjeto(new PlataformaCuenta()
                    {
                        idPlataformaCuenta = platformAccount.idPlataformaCuenta,
                        idCuenta = platformAccount.idCuenta,
                        idPlataforma = platformAccount.idPlataforma,
                        fechaPago = plataformaCuenta.fechaPago == null? DateTime.Parse(platformAccount.fechaPago).AddMonths(1).ToShortDateString(): platformAccount.fechaPago,
                        usuariosdisponibles = platformAccount.usuariosdisponibles,
                        clave = plataformaCuenta.clave != platformAccount.clave? plataformaCuenta.clave: platformAccount.clave
                    }, _context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    mensaje = _commonRepository_PC.ExceptionMessage(plataformaCuenta, "U");
                }
                return mensaje;
            }

        

        public async Task<string> UpdateEstado(EstadoDTO estado)
        {
            EstadoDTO account = await GetEstadobyId(estado.idEstado);
            try
            {
                return await _commonRepository_E.UpdateObjeto(new EstadoDTO()
                {
                    idEstado = account.idEstado,
                    descripcion = estado.descripcion
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return _commonRepository_E.ExceptionMessage(estado, "U");
            }
        }
            #endregion
            #region Delete
                public async Task<string> DeleteUsuario(UsuarioDTO usuario)
                {
                    UsuarioDTO user = await GetUsuariobyId(usuario.idUsuario, false);
                    try
                    {
                        return await _commonRepository_U.DeleteLogicoObjeto(new Usuario()
                        {
                            idUsuario = user.idUsuario,
                            descripcion = user.descripcion,
                            idEstado = 2,
                            fechaInscripcion = user.fechaInscripcion,
                            facturacion = user.facturacion,
                            pago = user.pago
                        }, _context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return _commonRepository_U.ExceptionMessage(usuario, "D");
                    }
                }
                public async Task<string> DeletePlataforma(PlataformaDTO plataforma)
                {
                    PlataformaDTO account = await GetPlataformabyId(plataforma.idPlataforma, false);
                    try
                    {
                        return await _commonRepository_P.DeleteLogicoObjeto(new PlataformaDTO()
                        {
                            idPlataforma = account.idPlataforma,
                            descripcion = plataforma.descripcion,
                            idEstado = 2,
                            numeroMaximoUsuarios = account.numeroMaximoUsuarios,
                            precio = account.precio
                        }, _context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return _commonRepository_P.ExceptionMessage(plataforma, "D");
                    }
                }
                public async Task<string> DeleteCuenta(CuentaDTO cuenta)
        {
            CuentaDTO account = await GetCuentabyId(cuenta.idCuenta, false);
            try
            {
                return await _commonRepository_C.DeleteLogicoObjeto(new Cuenta()
                {
                    idCuenta = account.idCuenta,
                    diminutivo = account.diminutivo,
                    correo = account.correo,
                    idEstado = 2
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return _commonRepository_C.ExceptionMessage(cuenta, "D");
            }
        }
                public async Task<string> DeletePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
            {
                return await _commonRepository_PC.DeleteObjeto(plataformaCuenta, _context);
            }
                public async Task<string> DeleteEstado(EstadoDTO estado)
        {
            EstadoDTO state = await GetEstadobyId(estado.idEstado);
            try
            {
                return await _commonRepository_E.DeleteObjeto(state, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return _commonRepository_E.ExceptionMessage(estado, "D");
            }
        }
            #endregion
        #endregion
        #region Metodos secundarios
            private string ObtenerFechaFacturacionUsuario()
            {
                DateTime fechaHoy = DateTime.Now;
                bool QuincenaMes = fechaHoy.Day <= 15 ? true : false;
                DateTime oPrimerDiaDelMes = new DateTime(fechaHoy.Year, fechaHoy.Month, 1);
                if (fechaHoy.Month < 12)
                {
                    if (QuincenaMes)
                    {
                        return new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1).ToShortDateString();
                    }
                    else
                    {
                        return oPrimerDiaDelMes.AddMonths(2).AddDays(-1).ToShortDateString();
                    }
                }
                else
                {
                    if (QuincenaMes)
                    {
                        return new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1).ToShortDateString();
                    }
                    else
                    {
                        return oPrimerDiaDelMes.AddMonths(2).AddDays(-1).ToShortDateString();
                    }
                }
            }
            private async Task<int?> ObtenerMontoPagoUsuario(List<UsuarioPlataformaCuenta> UsuarioPlataformaCuentas)
            {
                int? pago = 0;
                double? acumulado = 0;
                for (int i = 0; i < UsuarioPlataformaCuentas.Count; i++)
                {
                    acumulado += await GetPricePlataforma(UsuarioPlataformaCuentas[i].idPlataforma) * UsuarioPlataformaCuentas[i].cantidad;

                    if (i == UsuarioPlataformaCuentas.Count - 1)
                    {
                        if (UsuarioPlataformaCuentas[i].cantidad == 1 && UsuarioPlataformaCuentas.Count > 1) { pago = reprocesoUsuario(1, UsuarioPlataformaCuentas.Count, acumulado); }
                        else if (UsuarioPlataformaCuentas[i].cantidad > 1 && UsuarioPlataformaCuentas.Count == 1) { pago = reprocesoUsuario(2, UsuarioPlataformaCuentas[i].cantidad, acumulado); }
                        else pago = Convert.ToInt16(acumulado);
                    }
                }
                return pago;
            }
            private int reprocesoUsuario(int tipo, int? cuenta, double? monto)
            {
                if (tipo == 1)
                {
                    return (int)((monto / cuenta) * (cuenta * 0.9));
                }
                else
                {
                    return (int)(monto * 0.85);
                }
            }
            private async Task<double> GetPricePlataforma(int id)
            {
                return (await (from p in _context.PLATAFORMA
                               where p.idPlataforma == id
                               select p.precio).FirstOrDefaultAsync());
            }
            public async Task<PlataformaCuentaDTO> GetPlataformaCuentaDisponible(int idPlataforma, int? cantidad)
        {
            return await (from pc in _context.PLATAFORMACUENTA
                          join c in _context.CUENTA on pc.idCuenta equals c.idCuenta
                          where pc.idPlataforma == idPlataforma && pc.usuariosdisponibles >= cantidad && c.idEstado != 2
                          select new PlataformaCuentaDTO()
                          {
                              idPlataformaCuenta = pc.idCuenta.ToString() + "-" + pc.idPlataforma.ToString(),
                              idCuenta = pc.idCuenta,
                              idPlataforma = pc.idPlataforma,
                              usuariosdisponibles = pc.usuariosdisponibles,
                              fechaPago = pc.fechaPago
                          }).FirstOrDefaultAsync();
        }
        #endregion Metodos secundarios
    }
}