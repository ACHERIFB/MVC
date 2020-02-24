using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // On définit en constante la clé de l’info à mémoriser
        //static int nombreVisite;
        const string SessionKeyNombreVisite = "_nombreVisite";
        int nombreVisite;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        


        public IActionResult Index()
        {
            return View();
        }
        [ActionName("ContactUs")] // si on ajoute cette route , obligé d'ajouter dans View le nom de la page html correspendante ( Contact)
        public IActionResult Contact(int id, string nom)
        {

            //ViewData["id"] = "Votre id est " + id
            ViewData["idnom"] = "Votre id est "+id +" et votre nom est "+nom ;
            return View("Contact");
        }
        public IActionResult About()
        {

            int? nombreVisite = HttpContext.Session.GetInt32(SessionKeyNombreVisite);
            if (nombreVisite == null) nombreVisite = 0;
       
            HttpContext.Session.SetInt32(SessionKeyNombreVisite, nombreVisite.Value+1);

           
            ViewData["Message"] = "Vous avez deja visité ce site " + nombreVisite + " fois ";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
