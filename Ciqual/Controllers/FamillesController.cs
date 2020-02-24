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
    public class FamillesController : Controller
    {
        private readonly CiqualContext _context;

        public FamillesController(CiqualContext context)
        {
            _context = context;
        }

  

        public async Task<IActionResult> Index(string sortOrder)
        {
            IQueryable<Famille> reqEmpl = _context.Famille;

            // si la chaîne de tri reçue en paramère est vide,
            // on définit un critère de tri par défaut
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "Nom";

            // On applique le tri à la requête
            switch (sortOrder)
            {
                case "Nom":
                    reqEmpl = reqEmpl.OrderBy(e => e.Nom);
                    break;
                case "Nom_desc":
                    reqEmpl = reqEmpl.OrderByDescending(e => e.Nom);
                    break;
                case "IdFamille":
                    reqEmpl = reqEmpl.OrderBy(e => e.IdFamille);
                    break;
                case "IdFamille_desc":
                    reqEmpl = reqEmpl.OrderByDescending(e => e.IdFamille);
                    break;


            }

            // Pour chaque critère, on envoie le sens du tri inverse
            // à la vue pour le prochain appel de la méthode
            ViewData["TriParNom"] = sortOrder == "Nom" ? "Nom_desc" : "Nom";
            ViewData["TriParId"] = sortOrder == "IdFamille" ? "IdFamille_desc" : "IdFamille";
            // On récupère les données triées
            var familles = await reqEmpl.AsNoTracking().ToListAsync();

            // On renvoie une vue
            return View(familles);
        }



    }
}
