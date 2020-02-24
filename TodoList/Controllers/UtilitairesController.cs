using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class UtilitairesController : Controller
    {
        public IActionResult Index()
        {
            Calcul calcul = new Calcul() {DateInitiale=DateTime.Today };
            return View(calcul);
        }
        [ActionName("calculsdates")]
        public IActionResult AjouterJours(Calcul calcul)
        {
            if (ModelState.IsValid)
            {

                ModelState.Remove("DateResultat");
                if (calcul.Operation == selectoperation.plus)
                {
                    calcul.DateResultat = calcul.DateInitiale.AddDays(calcul.JoursAjoutes);
                }
                else
                {
                    calcul.DateResultat = calcul.DateInitiale.AddDays(-(calcul.JoursAjoutes));
                }
                @ViewBag.Resultat = calcul.DateResultat.ToShortDateString();
            }
                return View("Index", calcul);
        }

        public FileResult Download()
        {
            string fileName = "Exercice7.rar";
            byte[] fileBytes = System.IO.File.ReadAllBytes($"wwwroot/Telechargement/{fileName}");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


    }
}