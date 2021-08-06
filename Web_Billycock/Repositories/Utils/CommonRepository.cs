using Billycock.Data;
using Billycock.Models;
using Billycock.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Repositories.Repositories
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        private readonly BillycockServiceContext _context;
        public CommonRepository(BillycockServiceContext context)
        {
            _context = context;
        }
        public async Task Save(BillycockServiceContext _context)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> DeleteLogicoObjeto(T t, BillycockServiceContext _context)
        {
            string mensaje = "Eliminacion XXX de " + t.GetType().Name;
            try
            {
                _context.Entry(t).State = EntityState.Modified;
                await Save(_context);
                _context.Entry(t).State = EntityState.Detached;
                mensaje = mensaje.Replace("XXX","Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje);
            return mensaje;
        }
        public async Task<string> DeleteObjeto(T t, BillycockServiceContext _context)
        {
            string mensaje = "Eliminacion XXX de " + t.GetType().Name;
            try
            {
                _context.Entry(t).State = EntityState.Deleted;
                await Save(_context);
                _context.Entry(t).State = EntityState.Detached;
                mensaje = mensaje.Replace("XXX", "Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje);
            return mensaje;
        }
        public async Task<string> InsertObjeto(T t, BillycockServiceContext _context)
        {
            string mensaje = "Creacion XXX de " + t.GetType().Name;
            try
            {
                _context.Entry(t).State = EntityState.Added;
                await Save(_context);
                _context.Entry(t).State = EntityState.Detached;
                mensaje = mensaje.Replace("XXX", "Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje);
            return mensaje;
        }
        public async Task<string> UpdateObjeto(T t, BillycockServiceContext _context)
        {
            string mensaje = ("Actualizacion XXX de " + t.GetType().Name).ToUpper();
            try
            {
                _context.Entry(t).State = EntityState.Modified;
                await Save(_context);
                _context.Entry(t).State = EntityState.Detached;
                mensaje = mensaje.Replace("XXX", "Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje);
            return mensaje;
        }
        public async Task<string> ExceptionMessage(T t, string MessageType)
        {
            string message = "ERROR EN @PROCESO@ DE "+ t.GetType().Name.ToUpper() + "-SERVER";
            if (MessageType == "C")
            {
                message = message.Replace("@PROCESO@", "CREACION");
            }
            else if (MessageType == "U")
            {
                message = message.Replace("@PROCESO@", "ACTUALIZACION");
            }
            else if (MessageType == "D")
            {
                message = message.Replace("@PROCESO@", "ELIMINACION");
            }
            await InsertHistory(t, message);
            return message;
        }
        public async Task InsertHistory(T t, string response)
        {
            try
            {
                _context.Entry(new Historia()
                {
                    Request = JsonConvert.SerializeObject(t),
                    Response = response,
                    fecha = SetearFechaTiempo()
                }).State = EntityState.Added;
                await Save(_context);
                _context.Entry(t).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public string ObtenerFechaFacturacionUsuario()
        {
            DateTime fechaHoy = DateTime.Now;
            bool QuincenaMes = fechaHoy.Day <= 15 ? true : false;
            DateTime oPrimerDiaDelMes = new DateTime(fechaHoy.Year, fechaHoy.Month, 1);

            if (fechaHoy.Month < 12)
            {
                if (QuincenaMes)
                {
                    return SetearFecha(new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1));
                }
                else
                {
                    return SetearFecha(oPrimerDiaDelMes.AddMonths(2).AddDays(-1));
                }
            }
            else
            {
                if (QuincenaMes)
                {
                    return SetearFecha(new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1));
                }
                else
                {
                    return SetearFecha(oPrimerDiaDelMes.AddMonths(2).AddDays(-1));
                }
            }
        }        
        public int reprocesoUsuario(int cuenta, double monto)
        {
            return (int)((monto / cuenta) * (cuenta * 0.9));
        }
        public string SetearFecha(DateTime fecha)
        {
            return fecha.Day.ToString() + "/" + fecha.Month.ToString() + "/" + fecha.Year.ToString();
        }
        public string SetearFechaTiempo()
        {
            return DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
        }
    }
}