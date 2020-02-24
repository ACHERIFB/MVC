using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class TachesController : Controller
    {
        private readonly TodoListContext _context;


        public TachesController(TodoListContext context)
        {
            _context = context;
        }

        // GET: Taches
        public async Task<IActionResult> Index(string TextSaisi, int Etat)
        {
            ViewBag.Recherche = TextSaisi; // pour que ca reste memoriser
            int tacheTerminee = 0;
            var listeTache = await _context.Taches.ToListAsync();
            IQueryable<Tache> listeAffichee = _context.Taches; // une liste a completer : pour eviter les repition de context

            var DictSelection = new Dictionary<int, string>()
           {
               { 1, "Toutes" },
               { 2, "Terminees" },
               { 3, "Non Terminees" },
           };


            if (Etat == 0) //Gestion de la premiere connexion 
            {
                // Lecture d’une valeur dans un cookie
                if (Request.Cookies.TryGetValue("EtatCookies", out string val))  // faire appel a l'EtatCookie declaré plus bas ( si il éxiste )
                {
                    if (int.TryParse(val, out int etatCookie))
                    {
                        Etat = etatCookie;
                    }
                }
            }

            ViewBag.selection = new SelectList(DictSelection, "Key", "Value", Etat = Etat == 0 ? 1 : Etat);
            //---------------------------------------la barre de recherche------------------------------

            if (TextSaisi == null)
            {
                //if (Etat == 0)
                //{
                //    listeAffichee = listeAffichee;
                //}
                if (Etat == 2)
                {
                    listeAffichee = listeAffichee.Where(t => t.Terminee == true); //on complete notre liste 
                }
                if (Etat == 3)
                {
                    listeAffichee = listeAffichee.Where(t => t.Terminee == false);
                }
            }
            else
            {

                //ViewData["tachesgroupees"] = new List<Tache>();
                if (Etat == 1)
                {
                    listeAffichee = listeAffichee.Where(T => T.Description.Contains(TextSaisi));
                }
                if (Etat == 2)
                {
                    listeAffichee = listeAffichee.Where(t => t.Terminee == true).Where(T => T.Description.Contains(TextSaisi));
                }
                if (Etat == 3)
                {
                    listeAffichee = listeAffichee.Where(t => t.Terminee == false).Where(T => T.Description.Contains(TextSaisi));
                }
            }
            //------------------------------------------------------------------------------------------------------------------
            //---------------------------calcul du nombre de tache terminee pour l'afficher en haut de ma vue-----------------
            foreach (var item in listeTache)
            {

                if (item.Terminee)
                {
                    tacheTerminee++;
                }
            }
            ViewData["tacheTerminee"] = listeTache.Where(l => l.Terminee == true).Count();
            ViewData["totaltache"] = listeTache.Count();
            //-------------------------------------la creation ---------------------------------------------------------------
            Tache tacheAjoutee = new Tache() { DateCreation = DateTime.Today };
            listeTache.Add(tacheAjoutee);

            //----------------------------------------Cookies-----------------------------------------------------------------
            
            

            // Ecriture d’une valeur dans un cookie
            var options = new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(5)   // Le cookie expirera dans 5 minutes 
            };
            Response.Cookies.Append("EtatCookies", Etat.ToString(), options); // declaration de EtatCookie qui sera appelé a la premiere connexion





            //-----------------------------------------------------------------------------------------------------------------

            return View(await listeAffichee.ToListAsync()); // nous finalisons notre listeaffichee et lister
        }

        // GET: Taches/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Taches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tache == null)
            {
                return NotFound();
            }

            return View(tache);
        }

        public IActionResult InitSaisie()
        {
            return View("CreationGroupee");
        }

        [HttpPost]
        public IActionResult InitSaisie(int nombreTache)
        {
            Tache tache = new Tache() { DateEcheance = DateTime.Now.AddDays(7) };
            List<Tache> tachesgroupees = new List<Tache>();
            for (int i = 0; i < nombreTache; i++)
            {
                tachesgroupees.Add(tache);

            }
            return View("CreationGroupee", tachesgroupees);
        }


        [HttpPost]
        public async Task<IActionResult> CreationGroupee([Bind("Id,Description,DateEcheance")] List<Tache> taches)
        {
            if (ModelState.IsValid)
            {
                foreach (var tache in taches)
                {
                    tache.DateCreation = DateTime.Now;
                    _context.Add(tache);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("CreationGroupee", taches);
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Taches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DateCreation,DateEcheance,Terminee")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tache);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tache);
        }

        // GET: Taches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Taches.FindAsync(id);
            if (tache == null)
            {
                return NotFound();
            }
            return View(tache);
        }

        // POST: Taches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DateCreation,DateEcheance,Terminee")] Tache tache)
        {
            if (id != tache.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tache);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TacheExists(tache.Id))
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
            return View(tache);
        }

        // GET: Taches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Taches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tache == null)
            {
                return NotFound();
            }

            return View(tache);
        }

        // POST: Taches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tache = await _context.Taches.FindAsync(id);
            _context.Taches.Remove(tache);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TacheExists(int id)
        {
            return _context.Taches.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Statistiques()
        {

            var stat = new StatistiquesViewModel();
            List<Tache> taches = await _context.Taches.ToListAsync();

            stat.NbrTachesTerminee = taches.Where(T => T.Terminee == true).Count();
            stat.NbrTachesRetard = taches.Where(T => T.DateEcheance > DateTime.Today).Count();
            stat.NbrTachesEnCours = taches.Count() - taches.Where(T => T.Terminee == false).Count();
            stat.DelaiMoyen = taches.Where(t => t.Terminee).Average(t => ((DateTime)t.DateEcheance - t.DateCreation).TotalDays);


            return View(stat);
        }

    }
}
