using Microsoft.AspNetCore.Mvc;
using ProjTech.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace ProjTech.Controllers
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
            return View();
        }

        public async Task <IActionResult> Analiz()
        {
            using( var client = new HttpClient())
            {
                var response = await client.PostAsync("http://localhost:5000/analytical", null);
                var result = await response.Content.ReadAsStringAsync();
                ViewBag.Result = result;
            }

            return View("Index");
        }

        public async Task<IActionResult> RandomForest(string Param2,string Param3, string Param4, string Param5)
        {
            using (var client = new HttpClient())
            {
                var requestData = new
                {
                    n_arbres = Param2,
                    profondeur = (Param4),
                     n_plis = Param5,
                    n_minimum_split = Param3,



                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/randomforest", content);
                var result = await response.Content.ReadAsStringAsync();

                ViewBag.Result = result;
                
            }

            return View("Index");
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