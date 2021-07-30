using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Billycock.Repositories.Interfaces;
using Billycock.DTO;
using Microsoft.EntityFrameworkCore;

namespace Web_Billycock.Controllers.Billycock
{
    public class CuentaController : Controller
    {
        private readonly ICuentaRepository _context;

        public CuentaController(ICuentaRepository context)
        {
            _context = context;
        }

        // GET: Cuenta
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetCuentas());
        }

        // GET: Cuenta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.GetCuentabyId(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // GET: Cuenta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cuenta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("correo,diminutivo,netflix,amazon,disney,hbo,youtube,spotify")] CuentaDTO cuenta)
        {
            if (ModelState.IsValid)
            {
                await _context.InsertCuenta(cuenta);
                return RedirectToAction(nameof(Index));
            }
            return View(cuenta);
        }

        // GET: Cuenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.GetCuentabyId(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return View(cuenta);
        }

        // POST: Cuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idCuenta,correo,diminutivo,netflix,amazon,disney,hbo,youtube,spotify,idEstado")] CuentaDTO cuenta)
        {
            if (id != cuenta.idCuenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateCuenta(cuenta);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.CuentaExists(cuenta.idCuenta))
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
            return View(cuenta);
        }

        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.GetCuentabyId(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // POST: Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuenta = await _context.GetCuentabyId(id);
            await _context.DeleteCuenta(cuenta);
            return RedirectToAction(nameof(Index));
        }
    }
}
