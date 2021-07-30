using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Billycock.Data;
using Billycock.Models;

namespace Web_Billycock.Controllers.Billycock
{
    public class PlataformaCuentaController : Controller
    {
        //private readonly BillycockServiceContext _context;

        //public PlataformaCuentaController(BillycockServiceContext context)
        //{
        //    _context = context;
        //}

        //// GET: PlataformaCuenta
        //public async Task<IActionResult> Index()
        //{
        //    var billycockServiceContext = _context.PLATAFORMACUENTA.Include(p => p.Cuenta).Include(p => p.Plataforma);
        //    return View(await billycockServiceContext.ToListAsync());
        //}

        //// GET: PlataformaCuenta/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plataformaCuenta = await _context.PLATAFORMACUENTA
        //        .Include(p => p.Cuenta)
        //        .Include(p => p.Plataforma)
        //        .FirstOrDefaultAsync(m => m.idCuenta == id);
        //    if (plataformaCuenta == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(plataformaCuenta);
        //}

        //// GET: PlataformaCuenta/Create
        //public IActionResult Create()
        //{
        //    ViewData["idCuenta"] = new SelectList(_context.CUENTA, "idCuenta", "idCuenta");
        //    ViewData["idPlataforma"] = new SelectList(_context.PLATAFORMA, "idPlataforma", "idPlataforma");
        //    return View();
        //}

        //// POST: PlataformaCuenta/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("usuariosdisponibles,fechaPago,clave,idPlataforma,idCuenta")] PlataformaCuenta plataformaCuenta)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(plataformaCuenta);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["idCuenta"] = new SelectList(_context.CUENTA, "idCuenta", "idCuenta", plataformaCuenta.idCuenta);
        //    ViewData["idPlataforma"] = new SelectList(_context.PLATAFORMA, "idPlataforma", "idPlataforma", plataformaCuenta.idPlataforma);
        //    return View(plataformaCuenta);
        //}

        //// GET: PlataformaCuenta/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plataformaCuenta = await _context.PLATAFORMACUENTA.FindAsync(id);
        //    if (plataformaCuenta == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["idCuenta"] = new SelectList(_context.CUENTA, "idCuenta", "idCuenta", plataformaCuenta.idCuenta);
        //    ViewData["idPlataforma"] = new SelectList(_context.PLATAFORMA, "idPlataforma", "idPlataforma", plataformaCuenta.idPlataforma);
        //    return View(plataformaCuenta);
        //}

        //// POST: PlataformaCuenta/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("usuariosdisponibles,fechaPago,clave,idPlataforma,idCuenta")] PlataformaCuenta plataformaCuenta)
        //{
        //    if (id != plataformaCuenta.idCuenta)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(plataformaCuenta);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PlataformaCuentaExists(plataformaCuenta.idCuenta))
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
        //    ViewData["idCuenta"] = new SelectList(_context.CUENTA, "idCuenta", "idCuenta", plataformaCuenta.idCuenta);
        //    ViewData["idPlataforma"] = new SelectList(_context.PLATAFORMA, "idPlataforma", "idPlataforma", plataformaCuenta.idPlataforma);
        //    return View(plataformaCuenta);
        //}

        //// GET: PlataformaCuenta/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plataformaCuenta = await _context.PLATAFORMACUENTA
        //        .Include(p => p.Cuenta)
        //        .Include(p => p.Plataforma)
        //        .FirstOrDefaultAsync(m => m.idCuenta == id);
        //    if (plataformaCuenta == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(plataformaCuenta);
        //}

        //// POST: PlataformaCuenta/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var plataformaCuenta = await _context.PLATAFORMACUENTA.FindAsync(id);
        //    _context.PLATAFORMACUENTA.Remove(plataformaCuenta);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PlataformaCuentaExists(int id)
        //{
        //    return _context.PLATAFORMACUENTA.Any(e => e.idCuenta == id);
        //}
    }
}
