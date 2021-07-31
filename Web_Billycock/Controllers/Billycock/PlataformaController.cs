using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Billycock.Models;
using Billycock.Repositories.Interfaces;
using Billycock.DTO;
using Microsoft.EntityFrameworkCore;

namespace Web_Billycock.Controllers.Billycock
{
    public class PlataformaController : Controller
    {
        private readonly IPlataformaRepository _context;

        public PlataformaController(IPlataformaRepository context)
        {
            _context = context;
        }

        // GET: Plataforma
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetPlataformas());
        }

        // GET: Plataforma/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plataforma = await _context.GetPlataformabyId(id);
            if (plataforma == null)
            {
                return NotFound();
            }

            return View(plataforma);
        }

        // GET: Plataforma/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plataforma/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idPlataforma,descripcion,numeroMaximoUsuarios,precio,idEstado")] PlataformaDTO plataforma)
        {
            if (ModelState.IsValid)
            {
                await _context.InsertPlataforma(plataforma);
                return RedirectToAction(nameof(Index));
            }
            return View(plataforma);
        }

        // GET: Plataforma/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plataforma = await _context.GetPlataformabyId(id);
            if (plataforma == null)
            {
                return NotFound();
            }
            return View(plataforma);
        }

        // POST: Plataforma/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idPlataforma,descripcion,numeroMaximoUsuarios,precio,idEstado")] PlataformaDTO plataforma)
        {
            if (id != plataforma.idPlataforma)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdatePlataforma(plataforma);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.PlataformaExists(plataforma.idPlataforma))
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
            return View(plataforma);
        }

        // GET: Plataforma/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plataforma = await _context.GetPlataformabyId(id);
            if (plataforma == null)
            {
                return NotFound();
            }

            return View(plataforma);
        }

        // POST: Plataforma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plataforma = await _context.GetPlataformabyId(id);
            await _context.DeletePlataforma(plataforma);
            return RedirectToAction(nameof(Index));
        }
    }
}
