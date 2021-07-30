using Billycock.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billycock.Repositories.Interfaces;
using Billycock.Utils;
using Billycock.DTO;

namespace Billycock.Repositories.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        //private readonly BillycockServiceContext _context;
        //private readonly ICommonRepository<Usuario> _commonRepository;
        //private readonly IPlataformaCuentaRepository _plataformaCuentaRepository;
        //private readonly IPlataformaRepository _plataformaRepository;
        //public UsuarioRepository(BillycockServiceContext context, ICommonRepository<Usuario> commonRepository,
        //    IPlataformaCuentaRepository plataformaCuentaRepository, IPlataformaRepository plataformaRepository)
        //{
        //    _context = context;
        //    _commonRepository= commonRepository;
        //    _plataformaCuentaRepository = plataformaCuentaRepository;
        //    _plataformaRepository = plataformaRepository;
        //}
        //#region Metodos Principales
        //public async Task<string> DeleteUsuario(UsuarioDTO usuario, string tipoSalida)
        //{
        //    Usuario user = await GetUsuariobyId(usuario.idUsuario, tipoSalida);
        //    try
        //    {
        //        return await _commonRepository.DeleteLogicoObjeto(usuario,new Usuario()
        //        {
        //            idUsuario = user.idUsuario,
        //            descripcion = user.descripcion,
        //            idEstado = 2,
        //            fechaInscripcion = user.fechaInscripcion,
        //            facturacion = user.facturacion,
        //            pago = user.pago
        //        },_context);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return _commonRepository.ExceptionMessage(usuario, "D");
        //    }
        //}
        //public async Task<string> InsertUsuario(UsuarioDTO usuario)
        //{
        //    List<UsuarioPlataformaCuenta> plataformasxusuario=new List<UsuarioPlataformaCuenta>();
        //    List<PlataformaCuenta> plataformacuentasTemporal = new List<PlataformaCuenta>();
        //    List<PlataformaCuenta> plataformacuentas = new List<PlataformaCuenta>();
        //    PlataformaCuenta plataformacuenta = new PlataformaCuenta();
        //    List<string> resultadonulo = new List<string>();
        //    try
        //    {
        //        if (usuario.netflix > 0)plataformasxusuario.Add(new UsuarioPlataformaCuenta() {idPlataforma = 1,cantidad = usuario.netflix });
        //        if (usuario.amazon > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 2, cantidad = usuario.amazon });
        //        if (usuario.disney > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 3, cantidad = usuario.disney });
        //        if (usuario.hbo > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 4, cantidad = usuario.hbo });
        //        if (usuario.youtube > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 5, cantidad = usuario.youtube });
        //        if (usuario.spotify > 0) plataformasxusuario.Add(new UsuarioPlataformaCuenta() { idPlataforma = 6, cantidad = usuario.spotify });
        //        foreach (var item in plataformasxusuario)
        //        {
        //            plataformacuentas = new List<PlataformaCuenta>();
        //            plataformacuenta = await _plataformaCuentaRepository.GetPlataformaCuentaDisponible(item.idPlataforma, item.cantidad);
        //            if (plataformacuenta == null)
        //            {
        //                for (int i = 0; i < item.cantidad; i++)
        //                {
        //                    plataformacuenta = await _plataformaCuentaRepository.GetPlataformaCuentaDisponible(item.idPlataforma, 1);
        //                    if (plataformacuenta != null)
        //                    {
        //                        plataformacuentas.Add(plataformacuenta);

        //                        plataformacuenta = await _plataformaCuentaRepository.GetPlataformaCuentabyIds(plataformacuenta.idPlataforma.ToString() + "-"+plataformacuenta.idCuenta.ToString());

        //                        await _plataformaCuentaRepository.UpdatePlataformaCuenta(new PlataformaCuentaDTO()
        //                        {
        //                            idCuenta = plataformacuenta.idCuenta,
        //                            idPlataforma = plataformacuenta.idPlataforma,
        //                            fechaPago = plataformacuenta.fechaPago,
        //                            usuariosdisponibles = plataformacuenta.usuariosdisponibles - 1
        //                        });
        //                    }
        //                }
        //                if (item.cantidad > plataformacuentas.Count)
        //                {
        //                    resultadonulo.Add(item.cantidad + "-" + (_plataformaRepository.GetPlataformabyId(item.idPlataforma)).Result.descripcion);

        //                    foreach (var pfc in plataformacuentas)
        //                    {
        //                        plataformacuenta = await _plataformaCuentaRepository.GetPlataformaCuentabyIds(pfc.idPlataforma.ToString() + "-"+pfc.idCuenta.ToString());

        //                        await _plataformaCuentaRepository.InsertPlataformaCuenta(new PlataformaCuentaDTO()
        //                        {
        //                            idCuenta = plataformacuenta.idCuenta,
        //                            idPlataforma = plataformacuenta.idPlataforma,
        //                            fechaPago = plataformacuenta.fechaPago,
        //                            usuariosdisponibles = plataformacuenta.usuariosdisponibles + 1
        //                        });
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                plataformacuentas.Add(plataformacuenta);

        //                plataformacuenta = await _plataformaCuentaRepository.GetPlataformaCuentabyIds(plataformacuenta.idPlataforma.ToString() + "-"+plataformacuenta.idCuenta.ToString());

        //                await _plataformaCuentaRepository.InsertPlataformaCuenta(new PlataformaCuentaDTO()
        //                {
        //                    idCuenta = plataformacuenta.idCuenta,
        //                    idPlataforma = plataformacuenta.idPlataforma,
        //                    fechaPago = plataformacuenta.fechaPago,
        //                    usuariosdisponibles = plataformacuenta.usuariosdisponibles - item.cantidad
        //                });
        //            }
        //        }
        //        if (resultadonulo.Count != 0)
        //        {
        //            string mensaje = "NO HAY SUFICIENTES USUARIOS DISPONIBLES: " + Environment.NewLine;
        //            for (int i = 0; i < resultadonulo.Count; i++)
        //            {
        //                mensaje += resultadonulo[i];
        //                if (i < resultadonulo.Count - 1) mensaje += Environment.NewLine;
        //            }
        //            return mensaje;
        //        }
        //        return await _commonRepository.InsertObjeto(usuario,new Usuario()
        //        {
        //            descripcion = usuario.descripcion,
        //            fechaInscripcion = DateTime.Now,
        //            idEstado = 1,
        //            facturacion = ObtenerFechaFacturacion(),
        //            pago = await ObtenerMontoPagoAsync(plataformasxusuario)
        //        },_context);
        //        //plataformacuentasTemporal = plataformacuentas.GroupBy(x => x.idPlataformaCuenta)
        //        //                    .Select(group => group.First()).ToList();

        //        //foreach (var plataformasxusuario in usuario.plataformasxusuario)
        //        //{
        //        //    for (int i = 0; i < plataformacuentasTemporal.Count; i++)
        //        //    {
        //        //        user = await GetUsuariobyName(usuario.descripcion);
        //        //        await _context.UsuarioPlataformaCuenta.AddAsync(new UsuarioPlataformaCuenta()
        //        //        {
        //        //            idUsuario = user.idUsuario,
        //        //            idPlataforma = plataformasxusuario.idPlataforma,
        //        //            cantidad = plataformasxusuario.cantidad,
        //        //            idCuenta = plataformacuentasTemporal[i].idCuenta
        //        //        });
        //        //        await Save();
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return _commonRepository.ExceptionMessage(usuario, "C");
        //    }
        //}
        //public async Task<string> UpdateUsuario(UsuarioDTO usuario, string tipoSalida)
        //{
        //    Usuario user = await GetUsuariobyId(usuario.idUsuario, tipoSalida);
        //    try
        //    {
        //        return await _commonRepository.UpdateObjeto(usuario,new Usuario()
        //        {
        //            idUsuario = user.idUsuario,
        //            descripcion = usuario.descripcion,
        //            idEstado = usuario.idEstado,
        //            fechaInscripcion = user.fechaInscripcion,
        //            facturacion = usuario.facturacion,
        //            pago = usuario.pago
        //        },_context);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return _commonRepository.ExceptionMessage(usuario,"U");
        //    }
        //}
        //public async Task<List<UsuarioDTO>> GetUsuarios(string tipoSalida)
        //{
        //    return await ObtenerUsuarios(1, "", tipoSalida);
        //}
        //public async Task<UsuarioDTO> GetUsuariobyId(int? id,string tipoSalida)
        //{
        //    return (await ObtenerUsuarios(2, id.ToString(), tipoSalida))[0];
        //}
        //public async Task<UsuarioDTO> GetUsuariobyName(string name, string tipoSalida)
        //{
        //    return (await ObtenerUsuarios(3, name, tipoSalida))[0];
        //}
        //public async Task<bool> UsuarioExists(int id)
        //{
        //    return await _context.USUARIO.AnyAsync(e => e.idUsuario == id);
        //}
        //public async Task<List<UsuarioDTO>> ObtenerUsuarios(int tipo, string dato,string tipoSalida)
        //{
        //    List<UsuarioDTO> usuarios = new List<UsuarioDTO>();
        //    try
        //    {
        //        if (tipo == 1)
        //        {
        //            if(tipoSalida == "WEB")
        //            {
        //                usuarios = await (from u in _context.USUARIO
        //                                  select new UsuarioDTO()
        //                                  {
        //                                      idUsuario = u.idUsuario,
        //                                      descripcion = u.descripcion,
        //                                      fechaInscripcion = u.fechaInscripcion,
        //                                      idEstado = u.idEstado,
        //                                      descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
        //                                      facturacion = u.facturacion,
        //                                      //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
        //                                      //                            where up.idUsuario == u.idUsuario
        //                                      //                      orderby up.idUsuario
        //                                      //                      select new UsuarioPlataformaCuenta()
        //                                      //                      {
        //                                      //                          idUsuario = up.idUsuario,
        //                                      //                          idPlataforma = up.idPlataforma,
        //                                      //                          descPlataforma = (from p in _context.PLATAFORMA
        //                                      //                                            where p.idPlataforma == up.idPlataforma
        //                                      //                                            select p.descripcion).FirstOrDefault(),
        //                                      //                          cantidad = up.cantidad,
        //                                      //                          idCuenta = up.idCuenta,
        //                                      //                          credencial = (from c in _context.CUENTA
        //                                      //                                        where c.idCuenta == up.idCuenta
        //                                      //                                        select new UsuarioPlataformaCuenta.Credencial()
        //                                      //                                        {
        //                                      //                                            usuario = c.descripcion,
        //                                      //                                            clave = c.password
        //                                      //                                        }).FirstOrDefault()
        //                                      //                      }).ToList(),
        //                                      pago = u.pago
        //                                  }).ToListAsync();
        //            }
        //            else
        //            {
        //                usuarios = await (from u in _context.USUARIO
        //                              where u.idEstado != 2
        //                              select new UsuarioDTO()
        //                              {
        //                                  idUsuario = u.idUsuario,
        //                                  descripcion = u.descripcion,
        //                                  fechaInscripcion = u.fechaInscripcion,
        //                                  idEstado = u.idEstado,
        //                                  descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
        //                                  facturacion = u.facturacion,
        //                                  //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
        //                                  //                       where up.idUsuario == u.idUsuario
        //                                  //                       orderby up.idUsuario
        //                                  //                       select new UsuarioPlataformaCuenta()
        //                                  //                       {
        //                                  //                           idUsuario = up.idUsuario,
        //                                  //                           idPlataforma = up.idPlataforma,
        //                                  //                           descPlataforma = (from p in _context.PLATAFORMA
        //                                  //                                             where p.idPlataforma == up.idPlataforma
        //                                  //                                             select p.descripcion).FirstOrDefault(),
        //                                  //                           cantidad = up.cantidad,
        //                                  //                           idCuenta = up.idCuenta,
        //                                  //                           credencial = (from c in _context.CUENTA
        //                                  //                                         where c.idCuenta == up.idCuenta
        //                                  //                                         select new UsuarioPlataformaCuenta.Credencial()
        //                                  //                                         {
        //                                  //                                             usuario = c.descripcion,
        //                                  //                                             clave = c.password
        //                                  //                                         }).FirstOrDefault()
        //                                  //                       }).ToList(),
        //                                  pago = u.pago
        //                              }).ToListAsync();
        //            }
        //        }
        //        else if (tipo == 2)
        //        {
        //            if (tipoSalida == "WEB")
        //            {
        //                usuarios = await (from u in _context.USUARIO
        //                                  where u.idUsuario == int.Parse(dato)
        //                                  select new UsuarioDTO()
        //                                  {
        //                                      idUsuario = u.idUsuario,
        //                                      descripcion = u.descripcion,
        //                                      fechaInscripcion = u.fechaInscripcion,
        //                                      idEstado = u.idEstado,
        //                                      descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
        //                                      facturacion = u.facturacion,
        //                                      //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
        //                                      //                      where up.idUsuario == u.idUsuario
        //                                      //                      orderby up.idUsuario
        //                                      //                      select new UsuarioPlataformaCuenta()
        //                                      //                      {
        //                                      //                          idUsuario = up.idUsuario,
        //                                      //                          idPlataforma = up.idPlataforma,
        //                                      //                          descPlataforma = (from p in _context.PLATAFORMA
        //                                      //                                            where p.idPlataforma == up.idPlataforma
        //                                      //                                            select p.descripcion).FirstOrDefault(),
        //                                      //                          cantidad = up.cantidad,
        //                                      //                          idCuenta = up.idCuenta,
        //                                      //                          credencial = (from c in _context.CUENTA
        //                                      //                                        where c.idCuenta == up.idCuenta
        //                                      //                                        select new UsuarioPlataformaCuenta.Credencial()
        //                                      //                                        {
        //                                      //                                            usuario = c.descripcion,
        //                                      //                                            clave = c.password
        //                                      //                                        }).FirstOrDefault()
        //                                      //                      }).ToList(),
        //                                      pago = u.pago
        //                                  }).ToListAsync();
        //            }
        //            else
        //            {
        //                usuarios = await (from u in _context.USUARIO
        //                          where u.idEstado != 2 && u.idUsuario == int.Parse(dato)
        //                          select new UsuarioDTO()
        //                          {
        //                              idUsuario = u.idUsuario,
        //                              descripcion = u.descripcion,
        //                              fechaInscripcion = u.fechaInscripcion,
        //                              idEstado = u.idEstado,
        //                              descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
        //                              facturacion = u.facturacion,
        //                              //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
        //                              //                       where up.idUsuario == u.idUsuario
        //                              //                       orderby up.idUsuario
        //                              //                       select new UsuarioPlataformaCuenta()
        //                              //                       {
        //                              //                           idUsuario = up.idUsuario,
        //                              //                           idPlataforma = up.idPlataforma,
        //                              //                           descPlataforma = (from p in _context.PLATAFORMA
        //                              //                                             where p.idPlataforma == up.idPlataforma
        //                              //                                             select p.descripcion).FirstOrDefault(),
        //                              //                           cantidad = up.cantidad,
        //                              //                           idCuenta = up.idCuenta,
        //                              //                           credencial = (from c in _context.CUENTA
        //                              //                                         where c.idCuenta == up.idCuenta
        //                              //                                         select new UsuarioPlataformaCuenta.Credencial()
        //                              //                                         {
        //                              //                                             usuario = c.descripcion,
        //                              //                                             clave = c.password
        //                              //                                         }).FirstOrDefault()
        //                              //                       }).ToList(),
        //                              pago = u.pago
        //                          }).ToListAsync();
        //            }
        //        }
        //        else
        //        {
        //            if (tipoSalida == "WEB")
        //            {
        //                usuarios = await (from u in _context.USUARIO
        //                                  where u.descripcion == dato
        //                                  select new UsuarioDTO()
        //                                  {
        //                                      idUsuario = u.idUsuario,
        //                                      descripcion = u.descripcion,
        //                                      fechaInscripcion = u.fechaInscripcion,
        //                                      idEstado = u.idEstado,
        //                                      descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
        //                                      facturacion = u.facturacion,
        //                                      //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
        //                                      //                      where up.idUsuario == u.idUsuario
        //                                      //                      orderby up.idUsuario
        //                                      //                      select new UsuarioPlataformaCuenta()
        //                                      //                      {
        //                                      //                          idUsuario = up.idUsuario,
        //                                      //                          idPlataforma = up.idPlataforma,
        //                                      //                          descPlataforma = (from p in _context.PLATAFORMA
        //                                      //                                            where p.idPlataforma == up.idPlataforma
        //                                      //                                            select p.descripcion).FirstOrDefault(),
        //                                      //                          cantidad = up.cantidad,
        //                                      //                          idCuenta = up.idCuenta,
        //                                      //                          credencial = (from c in _context.CUENTA
        //                                      //                                        where c.idCuenta == up.idCuenta
        //                                      //                                        select new UsuarioPlataformaCuenta.Credencial()
        //                                      //                                        {
        //                                      //                                            usuario = c.descripcion,
        //                                      //                                            clave = c.password
        //                                      //                                        }).FirstOrDefault()
        //                                      //                      }).ToList(),
        //                                      pago = u.pago
        //                                  }).ToListAsync();
        //            }
        //            else
        //            {
        //                usuarios = await (from u in _context.USUARIO
        //                          where u.idEstado != 2 && u.descripcion == dato
        //                          select new UsuarioDTO()
        //                          {
        //                              idUsuario = u.idUsuario,
        //                              descripcion = u.descripcion,
        //                              fechaInscripcion = u.fechaInscripcion,
        //                              idEstado = u.idEstado,
        //                              descEstado = (from e in _context.ESTADO where e.idEstado == u.idEstado select e.descripcion).FirstOrDefault(),
        //                              facturacion = u.facturacion,
        //                              //usuarioPlataformacuentas = (from up in _context.USUARIOPLATAFORMACUENTA
        //                              //                       where up.idUsuario == u.idUsuario
        //                              //                       orderby up.idUsuario
        //                              //                       select new UsuarioPlataformaCuenta()
        //                              //                       {
        //                              //                           idUsuario = up.idUsuario,
        //                              //                           idPlataforma = up.idPlataforma,
        //                              //                           descPlataforma = (from p in _context.PLATAFORMA
        //                              //                                             where p.idPlataforma == up.idPlataforma
        //                              //                                             select p.descripcion).FirstOrDefault(),
        //                              //                           cantidad = up.cantidad,
        //                              //                           idCuenta = up.idCuenta,
        //                              //                           credencial = (from c in _context.CUENTA
        //                              //                                         where c.idCuenta == up.idCuenta
        //                              //                                         select new UsuarioPlataformaCuenta.Credencial()
        //                              //                                         {
        //                              //                                             usuario = c.descripcion,
        //                              //                                             clave = c.password
        //                              //                                         }).FirstOrDefault()
        //                              //                       }).ToList(),
        //                              pago = u.pago
        //                          }).ToListAsync();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //    return usuarios;
        //}
        //#endregion
        //#region Metodos secundarios
        //private string ObtenerFechaFacturacion()
        //{
        //    DateTime fechaHoy = DateTime.Now;
        //    bool QuincenaMes = fechaHoy.Day <= 15 ? true : false;
        //    DateTime oPrimerDiaDelMes = new DateTime(fechaHoy.Year, fechaHoy.Month, 1);
        //    if (fechaHoy.Month < 12)
        //    {
        //        if (QuincenaMes)
        //        {
        //            return new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1).ToShortDateString();
        //        }
        //        else
        //        {
        //            return oPrimerDiaDelMes.AddMonths(2).AddDays(-1).ToShortDateString();
        //        }
        //    }
        //    else
        //    {
        //        if (QuincenaMes)
        //        {
        //            return new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1).ToShortDateString();
        //        }
        //        else
        //        {
        //            return oPrimerDiaDelMes.AddMonths(2).AddDays(-1).ToShortDateString();
        //        }
        //    }
        //}
        //private async Task<int?> ObtenerMontoPagoAsync(List<UsuarioPlataformaCuenta> UsuarioPlataformaCuentas)
        //{
        //    int? pago = 0;
        //    double? acumulado = 0;
        //    for (int i = 0; i < UsuarioPlataformaCuentas.Count; i++)
        //    {
        //        acumulado += await _plataformaRepository.GetPricePlataforma(UsuarioPlataformaCuentas[i].idPlataforma) * UsuarioPlataformaCuentas[i].cantidad;

        //        if (i == UsuarioPlataformaCuentas.Count - 1)
        //        {
        //            if (UsuarioPlataformaCuentas[i].cantidad == 1 && UsuarioPlataformaCuentas.Count > 1) { pago = reproceso(1, UsuarioPlataformaCuentas.Count, acumulado); }
        //            else if (UsuarioPlataformaCuentas[i].cantidad > 1 && UsuarioPlataformaCuentas.Count == 1) { pago = reproceso(2, UsuarioPlataformaCuentas[i].cantidad, acumulado); }
        //            else pago = Convert.ToInt16(acumulado);
        //        }
        //    }
        //    return pago;
        //}
        //private int reproceso(int tipo, int? cuenta, double? monto)
        //{
        //    if (tipo == 1)
        //    {
        //        return (int)((monto / cuenta) * (cuenta * 0.9));
        //    }
        //    else
        //    {
        //        return (int)(monto * 0.85);
        //    }
        //}
        //#endregion
    }
}