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
            return View(await _context.GetPlataformaCuentas());
        }
    }
}
