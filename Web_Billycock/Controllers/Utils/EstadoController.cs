using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Billycock.Models;
using Web_Billycock.Repositories.Interfaces;

namespace Web_Billycock.Controllers.Billycock
{
    public class EstadoController : Controller
    {
        private readonly IEstadoRepository _context;

        public EstadoController(IEstadoRepository context)
        {
            _context = context;
        }

        // GET: Estado
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeleteEstado(new Estado() { idEstado=1,descripcion="Activo"}));
        }

        //// GET: Estado/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var estado = await _context.ESTADO
        //        .FirstOrDefaultAsync(m => m.idEstado == id);
        //    if (estado == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(estado);
        //}

        //// GET: Estado/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Estado/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("idEstado,descripcion")] Estado estado)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(estado);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(estado);
        //}

        //// GET: Estado/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var estado = await _context.ESTADO.FindAsync(id);
        //    if (estado == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(estado);
        //}

        //// POST: Estado/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("idEstado,descripcion")] Estado estado)
        //{
        //    if (id != estado.idEstado)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(estado);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EstadoExists(estado.idEstado))
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
        //    return View(estado);
        //}

        //// GET: Estado/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var estado = await _context.ESTADO
        //        .FirstOrDefaultAsync(m => m.idEstado == id);
        //    if (estado == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(estado);
        //}

        //// POST: Estado/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var estado = await _context.ESTADO.FindAsync(id);
        //    _context.ESTADO.Remove(estado);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool EstadoExists(int id)
        //{
        //    return _context.ESTADO.Any(e => e.idEstado == id);
        //}
    }
}
