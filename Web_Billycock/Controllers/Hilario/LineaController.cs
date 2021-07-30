using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Billycock.Data;
using Billycock.Models.Hilario;

namespace Web_Billycock.Controllers.Hilario
{
    public class LineaController : Controller
    {
        private readonly HilarioServiceContext _context;

        public LineaController(HilarioServiceContext context)
        {
            _context = context;
        }

        // GET: Linea
        public async Task<IActionResult> Index()
        {
            return View(await _context.LINEA.ToListAsync());
        }

        // GET: Linea/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var linea = await _context.LINEA
                .FirstOrDefaultAsync(m => m.idLinea == id);
            if (linea == null)
            {
                return NotFound();
            }

            return View(linea);
        }

        // GET: Linea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Linea/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idLinea,descripcion")] Linea linea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(linea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(linea);
        }

        // GET: Linea/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var linea = await _context.LINEA.FindAsync(id);
            if (linea == null)
            {
                return NotFound();
            }
            return View(linea);
        }

        // POST: Linea/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idLinea,descripcion")] Linea linea)
        {
            if (id != linea.idLinea)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(linea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineaExists(linea.idLinea))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(linea);
        }

        // GET: Linea/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var linea = await _context.LINEA
                .FirstOrDefaultAsync(m => m.idLinea == id);
            if (linea == null)
            {
                return NotFound();
            }

            return View(linea);
        }

        // POST: Linea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var linea = await _context.LINEA.FindAsync(id);
            _context.LINEA.Remove(linea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineaExists(int id)
        {
            return _context.LINEA.Any(e => e.idLinea == id);
        }
    }
}
