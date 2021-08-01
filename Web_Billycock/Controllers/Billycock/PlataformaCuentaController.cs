using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Billycock.Data;
using Billycock.Models;
using Web_Billycock.Repositories.Interfaces;
using Billycock.DTO;

namespace Web_Billycock.Controllers.Billycock
{
    public class PlataformaCuentaController : Controller
    {
        private readonly IBillycock_WebRepository<PlataformaCuenta> _context;

        public PlataformaCuentaController(IBillycock_WebRepository<PlataformaCuenta> context)
        {
            _context = context;
        }

        // GET: PlataformaCuenta
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetPlataformaCuentas(true));
        }

        // GET: PlataformaCuenta/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plataformaCuenta = await _context.GetPlataformaCuentabyIds(id, true);
            if (plataformaCuenta == null)
            {
                return NotFound();
            }

            return View(plataformaCuenta);
        }

        // GET: PlataformaCuenta/Edit/5
        public async Task<IActionResult> CrearContraseña(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plataformaCuenta = await _context.GetPlataformaCuentabyIds(id, true);
            if (plataformaCuenta == null)
            {
                return NotFound();
            }
            return View(plataformaCuenta);
        }

        // POST: PlataformaCuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearContraseña(string id, [Bind("idPlataformaCuenta,idCuenta,idPlataforma,fechaPago,usuariosdisponibles,clave")] PlataformaCuentaDTO plataformaCuenta)
        {
            if (id != plataformaCuenta.idPlataformaCuenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdatePlataformaCuenta(plataformaCuenta);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.PlataformaCuentaExists(plataformaCuenta.idPlataformaCuenta))
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
            return View(plataformaCuenta);
        }

        //ActualizarPago
        public async Task<IActionResult> ActualizarPago(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plataformaCuenta = await _context.GetPlataformaCuentabyIds(id, true);
            if (plataformaCuenta == null)
            {
                return NotFound();
            }
            return View(plataformaCuenta);
        }

        // POST: PlataformaCuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPago(string id, [Bind("idPlataformaCuenta,idCuenta,idPlataforma,usuariosdisponibles,clave")] PlataformaCuentaDTO plataformaCuenta)
        {
            if (id != plataformaCuenta.idPlataformaCuenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdatePlataformaCuenta(plataformaCuenta);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.PlataformaCuentaExists(plataformaCuenta.idPlataformaCuenta))
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
            return View(plataformaCuenta);
        }
    }
}
