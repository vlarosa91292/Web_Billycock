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
    public class ProductoController : Controller
    {
        private readonly HilarioServiceContext _context;

        public ProductoController(HilarioServiceContext context)
        {
            _context = context;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            return View(await (from p in _context.PRODUCTO
                               select new Producto()
                               {
                                   idProducto = p.idProducto,
                                   codigoBarra = p.codigoBarra,
                                   descripcion = p.descripcion,
                                   fechaVencimiento = p.fechaVencimiento,
                                   idlinea = p.idlinea,
                                   descLinea = (from l in _context.LINEA where l.idLinea == p.idlinea select l.descripcion).FirstOrDefault(),
                                   idProveedor = p.idProveedor,
                                   descProveedor = (from pr in _context.PROVEEDOR where pr.idProveedor == p.idProveedor select pr.descripcion).FirstOrDefault(),
                                   loteCaja = p.loteCaja,
                                   precioUnitario = p.precioUnitario
                               }).ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.PRODUCTO
                .FirstOrDefaultAsync(m => m.idProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idProducto,codigoBarra,descripcion,idlinea,idProveedor,fechaVencimiento,loteCaja,precioUnitario")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.precioUnitario = double.Parse(producto.precioUnitario.ToString().Replace(",", "."));
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.PRODUCTO.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idProducto,codigoBarra,descripcion,idlinea,idProveedor,fechaVencimiento,loteCaja,precioUnitario")] Producto producto)
        {
            if (id != producto.idProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.idProducto))
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
            return View(producto);
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.PRODUCTO
                .FirstOrDefaultAsync(m => m.idProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.PRODUCTO.FindAsync(id);
            _context.PRODUCTO.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.PRODUCTO.Any(e => e.idProducto == id);
        }
    }
}
