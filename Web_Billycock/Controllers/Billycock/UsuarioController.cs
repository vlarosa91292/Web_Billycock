using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Billycock.Models;
using Web_Billycock.Repositories.Interfaces;
using Billycock.DTO;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Web_Billycock.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IBillycock_WebRepository<Usuario> _context;
        private readonly INotyfService _notyfService;

        public UsuarioController(IBillycock_WebRepository<Usuario> context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            List<UsuarioDTO> usuarios = await _context.GetUsuarios(false);
            if (usuarios.Any())
            {
                if(usuarios.Count < 2)_notyfService.Information("Se listó "+ usuarios.Count + " usuario", 4);
                else _notyfService.Information("Se listaron " + usuarios.Count + " usuarios", 4);
            }
            else _notyfService.Information("No se encontraron usuarios para listar", 4);
            return View(await _context.GetUsuarios(false));
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _notyfService.Information("No se envia el id",4);
                return View();
            }

            var usuario = await _context.GetUsuariobyId(id, true);
            if (usuario == null)
            {
                _notyfService.Error("El usuario no existe", 4);
                return RedirectToAction("Index");
            }

            _notyfService.Success("Usuario encontrado", 4);
            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("descripcion,netflix,amazon,disney,hbo,youtube,spotify")] UsuarioDTO usuario)
        {
            if (ModelState.IsValid)
            {
                string mensaje = await _context.InsertUsuario(usuario);
                _notyfService.Success(mensaje.Replace("\r\n", "</br>"), 4);
                if (mensaje.Contains("CORRECTA")) return RedirectToAction("Index");
                else return View(usuario);
            }
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _notyfService.Information("No se envia el id", 4);
                return View();
            }

            var usuario = await _context.GetUsuariobyId(id, true);
            if (usuario == null)
            {
                _notyfService.Error("El usuario no existe", 4);
                return RedirectToAction("Index");
            }
            _notyfService.Success("Usuario encontrado", 4);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idUsuario,descripcion,idEstado,facturacion,pago,netflix,amazon,disney,hbo,youtube,spotify")] UsuarioDTO usuario)
        {
            if (id != usuario.idUsuario)
            {
                _notyfService.Information("El id no corresponde", 4);
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _notyfService.Information(await _context.UpdateUsuario(usuario),4);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.UsuarioExists(usuario.idUsuario))
                    {
                        _notyfService.Error("Usuario no existe", 4);
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.GetUsuariobyId(id, false);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.GetUsuariobyId(id, false);
            await _context.DeleteUsuario(usuario);
            return RedirectToAction(nameof(Index));
        }
    }
}
