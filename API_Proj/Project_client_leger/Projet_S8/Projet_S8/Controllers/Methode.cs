using Microsoft.AspNetCore.Mvc;

namespace Projet_S8.Controllers
{
    public class Methode : Controller
    {
        public IActionResult M_Analytique()
        {
            return View();
        }
        public IActionResult Random_Forest()
        {
            return View();
        }
        public IActionResult KNN()
        {
            return View();
        }
        public IActionResult SVM()
        {
            return View();
        }
    }
}
