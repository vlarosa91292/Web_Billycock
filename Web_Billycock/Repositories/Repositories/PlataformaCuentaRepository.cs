using Billycock.Data;
using Billycock.DTO;
using Billycock.Models;
using Billycock.Repositories.Interfaces;
using Billycock.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Repositories.Repositories
{
    public class PlataformaCuentaRepository : IPlataformaCuentaRepository
    {
        private readonly BillycockServiceContext _context;
        private readonly ICommonRepository<PlataformaCuentaDTO> _commonRepository;
        public PlataformaCuentaRepository(BillycockServiceContext context, ICommonRepository<PlataformaCuentaDTO> commonRepository)
        {
            _context = context;
            _commonRepository = commonRepository;
        }
        public async Task<string> DeletePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
        {
            return await _commonRepository.DeleteObjeto(plataformaCuenta, plataformaCuenta, _context);
        }

        public async Task<PlataformaCuentaDTO> GetPlataformaCuentabyIds(string id)
        {
            return (await ObtenerPlataformaCuentas(2, id))[0];
        }
        public async Task<PlataformaCuentaDTO> GetPlataformaCuentabyIdPlataforma(string id)
        {
            return (await ObtenerPlataformaCuentas(3, id))[0];
        }
        public async Task<PlataformaCuentaDTO> GetPlataformaCuentabyIdCuenta(string id)
        {
            return (await ObtenerPlataformaCuentas(4, id))[0];
        }

        //public async Task<PlataformaCuentaDTO> GetPlataformaCuentabyName(string name)
        //{
        //    return (await ObtenerPlataformaCuentas(3, name))[0];
        //}

        public async Task<List<PlataformaCuentaDTO>> GetPlataformaCuentas()
        {
            return await ObtenerPlataformaCuentas(1, null);
        }

        public async Task<List<PlataformaCuentaDTO>> ObtenerPlataformaCuentas(int tipo, string dato)
        {
            string[] array;
            if (tipo == 1)
            {
                return await (from pc in _context.PLATAFORMACUENTA
                              select new PlataformaCuentaDTO()
                              {
                                  idPlataformaCuenta = pc.idPlataforma + "-" + pc.idCuenta,
                                  idPlataforma = pc.idPlataforma,
                                  idCuenta = pc.idCuenta,
                                  clave = pc.clave,
                                  fechaPago = pc.fechaPago,
                                  Cuenta = (from c in _context.CUENTA where c.idCuenta == pc.idCuenta select c).FirstOrDefault(),
                                  Plataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p).FirstOrDefault()
                                  //plataformaCuentas = (from pc in _context.PLATAFORMACUENTA
                                  //                     where pc.idPlataforma == c.idPlataforma
                                  //                     select new PlataformaCuentaDTO()
                                  //                     {
                                  //                         idCuenta = pc.idPlataforma,
                                  //                         descCuenta = c.descripcion,
                                  //                         idPlataforma = pc.idPlataforma,
                                  //                         descPlataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p.descripcion).FirstOrDefault(),
                                  //                         fechaPago = pc.fechaPago,
                                  //                         usuariosdisponibles = pc.usuariosdisponibles
                                  //                     }).ToList()
                              }).ToListAsync();
            }
            else if (tipo == 2)
            {
                array = dato.Split("-");
                return await (from pc in _context.PLATAFORMACUENTA
                              where pc.idPlataforma == int.Parse(array[0]) && pc.idCuenta == int.Parse(array[1])
                              select new PlataformaCuentaDTO()
                              {
                                  idPlataformaCuenta = pc.idPlataforma + "-" + pc.idCuenta,
                                  idPlataforma = pc.idPlataforma,
                                  idCuenta = pc.idCuenta,
                                  clave = pc.clave,
                                  fechaPago = pc.fechaPago,
                                  Cuenta = (from c in _context.CUENTA where c.idCuenta == pc.idCuenta select c).FirstOrDefault(),
                                  Plataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p).FirstOrDefault()
                                  //plataformaCuentas = (from pc in _context.PLATAFORMACUENTA
                                  //                     where pc.idPlataforma == c.idPlataforma
                                  //                     select new PlataformaCuentaDTO()
                                  //                     {
                                  //                         idCuenta = pc.idPlataforma,
                                  //                         descCuenta = c.descripcion,
                                  //                         idPlataforma = pc.idPlataforma,
                                  //                         descPlataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p.descripcion).FirstOrDefault(),
                                  //                         fechaPago = pc.fechaPago,
                                  //                         usuariosdisponibles = pc.usuariosdisponibles
                                  //                     }).ToList()
                              }).ToListAsync();
            }
            else if (tipo == 3)
            {
                return await (from pc in _context.PLATAFORMACUENTA
                              where pc.idPlataforma == int.Parse(dato)
                              select new PlataformaCuentaDTO()
                              {
                                  idPlataformaCuenta = pc.idPlataforma + "-" + pc.idCuenta,
                                  idPlataforma = pc.idPlataforma,
                                  idCuenta = pc.idCuenta,
                                  clave = pc.clave,
                                  fechaPago = pc.fechaPago,
                                  Cuenta = (from c in _context.CUENTA where c.idCuenta == pc.idCuenta select c).FirstOrDefault(),
                                  Plataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p).FirstOrDefault()
                                  //plataformaCuentas = (from pc in _context.PLATAFORMACUENTA
                                  //                     where pc.idPlataforma == c.idPlataforma
                                  //                     select new PlataformaCuentaDTO()
                                  //                     {
                                  //                         idCuenta = pc.idPlataforma,
                                  //                         descCuenta = c.descripcion,
                                  //                         idPlataforma = pc.idPlataforma,
                                  //                         descPlataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p.descripcion).FirstOrDefault(),
                                  //                         fechaPago = pc.fechaPago,
                                  //                         usuariosdisponibles = pc.usuariosdisponibles
                                  //                     }).ToList()
                              }).ToListAsync();
            }
            else
            {
                return await (from pc in _context.PLATAFORMACUENTA
                              where pc.idCuenta == int.Parse(dato)
                              select new PlataformaCuentaDTO()
                              {
                                  idPlataformaCuenta = pc.idPlataforma + "-" + pc.idCuenta,
                                  idPlataforma = pc.idPlataforma,
                                  idCuenta = pc.idCuenta,
                                  clave = pc.clave,
                                  fechaPago = pc.fechaPago,
                                  Cuenta = (from c in _context.CUENTA where c.idCuenta == pc.idCuenta select c).FirstOrDefault(),
                                  Plataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p).FirstOrDefault()
                                  //plataformaCuentas = (from pc in _context.PLATAFORMACUENTA
                                  //                     where pc.idPlataforma == c.idPlataforma
                                  //                     select new PlataformaCuentaDTO()
                                  //                     {
                                  //                         idCuenta = pc.idPlataforma,
                                  //                         descCuenta = c.descripcion,
                                  //                         idPlataforma = pc.idPlataforma,
                                  //                         descPlataforma = (from p in _context.PLATAFORMA where p.idPlataforma == pc.idPlataforma select p.descripcion).FirstOrDefault(),
                                  //                         fechaPago = pc.fechaPago,
                                  //                         usuariosdisponibles = pc.usuariosdisponibles
                                  //                     }).ToList()
                              }).ToListAsync();
            }
        }

        public async Task<string> InsertPlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
        {
            return await _commonRepository.InsertObjeto(plataformaCuenta, plataformaCuenta, _context);
        }

        public async Task<bool> PlataformaCuentaExists(string idPlataformaCuenta)
        {
            int idPlataforma = int.Parse(idPlataformaCuenta.Split("-")[0]);
            int idCuenta = int.Parse(idPlataformaCuenta.Split("-")[1]);
            return await _context.PLATAFORMACUENTA.AnyAsync(e => e.idPlataforma == idPlataforma
                                                                && e.idCuenta == idCuenta);
        }

        public async Task<string> UpdatePlataformaCuenta(PlataformaCuentaDTO plataformaCuenta)
        {
            return await _commonRepository.UpdateObjeto(plataformaCuenta, plataformaCuenta, _context);
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
    }
}
