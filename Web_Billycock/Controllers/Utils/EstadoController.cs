using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Billycock.Models;
using Web_Billycock.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Web_Billycock.DTO;

namespace Web_Billycock.Controllers.Billycock
{
    public class EstadoController : Controller
    {
        private readonly IBillycock_WebRepository<Estado> _context;

        public EstadoController(IBillycock_WebRepository<Estado> context)
        {
            _context = context;
        }

        //GET: Estado
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetEstados());
        }

        // GET: Estado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.GetEstadobyId(id);
            if (estado == null)
            {
                return NotFound();
            }

            return View(estado);
        }

        // GET: Estado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idEstado,descripcion")] EstadoDTO estado)
        {
            if (ModelState.IsValid)
            {
                await _context.InsertEstado(estado);
                return RedirectToAction(nameof(Index));
            }
            return View(estado);
        }

        // GET: Estado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.GetEstadobyId(id);
            if (estado == null)
            {
                return NotFound();
            }
            return View(estado);
        }

        // POST: Estado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idEstado,descripcion")] EstadoDTO estado)
        {
            if (id != estado.idEstado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateEstado(estado);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.EstadoExists(estado.idEstado))
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
            return View(estado);
        }

        // GET: Estado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.GetEstadobyId(id);
            if (estado == null)
            {
                return NotFound();
            }

            return View(estado);
        }

        // POST: Estado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estado = await _context.GetEstadobyId(id);
            await _context.DeleteEstado(estado);
            return RedirectToAction(nameof(Index));
        }
    }
}
