using Billycock.Data;
using Billycock.Models;
using Billycock.Repositories.Interfaces;
using Billycock.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Billycock.Repositories.Repositories
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        public async Task Save(BillycockServiceContext _context)
        {
            await _context.SaveChangesAsync();
        }
        public async Task<string> DeleteLogicoObjeto(T tracker,T t, BillycockServiceContext _context)
        {
            string mensaje = "Eliminacion XXX de " + t.GetType().Name;
            try
            {
                _context.Entry(tracker).State = EntityState.Detached; 
                _context.Update(t);
                await Save(_context);
                mensaje = mensaje.Replace("XXX","Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje,_context);
            return mensaje;
        }
        public async Task<string> DeleteObjeto(T tracker, T t, BillycockServiceContext _context)
        {
            string mensaje = "Eliminacion XXX de " + t.GetType().Name;
            try
            {
                _context.Entry(tracker).State = EntityState.Detached;
                _context.Remove(t);
                await Save(_context);
                mensaje = mensaje.Replace("XXX", "Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje, _context);
            return mensaje;
        }
        public async Task<string> InsertObjeto(T tracker, T t, BillycockServiceContext _context)
        {
            string mensaje = "Creacion XXX de " + t.GetType().Name;
            try
            {
                _context.Entry(tracker).State = EntityState.Detached;
                await _context.AddAsync(t);

                await Save(_context);

                mensaje = mensaje.Replace("XXX", "Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje, _context);
            return mensaje;
        }
        public async Task<string> UpdateObjeto(T tracker, T t, BillycockServiceContext _context)
        {
            string mensaje = ("Actualizacion XXX de " + t.GetType().Name).ToUpper();
            try
            {
                _context.Entry(tracker).State = EntityState.Detached;
                _context.Update(t);
                await Save(_context);

                mensaje = mensaje.Replace("XXX", "Correcta").ToUpper();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = mensaje.Replace("XXX", "Incorrecta").ToUpper();
            }
            await InsertHistory(t, mensaje, _context);
            return mensaje;
        }
        public string ExceptionMessage(T t, string MessageType)
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
            return message;
        }
        public async Task InsertHistory(T t, string response, BillycockServiceContext _context)
        {
            try
            {
                await _context.AddAsync(new Historia()
                {
                    Request = JsonConvert.SerializeObject(t),
                    Response = response,
                    fecha = DateTime.Now
                });
                await Save(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}