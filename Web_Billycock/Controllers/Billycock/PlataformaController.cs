using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Billycock.Models;

namespace Web_Billycock.Controllers.Billycock
{
    public class PlataformaController : Controller
    {
        //private readonly BillycockServiceContext _context;

        //public PlataformaController(BillycockServiceContext context)
        //{
        //    _context = context;
        //}

        //// GET: Plataforma
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.PLATAFORMA.ToListAsync());
        //}

        //// GET: Plataforma/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plataforma = await _context.PLATAFORMA
        //        .FirstOrDefaultAsync(m => m.idPlataforma == id);
        //    if (plataforma == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(plataforma);
        //}

        //// GET: Plataforma/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Plataforma/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("idPlataforma,descripcion,numeroMaximoUsuarios,precio,idEstado")] Plataforma plataforma)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(plataforma);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(plataforma);
        //}

        //// GET: Plataforma/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plataforma = await _context.PLATAFORMA.FindAsync(id);
        //    if (plataforma == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(plataforma);
        //}

        //// POST: Plataforma/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("idPlataforma,descripcion,numeroMaximoUsuarios,precio,idEstado")] Plataforma plataforma)
        //{
        //    if (id != plataforma.idPlataforma)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(plataforma);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PlataformaExists(plataforma.idPlataforma))
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
        //    return View(plataforma);
        //}

        //// GET: Plataforma/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plataforma = await _context.PLATAFORMA
        //        .FirstOrDefaultAsync(m => m.idPlataforma == id);
        //    if (plataforma == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(plataforma);
        //}

        //// POST: Plataforma/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var plataforma = await _context.PLATAFORMA.FindAsync(id);
        //    _context.PLATAFORMA.Remove(plataforma);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PlataformaExists(int id)
        //{
        //    return _context.PLATAFORMA.Any(e => e.idPlataforma == id);
        //}
    }
}
