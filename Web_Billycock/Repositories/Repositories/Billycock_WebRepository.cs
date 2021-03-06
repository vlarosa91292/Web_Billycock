using Billycock.Data;
using Billycock.DTO;
using Billycock.Models;
using Billycock.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly ICommonRepository<Estado> _commonRepository_E;
        private readonly ICommonRepository<UsuarioPlataformaCuenta> _commonRepository_UPC;
        
        public Billycock_WebRepository(BillycockServiceContext context, ICommonRepository<Usuario> commonRepository_U,
            ICommonRepository<Plataforma> commonRepository_P, ICommonRepository<Cuenta> commonRepository_C,
            ICommonRepository<Estado> commonRepository_E, ICommonRepository<PlataformaCuenta> commonRepository_PC,
            ICommonRepository<UsuarioPlataformaCuenta> commonRepository_UPC)
        {
            _context = context;
            _commonRepository_U = commonRepository_U;
            _commonRepository_P = commonRepository_P;
            _commonRepository_C = commonRepository_C;
            _commonRepository_E = commonRepository_E;
            _commonRepository_PC = commonRepository_PC;
            _commonRepository_UPC = commonRepository_UPC;
        }

        public async Task<string> InsertUsuario(UsuarioDTO usuario)
        {
            List<UsuarioPlataformaCuenta> plataformasxusuario = new List<UsuarioPlataformaCuenta>();
            List<PlataformaCuenta> plataformacuentasTotalitario = new List<PlataformaCuenta>();
            List<PlataformaCuenta> plataformacuentasTemporal = new List<PlataformaCuenta>();
            List<PlataformaCuenta> plataformacuentas = new List<PlataformaCuenta>();
            PlataformaCuenta plataformacuenta = new PlataformaCuenta();
            List<string> resultadonulo = new List<string>();
            string mensaje = string.Empty;  
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
                                plataformacuentasTotalitario.Add(plataformacuenta);

                                plataformacuenta = await GetPlataformaCuentabyIds(plataformacuenta.idPlataformaCuenta,false);

                                if (mensaje != string.Empty) mensaje += Environment.NewLine;
                                mensaje += await UpdatePlataformaCuenta(new PlataformaCuentaDTO()
                                {
                                    idPlataformaCuenta = plataformacuenta.idPlataformaCuenta,
                                    idCuenta = plataformacuenta.idCuenta,
                                    idPlataforma = plataformacuenta.idPlataforma,
                                    fechaPago = plataformacuenta.fechaPago,
                                    usuariosdisponibles = plataformacuenta.usuariosdisponibles - 1,
                                    clave = plataformacuenta.clave
                                });
                            }
                        }
                        if (item.cantidad > plataformacuentas.Count)
                        {
                            resultadonulo.Add(item.cantidad + "-" + (GetPlataformabyId(item.idPlataforma, false)).Result.descripcion);
                        }
                    }
                    else
                    {
                        plataformacuentas.Add(plataformacuenta);
                        plataformacuentasTotalitario.Add(plataformacuenta);

                        plataformacuenta = await GetPlataformaCuentabyIds(plataformacuenta.idPlataformaCuenta, false);
                        if (mensaje != string.Empty) mensaje += Environment.NewLine;
                        mensaje += await UpdatePlataformaCuenta(new PlataformaCuentaDTO()
                        {
                            idPlataformaCuenta = plataformacuenta.idPlataformaCuenta,
                            idCuenta = plataformacuenta.idCuenta,
                            idPlataforma = plataformacuenta.idPlataforma,
                            fechaPago = plataformacuenta.fechaPago,
                            usuariosdisponibles = plataformacuenta.usuariosdisponibles - item.cantidad,
                            clave = plataformacuenta.clave
                        });
                    }
                }
                if (resultadonulo.Any())
                {
                    if(mensaje != string.Empty)mensaje += Environment.NewLine;
                    mensaje += "NO HAY SUFICIENTES USUARIOS DISPONIBLES: ";
                    plataformacuentasTemporal = plataformacuentasTotalitario;
                    plataformacuentasTotalitario = plataformacuentasTotalitario.GroupBy(x => x.idPlataformaCuenta)
                                            .Select(group => group.First()).ToList();

                    for (int i = 0; i < resultadonulo.Count; i++)
                    {
                        mensaje += Environment.NewLine;
                        mensaje += resultadonulo[i];
                    }
                    
                    foreach (var item in plataformacuentasTotalitario)
                    {
                        plataformacuenta = await GetPlataformaCuentabyIds(item.idPlataformaCuenta, false);
                        mensaje += Environment.NewLine;
                        mensaje += Environment.NewLine;
                        mensaje += await UpdatePlataformaCuenta(new PlataformaCuentaDTO()
                        {
                            idPlataformaCuenta = plataformacuenta.idPlataformaCuenta,
                            idCuenta = plataformacuenta.idCuenta,
                            idPlataforma = plataformacuenta.idPlataforma,
                            fechaPago = plataformacuenta.fechaPago,
                            usuariosdisponibles = plataformacuenta.usuariosdisponibles + plataformacuentasTemporal.Where(p => p.idPlataformaCuenta == item.idPlataformaCuenta).ToList().Count,
                            clave = plataformacuenta.clave
                        });
                    }
                            
                    return mensaje;
                }

                mensaje += Environment.NewLine;
                mensaje += await _commonRepository_U.InsertObjeto(new Usuario()
                {
                    descripcion = usuario.descripcion,
                    fechaInscripcion = _commonRepository_U.SetearFechaTiempo(),
                    idEstado = 1,
                    facturacion = _commonRepository_U.ObtenerFechaFacturacionUsuario(),
                    pago = await ObtenerMontoPagoUsuario(plataformasxusuario)
                }, _context);

                foreach (var item in plataformasxusuario)      
                {
                    Usuario user = await GetUsuariobyName(usuario.descripcion,false);
                    if(plataformacuentasTotalitario.Where(p => p.idPlataforma == item.idPlataforma).ToList().Count == 1)
                    {
                        mensaje += Environment.NewLine;
                        mensaje += await InsertUsuarioPlataformaCuenta(new UsuarioPlataformaCuentaDTO()
                        {
                            idUsuario = user.idUsuario,
                            idPlataforma = item.idPlataforma,
                            idCuenta = plataformacuentasTotalitario.Where(p => p.idPlataforma == item.idPlataforma).FirstOrDefault().idCuenta,
                            cantidad = item.cantidad
                        });
                    }
                    else 
                    {
                        plataformacuentasTemporal = plataformacuentasTotalitario.Where(p => p.idPlataforma == item.idPlataforma).ToList();
                        plataformacuentas = plataformacuentasTemporal.GroupBy(x => x.idPlataformaCuenta)
                                            .Select(group => group.First()).ToList();
                        foreach (var conteo in plataformacuentas)
                        {
                            mensaje += Environment.NewLine;
                            mensaje += await InsertUsuarioPlataformaCuenta(new UsuarioPlataformaCuentaDTO()
                            {
                                idUsuario = user.idUsuario,
                                idPlataforma = item.idPlataforma,
                                idCuenta = conteo.idCuenta,
                                cantidad = plataformacuentasTemporal.Where(p => p.idPlataformaCuenta == conteo.idPlataformaCuenta).ToList().Count
                            });
                        }
                    }
                }
            }
            catch 
            {
                mensaje += await _commonRepository_U.ExceptionMessage(usuario, "C");
            }
            return mensaje;
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
                return await _commonRepository_P.ExceptionMessage(plataforma, "C");
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
                mensaje += await _commonRepository_C.InsertObjeto(new Cuenta()
                {
                    diminutivo = cuenta.diminutivo,
                    correo = cuenta.correo,
                    idEstado = 1
                }, _context);
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
                                fechaPago = _commonRepository_C.SetearFecha(DateTime.Now),
                                usuariosdisponibles = GetPlataformabyId(item, false).Result.numeroMaximoUsuarios
                            });
                            contador++;
                        }
                    }
                    catch
                    {
                        mensaje += _commonRepository_C.ExceptionMessage(cuenta, "C");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje += await _commonRepository_C.ExceptionMessage(cuenta, "C");
            }
            return mensaje;
        }
        public async Task<string> InsertPlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
        {
            try
            {
                plataformaCuenta.idPlataformaCuenta = plataformaCuenta.idPlataforma + "-" + plataformaCuenta.idCuenta;
                return await _commonRepository_PC.InsertObjeto(new PlataformaCuenta()
                {
                    idPlataformaCuenta = plataformaCuenta.idPlataformaCuenta,
                    idPlataforma = plataformaCuenta.idPlataforma,
                    idCuenta = plataformaCuenta.idCuenta,
                    fechaPago = plataformaCuenta.fechaPago,
                    clave = plataformaCuenta.clave,
                    usuariosdisponibles = plataformaCuenta.usuariosdisponibles
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await _commonRepository_PC.ExceptionMessage(plataformaCuenta, "C");
            }
        }
        public async Task<string> InsertUsuarioPlataformaCuenta(UsuarioPlataformaCuentaDTO usuarioPlataformaCuenta)
        {
            try
            {
                usuarioPlataformaCuenta.idUsuarioPlataformaCuenta = usuarioPlataformaCuenta.idUsuario + "-" + usuarioPlataformaCuenta.idPlataforma + "-" + usuarioPlataformaCuenta.idCuenta;
                return await _commonRepository_UPC.InsertObjeto(new UsuarioPlataformaCuenta()
                {
                    idUsuarioPlataformaCuenta = usuarioPlataformaCuenta.idUsuarioPlataformaCuenta,
                    idUsuario = usuarioPlataformaCuenta.idUsuario,
                    idPlataforma = usuarioPlataformaCuenta.idPlataforma,
                    idCuenta = usuarioPlataformaCuenta.idCuenta,
                    cantidad = usuarioPlataformaCuenta.cantidad
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await _commonRepository_UPC.ExceptionMessage(usuarioPlataformaCuenta, "C");
            }
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
                return await _commonRepository_E.ExceptionMessage(estado, "C");
            }
        }

        public async Task<List<UsuarioDTO>> GetUsuarios(bool complemento)
        {
            return await ObtenerUsuarios(1, null, complemento);
        }
        public async Task<UsuarioDTO> GetUsuariobyId(int? id, bool complemento)
        {
            List<UsuarioDTO> usuarios = await ObtenerUsuarios(2, id.ToString(), complemento);
            if (usuarios.Count == 1 ) return usuarios[0];
            else return null;
        }
        public async Task<UsuarioDTO> GetUsuariobyName(string name, bool complemento)
        {
            List<UsuarioDTO> usuarios = await ObtenerUsuarios(3, name, complemento);
            if (usuarios.Count == 1 )  return usuarios[0];
            else return null;
        }
        public async Task<bool> UsuarioExists(int id)
        {
            return await _context.USUARIO.AnyAsync(e => e.idUsuario == id);
        }
        public async Task<List<UsuarioDTO>> ObtenerUsuarios(int tipo, string dato, bool complemento)
        {
            List<UsuarioDTO> usuarios = new List<UsuarioDTO>();
            List<UsuarioPlataformaCuenta> usuarioPlataformaCuentas = new List<UsuarioPlataformaCuenta>();
            UsuarioPlataformaCuenta usuarioPlataformaCuenta = new UsuarioPlataformaCuenta();
            try
            {
                if (tipo == 1)
                {
                    usuarios = await (from u in _context.USUARIO
                                        orderby u.idUsuario
                                        select new UsuarioDTO()
                                        {
                                            idUsuario = u.idUsuario,
                                            descripcion = u.descripcion,
                                            fechaInscripcion = u.fechaInscripcion,
                                            idEstado = u.idEstado,
                                            descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
                                            facturacion = u.facturacion,
                                            pago = u.pago
                                        }).ToListAsync();

                }
                else if (tipo == 2)
                {
                    usuarios = await (from u in _context.USUARIO
                                        where u.idUsuario == int.Parse(dato)
                                        orderby u.idUsuario
                                        select new UsuarioDTO()
                                        {
                                            idUsuario = u.idUsuario,
                                            descripcion = u.descripcion,
                                            fechaInscripcion = u.fechaInscripcion,
                                            idEstado = u.idEstado,
                                            descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
                                            facturacion = u.facturacion,
                                            pago = u.pago
                                        }).ToListAsync();

                }
                else
                {
                    usuarios = await (from u in _context.USUARIO
                                        where u.descripcion == dato
                                        orderby u.idUsuario
                                        select new UsuarioDTO()
                                        {
                                            idUsuario = u.idUsuario,
                                            descripcion = u.descripcion,
                                            fechaInscripcion = u.fechaInscripcion,
                                            idEstado = u.idEstado,
                                            descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
                                            facturacion = u.facturacion,
                                            pago = u.pago,
                                                
                                        }).ToListAsync();

                }
                if(complemento)
                {
                    foreach (var _usuario in usuarios)
                    {
                        foreach (var _plataforma in await GetPlataformas(false))
                        {
                            foreach (var _cuenta in await GetCuentas(false))
                            {
                                if (await UsuarioPlataformaCuentaExists(_usuario.idUsuario + "-"+ _plataforma.idPlataforma + "-" + _cuenta.idCuenta))
                                {
                                    usuarioPlataformaCuenta = await GetUsuarioPlataformaCuentabyIds(_usuario.idUsuario + "-" + _plataforma.idPlataforma + "-" + _cuenta.idCuenta, true);
                                    if(usuarioPlataformaCuenta != null)
                                    {
                                        if (_plataforma.idPlataforma == 1) _usuario.netflix += usuarioPlataformaCuenta.cantidad;
                                        else if (_plataforma.idPlataforma == 2) _usuario.amazon += usuarioPlataformaCuenta.cantidad;
                                        else if (_plataforma.idPlataforma == 3) _usuario.disney += usuarioPlataformaCuenta.cantidad;
                                        else if (_plataforma.idPlataforma == 4) _usuario.hbo += usuarioPlataformaCuenta.cantidad;
                                        else if (_plataforma.idPlataforma == 5) _usuario.youtube += usuarioPlataformaCuenta.cantidad;
                                        else if (_plataforma.idPlataforma == 6) _usuario.spotify += usuarioPlataformaCuenta.cantidad;
                                        usuarioPlataformaCuentas.Add(usuarioPlataformaCuenta);
                                    }
                                }
                            }
                        }
                        _usuario.usuarioPlataformaCuentas = usuarioPlataformaCuentas;
                    }
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
            List<CuentaDTO> cuentas = await ObtenerCuentas(2, id.ToString(), complemento);
            if (cuentas.Count == 1) return cuentas[0];
            else  return null;
        }
        public async Task<CuentaDTO> GetCuentabyName(string name, bool complemento)
        {
            List<CuentaDTO> cuentas = await ObtenerCuentas(3, name, complemento);
            if (cuentas.Count == 1)  return cuentas[0];
            else return null;
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
                                    orderby c.idCuenta
                                    select new CuentaDTO()
                                    {
                                        idCuenta = c.idCuenta,
                                        idEstado = c.idEstado,
                                        descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                        correo = c.correo,
                                        diminutivo = c.diminutivo
                                    }).ToListAsync();
            }
            else if (tipo == 2)
            {
                cuentas = await (from c in _context.CUENTA
                                    where c.idCuenta == int.Parse(dato)
                                    orderby c.idCuenta
                                    select new CuentaDTO()
                                    {
                                        idCuenta = c.idCuenta,
                                        idEstado = c.idEstado,
                                        descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                        correo = c.correo,
                                        diminutivo = c.diminutivo
                                    }).ToListAsync();
            }
            else
            {
                cuentas = await (from c in _context.CUENTA
                                    where c.correo == dato
                                    orderby c.idCuenta
                                    select new CuentaDTO()
                                    {
                                        idCuenta = c.idCuenta,
                                        idEstado = c.idEstado,
                                        descEstado = (from e in _context.ESTADO where e.idEstado == c.idEstado select e.descripcion).FirstOrDefault(),
                                        correo = c.correo,
                                        diminutivo = c.diminutivo
                                    }).ToListAsync();
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
                            plataformaCuentas.Add(await GetPlataformaCuentabyIds(_plataforma.idPlataforma + "-" + _cuenta.idCuenta,true));
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
            List<PlataformaDTO> plataformas = await ObtenerPlataformas(2, id.ToString(), complemento);
            if (plataformas.Count == 1)  return plataformas[0];
            else return null;
        }
        public async Task<PlataformaDTO> GetPlataformabyName(string name, bool complemento)
        {
            List<PlataformaDTO> plataformas = await ObtenerPlataformas(2, name, complemento);
            if (plataformas.Count == 1)  return plataformas[0];
            else return null;
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
                plataformas = await (from p in _context.PLATAFORMA
                                        orderby p.idPlataforma
                                        select new PlataformaDTO()
                                        {
                                            idPlataforma = p.idPlataforma,
                                            descripcion = p.descripcion,
                                            idEstado = p.idEstado,
                                            descEstado = (from e in _context.ESTADO where e.idEstado == p.idEstado select e.descripcion).FirstOrDefault(),
                                            numeroMaximoUsuarios = p.numeroMaximoUsuarios,
                                            precio = p.precio
                                        }).ToListAsync();
            }
            else if (tipo == 2)
            {
                plataformas = await (from p in _context.PLATAFORMA
                                        where p.idPlataforma == int.Parse(dato)
                                        orderby p.idPlataforma
                                        select new PlataformaDTO()
                                        {
                                            idPlataforma = p.idPlataforma,
                                            descripcion = p.descripcion,
                                            idEstado = p.idEstado,
                                            descEstado = (from e in _context.ESTADO where e.idEstado == p.idEstado select e.descripcion).FirstOrDefault(),
                                            numeroMaximoUsuarios = p.numeroMaximoUsuarios,
                                            precio = p.precio
                                        }).ToListAsync();
            }
            else
            {
                plataformas = await (from p in _context.PLATAFORMA
                                        where p.descripcion == dato
                                        orderby p.idPlataforma
                                        select new PlataformaDTO()
                                        {
                                            idPlataforma = p.idPlataforma,
                                            descripcion = p.descripcion,
                                            idEstado = p.idEstado,
                                            descEstado = (from e in _context.ESTADO where e.idEstado == p.idEstado select e.descripcion).FirstOrDefault(),
                                            numeroMaximoUsuarios = p.numeroMaximoUsuarios,
                                            precio = p.precio
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
            List<PlataformaCuentaDTO> plataformaCuentas = await ObtenerPlataformaCuentas(2, id, complemento);
            if (plataformaCuentas.Count == 1) return plataformaCuentas[0];
            else return null;
        }
        public async Task<List<PlataformaCuentaDTO>> GetPlataformaCuentasbyIdPlataforma(string id, bool complemento)
        {
            return (await ObtenerPlataformaCuentas(3, id,complemento));
        }
        public async Task<List<PlataformaCuentaDTO>> GetPlataformaCuentasbyIdCuenta(string id, bool complemento)
        {
            return (await ObtenerPlataformaCuentas(4, id,complemento));
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
                                            orderby pc.idCuenta
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
                                            orderby pc.idCuenta
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
                                            orderby pc.idCuenta
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
                                            orderby pc.idCuenta
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

        public async Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentas(bool complemento)
        {
            return await ObtenerUsuarioPlataformaCuentas(1, null, complemento);
        }
        public async Task<UsuarioPlataformaCuentaDTO> GetUsuarioPlataformaCuentabyIds(string id, bool complemento)
        {
            List<UsuarioPlataformaCuentaDTO> usuarioPlataformaCuentas = await ObtenerUsuarioPlataformaCuentas(2, id, complemento);
            if (usuarioPlataformaCuentas.Count == 1) return usuarioPlataformaCuentas[0];
            else return null;
        }
        public async Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentasbyIdUsuario(string id, bool complemento)
        {
            return (await ObtenerUsuarioPlataformaCuentas(3, id, complemento));
        }
        public async Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentasbyIdPlataforma(string id, bool complemento)
        {
            return (await ObtenerUsuarioPlataformaCuentas(4, id, complemento));
        }
        public async Task<List<UsuarioPlataformaCuentaDTO>> GetUsuarioPlataformaCuentasbyIdCuenta(string id, bool complemento)
        {
            return (await ObtenerUsuarioPlataformaCuentas(5, id, complemento));
        }
        public async Task<bool> UsuarioPlataformaCuentaExists(string idUsuarioPlataformaCuenta)
        {
            int idUsuario = int.Parse(idUsuarioPlataformaCuenta.Split("-")[0]);
            int idPlataforma = int.Parse(idUsuarioPlataformaCuenta.Split("-")[1]);
            int idCuenta = int.Parse(idUsuarioPlataformaCuenta.Split("-")[2]);
            return await _context.USUARIOPLATAFORMACUENTA.AnyAsync(e => e.idUsuario == idUsuario
                                                                && e.idPlataforma == idPlataforma
                                                                && e.idCuenta == idCuenta);
        }
        public async Task<List<UsuarioPlataformaCuentaDTO>> ObtenerUsuarioPlataformaCuentas(int tipo, string dato, bool complemento)
        {
            List<UsuarioPlataformaCuentaDTO> usuarioPlataformaCuentas = new List<UsuarioPlataformaCuentaDTO>();
            string[] array;
            if (tipo == 1)
            {
                usuarioPlataformaCuentas = await (from upc in _context.USUARIOPLATAFORMACUENTA
                                                    orderby upc.idUsuario
                                                    select new UsuarioPlataformaCuentaDTO()
                                                    {
                                                        idUsuarioPlataformaCuenta = upc.idUsuarioPlataformaCuenta,
                                                        idUsuario = upc.idUsuario,
                                                        idPlataforma = upc.idPlataforma,
                                                        idCuenta = upc.idCuenta,
                                                        cantidad = upc.cantidad
                                                    }).ToListAsync();
            }
            else if (tipo == 2)
            {
                array = dato.Split("-");
                usuarioPlataformaCuentas = await (from upc in _context.USUARIOPLATAFORMACUENTA
                                                    where upc.idUsuario == int.Parse(array[0])
                                                    && upc.idPlataforma == int.Parse(array[1])
                                                    && upc.idCuenta == int.Parse(array[2])
                                                    orderby upc.idUsuario
                                                    select new UsuarioPlataformaCuentaDTO()
                                                    {
                                                        idUsuarioPlataformaCuenta = upc.idUsuarioPlataformaCuenta,
                                                        idUsuario = upc.idUsuario,
                                                        idPlataforma = upc.idPlataforma,
                                                        idCuenta = upc.idCuenta,
                                                        cantidad = upc.cantidad
                                                    }).ToListAsync();
            }
            else if (tipo == 3)
            {
                usuarioPlataformaCuentas = await (from upc in _context.USUARIOPLATAFORMACUENTA
                                                    where upc.idUsuario == int.Parse(dato)
                                                    orderby upc.idUsuario
                                                    select new UsuarioPlataformaCuentaDTO()
                                                    {
                                                        idUsuarioPlataformaCuenta = upc.idUsuarioPlataformaCuenta,
                                                        idUsuario = upc.idUsuario,
                                                        idPlataforma = upc.idPlataforma,
                                                        idCuenta = upc.idCuenta,
                                                        cantidad = upc.cantidad
                                                    }).ToListAsync();
            }
            else if (tipo == 4)
            {
                usuarioPlataformaCuentas = await (from upc in _context.USUARIOPLATAFORMACUENTA
                                                    where upc.idPlataforma == int.Parse(dato)
                                                    orderby upc.idPlataforma
                                                    select new UsuarioPlataformaCuentaDTO()
                                                    {
                                                        idUsuarioPlataformaCuenta = upc.idUsuarioPlataformaCuenta,
                                                        idUsuario = upc.idUsuario,
                                                        idPlataforma = upc.idPlataforma,
                                                        idCuenta = upc.idCuenta,
                                                        cantidad = upc.cantidad
                                                    }).ToListAsync();
            }
            else
            {
                usuarioPlataformaCuentas = await (from upc in _context.USUARIOPLATAFORMACUENTA
                                                    where upc.idCuenta == int.Parse(dato)
                                                    orderby upc.idCuenta
                                                    select new UsuarioPlataformaCuentaDTO()
                                                    {
                                                        idUsuarioPlataformaCuenta = upc.idUsuarioPlataformaCuenta,
                                                        idUsuario = upc.idUsuario,
                                                        idPlataforma = upc.idPlataforma,
                                                        idCuenta = upc.idCuenta,
                                                        cantidad = upc.cantidad
                                                    }).ToListAsync();
            }
            if (complemento)
            {
                foreach (var _plataformaCuenta in usuarioPlataformaCuentas)
                {
                    _plataformaCuenta.Cuenta = await GetCuentabyId(_plataformaCuenta.idCuenta, false);
                    _plataformaCuenta.Plataforma = await GetPlataformabyId(_plataformaCuenta.idPlataforma, false);
                    _plataformaCuenta.Usuario = await GetUsuariobyId(_plataformaCuenta.idUsuario, false);
                }
            }
        return usuarioPlataformaCuentas;
        }

        public async Task<List<EstadoDTO>> GetEstados()
        {
            return await ObtenerEstados(1, null);
        }
        public async Task<EstadoDTO> GetEstadobyId(int? id)
        {
            List<EstadoDTO> estados = await ObtenerEstados(2, id.ToString());
            if(estados.Count == 1)return estados[0];
            else return null;
        }
        public async Task<EstadoDTO> GetEstadobyName(string name)
        {
            List<EstadoDTO> estados = await ObtenerEstados(3, name);
            if (estados.Count == 1) return estados[0];
            else return null;
        }
        public async Task<bool> EstadoExists(int id)
        {
            return await _context.ESTADO.AnyAsync(e => e.idEstado == id);
        }
        public async Task<List<EstadoDTO>> ObtenerEstados(int tipo, string dato)
        {
            if (tipo == 1)
            {
                return await (from e in _context.ESTADO
                                select new EstadoDTO()
                                {
                                    idEstado = e.idEstado,
                                    descripcion = e.descripcion
                                }).ToListAsync();
            }
            else if (tipo == 2)
            {
                return await (from e in _context.ESTADO
                                where e.idEstado == int.Parse(dato)
                                orderby e.idEstado
                                select new EstadoDTO()
                                {
                                    idEstado = e.idEstado,
                                    descripcion = e.descripcion
                                }).ToListAsync();
            }
            else
            {
                return await (from e in _context.ESTADO
                                where e.descripcion == dato
                                orderby e.idEstado
                                select new EstadoDTO()
                                {
                                    idEstado = e.idEstado,
                                    descripcion = e.descripcion
                                }).ToListAsync();
            }
        }
            
        public async Task<string> UpdateUsuario(UsuarioDTO usuario)
        {
            string mensaje = string.Empty;
            List<UsuarioPlataformaCuentaDTO> usuarioplataformacuentas;
            try
            {

                usuarioplataformacuentas = await GetUsuarioPlataformaCuentasbyIdUsuario(usuario.idUsuario.ToString(), false);
                /*foreach (var item in usuarioplataformacuentas)
                {
                    mensaje += Environment.NewLine;
                    PlataformaCuentaDTO platformAccount = await GetPlataformaCuentabyIds(item.idPlataforma + "-" + item.idCuenta, false);
                    platformAccount.usuariosdisponibles += item.cantidad;
                    mensaje += await UpdatePlataformaCuenta(platformAccount);

                    mensaje += Environment.NewLine;
                    mensaje += await DeleteUsuarioPlataformaCuenta(item);
                }*/
                UsuarioDTO user = await GetUsuariobyId(usuario.idUsuario, true);
                mensaje += await _commonRepository_U.UpdateObjeto(new Usuario()
                {
                    idUsuario = user.idUsuario,
                    descripcion = usuario.descripcion,
                    idEstado = user.idEstado,
                    fechaInscripcion = user.fechaInscripcion,
                    facturacion = user.facturacion,
                    pago = user.pago
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje += _commonRepository_U.ExceptionMessage(usuario, "U");
            }
            return mensaje;
        }
        public async Task<string> UpdatePlataforma(PlataformaDTO plataforma)
        {
            try
            {
                PlataformaDTO platform = await GetPlataformabyId(plataforma.idPlataforma, false);
                return await _commonRepository_P.UpdateObjeto(new PlataformaDTO()
                {
                    idPlataforma = platform.idPlataforma,
                    descripcion = plataforma.descripcion,
                    idEstado = platform.idEstado,
                    numeroMaximoUsuarios = plataforma.numeroMaximoUsuarios,
                    precio = plataforma.precio
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await _commonRepository_P.ExceptionMessage(plataforma, "U");
            }
        }
        public async Task<string> UpdateCuenta(CuentaDTO cuenta)
        {
            string mensaje = string.Empty;
            List<int> idPlataformasAgregar = new List<int>();
            List<int> idPlataformasEliminar = new List<int>();

            try
            {
                CuentaDTO account = await GetCuentabyId(cuenta.idCuenta, true);
                mensaje += await _commonRepository_C.UpdateObjeto(new Cuenta()
                {
                    idCuenta = account.idCuenta,
                    diminutivo = cuenta.diminutivo,
                    correo = cuenta.correo,
                    idEstado = account.idEstado
                }, _context);

                if (mensaje.Contains("CORRECTA"))
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
                            fechaPago = _commonRepository_PC.SetearFecha(DateTime.Now),
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje += _commonRepository_C.ExceptionMessage(cuenta, "U");
            }
            return mensaje;
        }
        public async Task<string> UpdatePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
        {
            try
            {
                PlataformaCuentaDTO platformAccount = await GetPlataformaCuentabyIds(plataformaCuenta.idPlataformaCuenta,false);
                return await _commonRepository_PC.UpdateObjeto(new PlataformaCuenta()
                {
                    idPlataformaCuenta = platformAccount.idPlataformaCuenta,
                    idCuenta = platformAccount.idCuenta,
                    idPlataforma = platformAccount.idPlataforma,
                    fechaPago = plataformaCuenta.fechaPago == null ? plataformaCuenta.fechaxActualizar : platformAccount.fechaPago,
                    usuariosdisponibles = plataformaCuenta.usuariosdisponibles != platformAccount.usuariosdisponibles ? plataformaCuenta.usuariosdisponibles : platformAccount.usuariosdisponibles,
                    clave = plataformaCuenta.clave != platformAccount.clave ? plataformaCuenta.clave : platformAccount.clave
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await _commonRepository_PC.ExceptionMessage(plataformaCuenta, "U");
            }
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
                return await _commonRepository_E.ExceptionMessage(estado, "U");
            }
        }

        public async Task<string> DeleteUsuario(UsuarioDTO usuario)
        {
            string mensaje = string.Empty;
            List<UsuarioPlataformaCuentaDTO> usuarioplataformacuentas;
            try
            {
                UsuarioDTO user = await GetUsuariobyId(usuario.idUsuario, false);
                mensaje += await _commonRepository_U.DeleteLogicoObjeto(new Usuario()
                {
                    idUsuario = user.idUsuario,
                    descripcion = user.descripcion,
                    idEstado = 2,
                    fechaInscripcion = user.fechaInscripcion,
                    facturacion = user.facturacion,
                    pago = user.pago
                }, _context);
                usuarioplataformacuentas = await GetUsuarioPlataformaCuentasbyIdUsuario(usuario.idUsuario.ToString(),false);
                foreach (var item in usuarioplataformacuentas)
                {
                    mensaje += Environment.NewLine;
                    PlataformaCuentaDTO platformAccount = await GetPlataformaCuentabyIds(item.idPlataforma+"-"+item.idCuenta,false);
                    platformAccount.usuariosdisponibles += item.cantidad;
                    mensaje += await UpdatePlataformaCuenta(platformAccount);

                    mensaje += Environment.NewLine;
                    mensaje += await DeleteUsuarioPlataformaCuenta(item);
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje += _commonRepository_U.ExceptionMessage(usuario, "D");
            }
            return mensaje; 
        }
        public async Task<string> DeletePlataforma(PlataformaDTO plataforma)
        {
            List<UsuarioPlataformaCuentaDTO> usuarioplataformacuentas;
            List<PlataformaCuentaDTO> plataformacuentas;
            string mensaje = string.Empty;
            try
            {
                PlataformaDTO platform = await GetPlataformabyId(plataforma.idPlataforma, false);
                mensaje += await _commonRepository_P.DeleteLogicoObjeto(new Plataforma()
                {
                    idPlataforma = platform.idPlataforma,
                    descripcion = plataforma.descripcion,
                    idEstado = 2,
                    numeroMaximoUsuarios = platform.numeroMaximoUsuarios,
                    precio = platform.precio
                }, _context);
                usuarioplataformacuentas = await GetUsuarioPlataformaCuentasbyIdPlataforma(plataforma.idPlataforma.ToString(), false);
                foreach (var item in usuarioplataformacuentas)
                {
                    mensaje += Environment.NewLine;
                    mensaje += await DeleteUsuarioPlataformaCuenta(item);
                }
                plataformacuentas = await GetPlataformaCuentasbyIdCuenta(plataforma.idPlataforma.ToString(), false);
                foreach (var item in plataformacuentas)
                {
                    mensaje += Environment.NewLine;
                    mensaje += await DeletePlataformaCuenta(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje += _commonRepository_P.ExceptionMessage(plataforma, "D");
            }
            return mensaje;
        }
        public async Task<string> DeleteCuenta(CuentaDTO cuenta)
        {
            List<UsuarioPlataformaCuentaDTO> usuarioplataformacuentas;
            List<PlataformaCuentaDTO> plataformacuentas;
            string mensaje=string.Empty;
            try
            {
                CuentaDTO account = await GetCuentabyId(cuenta.idCuenta, false);
                mensaje += await _commonRepository_C.DeleteLogicoObjeto(new Cuenta()
                {
                    idCuenta = account.idCuenta,
                    diminutivo = account.diminutivo,
                    correo = account.correo,
                    idEstado = 2
                }, _context);
                usuarioplataformacuentas = await GetUsuarioPlataformaCuentasbyIdCuenta(cuenta.idCuenta.ToString(), false);
                foreach (var item in usuarioplataformacuentas)
                {
                    mensaje += Environment.NewLine;
                    mensaje += await DeleteUsuarioPlataformaCuenta(item);
                }
                plataformacuentas = await GetPlataformaCuentasbyIdCuenta(cuenta.idCuenta.ToString(), false);
                foreach (var item in plataformacuentas)
                {
                    mensaje += Environment.NewLine;
                    mensaje += await DeletePlataformaCuenta(item);
                }
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje += _commonRepository_C.ExceptionMessage(cuenta, "D");
            }
            return mensaje;
        }
        public async Task<string> DeletePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
        {
            try
            {
                plataformaCuenta.idPlataformaCuenta = plataformaCuenta.idPlataforma + "-" + plataformaCuenta.idCuenta;
                return await _commonRepository_PC.DeleteObjeto(new PlataformaCuenta()
                {
                    idPlataformaCuenta = plataformaCuenta.idPlataformaCuenta,
                    idPlataforma = plataformaCuenta.idPlataforma,
                    idCuenta = plataformaCuenta.idCuenta,
                    fechaPago = plataformaCuenta.fechaPago,
                    clave = plataformaCuenta.clave,
                    usuariosdisponibles = plataformaCuenta.usuariosdisponibles
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await _commonRepository_PC.ExceptionMessage(plataformaCuenta, "D");
            }
        }
        public async Task<string> DeleteUsuarioPlataformaCuenta(UsuarioPlataformaCuentaDTO usuarioPlataformaCuenta)
        {
            try
            {
                return await _commonRepository_UPC.DeleteObjeto(new UsuarioPlataformaCuenta()
                {
                    idUsuarioPlataformaCuenta = usuarioPlataformaCuenta.idUsuarioPlataformaCuenta,
                    idUsuario = usuarioPlataformaCuenta.idUsuario,
                    idPlataforma = usuarioPlataformaCuenta.idPlataforma,
                    idCuenta = usuarioPlataformaCuenta.idCuenta,
                    cantidad = usuarioPlataformaCuenta.cantidad
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await _commonRepository_UPC.ExceptionMessage(usuarioPlataformaCuenta, "D");
            }
        }
        public async Task<string> DeleteEstado(EstadoDTO estado)
        {
            try
            {
                return await _commonRepository_E.DeleteObjeto(new Estado() 
                {
                    idEstado=estado.idEstado,
                    descripcion=estado.descripcion 
                }, _context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await _commonRepository_E.ExceptionMessage(estado, "D");
            }
        }
            
        public async Task<PlataformaCuentaDTO> GetPlataformaCuentaDisponible(int idPlataforma, int cantidad)
        {
            return await (from pc in _context.PLATAFORMACUENTA
                            join c in _context.CUENTA on pc.idCuenta equals c.idCuenta
                            where pc.idPlataforma == idPlataforma && pc.usuariosdisponibles >= cantidad && c.idEstado != 2
                            select new PlataformaCuentaDTO()
                            {
                                idPlataformaCuenta = pc.idPlataformaCuenta,
                                idCuenta = pc.idCuenta,
                                idPlataforma = pc.idPlataforma,
                                usuariosdisponibles = pc.usuariosdisponibles,
                                fechaPago = pc.fechaPago,
                                clave = pc.clave
                            }).FirstOrDefaultAsync();
        }
        public async Task<int> ObtenerMontoPagoUsuario(List<UsuarioPlataformaCuenta> UsuarioPlataformaCuentas)
        {
            double acumulado = 0;
            for (int i = 0; i < UsuarioPlataformaCuentas.Count; i++)
            {
                if (UsuarioPlataformaCuentas[i].cantidad > 1 && UsuarioPlataformaCuentas[i].cantidad < 4) acumulado += await ObtenerPrecioPlataforma(UsuarioPlataformaCuentas[i].idPlataforma) * UsuarioPlataformaCuentas[i].cantidad * 0.85;
                else acumulado += await ObtenerPrecioPlataforma(UsuarioPlataformaCuentas[i].idPlataforma) * UsuarioPlataformaCuentas[i].cantidad;

            }
            if (UsuarioPlataformaCuentas.Count < 4)
            {
                if (UsuarioPlataformaCuentas.Sum(p => p.cantidad) == UsuarioPlataformaCuentas.Count && UsuarioPlataformaCuentas.Count > 1) { acumulado = _commonRepository_U.reprocesoUsuario(UsuarioPlataformaCuentas.Count, acumulado); }
            }
            return Convert.ToInt16(acumulado);
        }
        public async Task<double> ObtenerPrecioPlataforma(int id)
        {
            return (await (from p in _context.PLATAFORMA
                            where p.idPlataforma == id
                            select p.precio).FirstOrDefaultAsync());
        }
    }
}
