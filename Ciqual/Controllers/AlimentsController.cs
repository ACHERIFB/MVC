using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciqual.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Ciqual.Controllers
{
    [Authorize]
    public class AlimentsController : Controller
    {
        const string SessionKeyIdFamille = "_IdFamille";
        private readonly CiqualContext _context;

        public AlimentsController(CiqualContext context)
        {
            _context = context;
        }
            
        [AllowAnonymous]
        // GET: Aliments
        public async Task<IActionResult> Index(int id)
        {

            FamillesAlim famillesAlim = new FamillesAlim();

            if (id ==0)
            {
                id = _context.Famille.OrderBy(F => F.Nom).First().IdFamille;
               
            }

            HttpContext.Session.SetInt32(SessionKeyIdFamille, id); // pour la memorisation de la session
            ViewBag.IdFamille=id;
            //familleSelect = _context.Famille.Where(F => F.IdFamille == id);
            var listeAliments = _context.Aliment.Where(A => A.IdFamille == id).Select(a => new AlimentsConsti(a.IdAliment, a.Nom, a.IdFamille, a.Composition.Count)).ToList();
            
            famillesAlim.AlimentsConsti=listeAliments;
            

            var listeFamilles = await _context.Famille.OrderBy(F=>F.Nom).ToListAsync();
            //var listeAliments = _context.Aliment.ToListAsync();
            
            famillesAlim.Familles= listeFamilles;
            return View(famillesAlim);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ListByFirstLetter(string lettre, int page=1)
        {
            var aliments = _context.Aliment.Where(a => a.Nom.Substring(0, 1) == lettre).OrderBy(a => a.Nom);
            PageItems<Aliment> pagealiments = await PageItems<Aliment>.CreateAsync(aliments, page, 20);
            ViewBag.lettre = lettre;
            return View(pagealiments);
        }
        [AllowAnonymous]
        // GET: Aliments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aliment = await _context.Aliment
                .Include(a => a.IdFamilleNavigation).Include(c=>c.Composition)
                .FirstOrDefaultAsync(m => m.IdAliment == id);
            if (aliment == null)
            {
                return NotFound();
            }

            return View(aliment);
        }


        public IActionResult Create()
        {
            int? idEnreg = HttpContext.Session.GetInt32(SessionKeyIdFamille);
            ViewData["IdFamille"] = new SelectList(_context.Famille, "IdFamille", "Nom", idEnreg);
            return View();
        }

        // POST: Aliments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAliment,Nom,IdFamille")] Aliment aliment)
        {
            if (ModelState.IsValid)
            {

                _context.Add(aliment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }         
            ViewData["IdFamille"] = new SelectList(_context.Famille, "IdFamille", "Nom", aliment.IdFamille);
            return View(aliment);
        }

        // GET: Aliments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aliment = await _context.Aliment.FindAsync(id);
            if (aliment == null)
            {
                return NotFound();
            }
            ViewData["IdFamille"] = new SelectList(_context.Famille, "IdFamille", "Nom", aliment.IdFamille);
            return View(aliment);
        }

        // POST: Aliments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAliment,Nom,IdFamille")] Aliment aliment)
        {
            if (id != aliment.IdAliment)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aliment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlimentExists(aliment.IdAliment))
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
            ViewData["IdFamille"] = new SelectList(_context.Famille, "IdFamille", "Nom", aliment.IdFamille);
            return View(aliment);
        }

        // GET: Aliments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aliment = await _context.Aliment
                .Include(a => a.IdFamilleNavigation)
                .FirstOrDefaultAsync(m => m.IdAliment == id);
            if (aliment == null)
            {
                return NotFound();
            }

            return View(aliment);
        }

        // POST: Aliments/Delete/5
        [HttpPost, ActionName("Delete")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aliment = await _context.Aliment.FindAsync(id);
            _context.Aliment.Remove(aliment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlimentExists(int id)
        {
            return _context.Aliment.Any(e => e.IdAliment == id);
        }
    }
}
