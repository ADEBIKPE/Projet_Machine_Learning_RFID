using Microsoft.AspNetCore.Mvc;
using Projet_S8.Models;
using System.Diagnostics;

namespace Projet_S8.Controllers
{
    public class Choix_methode : Controller
    {
        public IActionResult Methode_choix()
        {
            return View();
        }
    }
}
