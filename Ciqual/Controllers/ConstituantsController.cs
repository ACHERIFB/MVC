using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciqual.Models;

namespace Ciqual.Controllers
{
    public class ConstituantsController : Controller
    {
        private readonly CiqualContext _context;

        public ConstituantsController(CiqualContext context)
        {
            _context = context;
        }

        // GET: Constituants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Constituant.ToListAsync());
        }

       
    }
}
