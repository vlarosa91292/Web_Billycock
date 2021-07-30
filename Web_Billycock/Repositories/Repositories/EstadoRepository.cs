using Billycock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billycock.Repositories.Interfaces;
using Billycock.Utils;
using Web_Billycock.Data;
using Web_Billycock.Repositories.Interfaces;

namespace Billycock.Repositories.Repositories
{
    public class EstadoRepository:IEstadoRepository
    {
        private readonly IDBSqlRepository _context;
        private readonly ICommonRepository<Estado> _commonRepository;
        public EstadoRepository(IDBSqlRepository context, ICommonRepository<Estado> commonRepository)
        {
            _context = context;
            _commonRepository = commonRepository;
        }
        #region Metodos Principales
        public async Task<bool> DeleteEstado(Estado estado)
        {
            //Estado state = await GetEstadobyId(estado.idEstado);
            try
            {
                return await _context.EjecutaSP("B","",false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        //public async Task<string> InsertEstado(Estado estado)
        //{
        //    try
        //    {
        //        return await _commonRepository.InsertObjeto(estado,new Estado()
        //        {
        //            descripcion = estado.descripcion
        //        },_context);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return _commonRepository.ExceptionMessage(estado, "C");
        //    }
        //}
        //public async Task<string> UpdateEstado(Estado estado)
        //{
        //    Estado account = await GetEstadobyId(estado.idEstado);
        //    try
        //    {
        //        return await _commonRepository.UpdateObjeto(estado,new Estado()
        //        {
        //            idEstado = account.idEstado,
        //            descripcion = estado.descripcion
        //        },_context);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return _commonRepository.ExceptionMessage(estado, "U");
        //    }
        //}
        //public async Task<List<Estado>> GetEstados()
        //{
        //    return await ObtenerEstados(1, "");
        //}
        //public async Task<Estado> GetEstadobyId(int? id)
        //{
        //    return (await ObtenerEstados(2, id.ToString()))[0];
        //}
        //public async Task<Estado> GetEstadobyName(string name)
        //{
        //    return (await ObtenerEstados(3, name))[0];
        //}
        //public async Task<bool> EstadoExists(int id)
        //{
        //    return await _context.ESTADO.AnyAsync(e => e.idEstado == id);
        //}
        //public async Task<List<Estado>> ObtenerEstados(int tipo, string dato)
        //{
        //    if (tipo == 1)
        //    {
        //        return await (from c in _context.ESTADO
        //                      select new Estado()
        //                      {
        //                          idEstado = c.idEstado,
        //                          descripcion = c.descripcion
        //                      }).ToListAsync();
        //    }
        //    else if (tipo == 2)
        //    {
        //        return await (from c in _context.ESTADO
        //                      where c.idEstado == int.Parse(dato)
        //                      select new Estado()
        //                      {
        //                          idEstado = c.idEstado,
        //                          descripcion = c.descripcion
        //                      }).ToListAsync();
        //    }
        //    else
        //    {
        //        return await (from c in _context.ESTADO
        //                      where c.descripcion == dato
        //                      select new Estado()
        //                      {
        //                          idEstado = c.idEstado,
        //                          descripcion = c.descripcion
        //                      }).ToListAsync();
        //    }
        //}
        #endregion
        //#region Metodos Secundarios
        //#endregion
    }
}
