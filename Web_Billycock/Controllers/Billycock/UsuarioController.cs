using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Billycock.Models;
using Billycock.Repositories.Interfaces;
using Billycock.DTO;

namespace Web_Billycock.Controllers
{
    public class UsuarioController : Controller
    {
        //private readonly IUsuarioRepository _context;
        //private string mensajeEntreVistas;

        //public UsuarioController(IUsuarioRepository context)
        //{
        //    _context = context;
        //}

        //// GET: Usuario
        //public async Task<IActionResult> Index()
        //{
        //    ViewBag.ShowDialog = true;
        //    return View(await _context.GetUsuarios("WEB"));
        //}

        //// GET: Usuario/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var usuario = await _context.GetUsuariobyId(id, "WEB");
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(usuario);
        //}

        //// GET: Usuario/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Usuario/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("descripcion,netflix,amazon,disney")] UsuarioDTO usuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        mensajeEntreVistas = await _context.InsertUsuario(usuario);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(usuario);
        //}

        //// GET: Usuario/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var usuario = await _context.GetUsuariobyId(id, "WEB");
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(usuario);
        //}

        //// POST: Usuario/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("idUsuario,descripcion,idEstado,facturacion,pago,netflix,amazon,disney")] UsuarioDTO usuario)
        //{
        //    if (id != usuario.idUsuario)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            mensajeEntreVistas = await _context.UpdateUsuario(usuario, "WEB");
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await _context.UsuarioExists(usuario.idUsuario))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(usuario);
        //}

        //// GET: Usuario/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var usuario = await _context.GetUsuariobyId(id, "WEB");
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(usuario);
        //}

        //// POST: Usuario/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var usuario = await _context.GetUsuariobyId(id, "WEB");
        //    mensajeEntreVistas = await _context.DeleteUsuario(usuario, "WEB");
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
