using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetS8.Models;
using System.Diagnostics;

namespace ProjetS8A.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    // L'utilisateur est dans le rôle "Admin"
                    return RedirectToAction("Index", "Home");
                }
                else if (User.IsInRole("Lamda"))
                {
                    // L'utilisateur est dans le rôle "Lamda"
                    return RedirectToAction("Guest", "Home");
                }
                else
                {
                    // L'utilisateur est authentifié mais n'est pas dans un rôle spécifique
                    return View();
                }
            }
            else
            {
                // L'utilisateur n'est pas authentifié, vous pouvez le rediriger vers la page de connexion
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}