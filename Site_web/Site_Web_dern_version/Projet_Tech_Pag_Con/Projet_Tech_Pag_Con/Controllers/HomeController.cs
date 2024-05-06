using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Projet_Tech_Pag_Con.Models;
using System.Diagnostics;
using System.Text;

namespace Projet_Tech_Pag_Con.Controllers
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
                    // L'utilisateur est dans le r�le "Admin"
                    return RedirectToAction("IndexAdmin", "Home");
                }
                if (!User.IsInRole("Admin"))
                {
                    // L'utilisateur n'est pas dans le r�le "Admin", rediriger vers la page "Guest"
                    return RedirectToAction("Guest", "Home");
                }
                else
                {
                    // L'utilisateur est authentifi� mais n'est pas dans un r�le sp�cifique
                    return View();
                }
            }
            else
            {
                // L'utilisateur n'est pas authentifi�, vous pouvez le rediriger vers la page de connexion
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }

        public IActionResult Guest()
        {
            return View();
        }
        public IActionResult Resultats()
        {
            return View();
        }

        public IActionResult IndexAdmin()
        {
            return View("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Analiz()
        {
            using (var client = new HttpClient())
            {

                var response = await client.PostAsync("http://localhost:5000/analytical", null);
                //var result = await response.Content.ReadAsStringAsync();
                //ViewBag.Result = result;
                var result = await response.Content.ReadAsStringAsync();

                // Analyse la cha�ne JSON pour extraire les valeurs de pr�cision et de temps d'ex�cution
                var jsonObject = JObject.Parse(result);
                var accuracy = jsonObject["accuracy"].ToObject<double>();
                var executionTimeInSeconds = (double)jsonObject["execution_time"];

                // Formatage du temps d'ex�cution avec trois chiffres apr�s la virgule
                var formattedExecutionTime = executionTimeInSeconds.ToString("0.###") + " secs";
                // Stocke les valeurs dans ViewBag
                ViewBag.Accuracy = accuracy;
                ViewBag.ExecutionTime = formattedExecutionTime;
                //ViewBag.ExecutionTime = executionTime;

            }

            return View("Index");
        }

        public async Task<IActionResult> RandomForest(string Param2, string Param3, string Param4, string Param5)
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

                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5000/randomforest", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                // Convertir la r�ponse JSON en un objet dynamique
                dynamic jsonResponse = JsonConvert.DeserializeObject(result);

                // Stocker les r�sultats dans ViewBag ou un autre objet
                ViewBag.Score = jsonResponse.score;
                ViewBag.MatriceDeConfusion = jsonResponse.matrice_de_confusion;
                double tempsExecution = jsonResponse.temps_execution;
                string tempsExecutionFormate = tempsExecution.ToString("0.###");
                ViewBag.TempsExecution = tempsExecutionFormate;

                //ViewBag.TempsExecution = jsonResponse.temps_execution;

                // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans ViewBag
                JArray detailsArray = JArray.Parse(jsonResponse.details_classement.ToString());
                List<dynamic> detailsList = detailsArray.ToObject<List<dynamic>>();
                ViewBag.DetailsClassement = detailsList;


                /*var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/randomforest", content);
                var result = await response.Content.ReadAsStringAsync();

                ViewBag.Result = result;*/

            }

            return View("Index");
        }
        public async Task<IActionResult> KNN(string Param2, string Param3, string Param4, string Param5)
        {
            using (var client = new HttpClient())
            {
                var requestData = new
                {
                    metric = Param4,
                    n_neighbor = Param2,
                    n_plis = Param5,
                    weights = Param3,



                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/knn", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                // Convertir la r�ponse JSON en un objet dynamique
                dynamic jsonResponse = JsonConvert.DeserializeObject(result);

                // Stocker les r�sultats dans ViewBag ou un autre objet
                ViewBag.Score = jsonResponse.score;
                ViewBag.MatriceDeConfusion = jsonResponse.matrice_de_confusion;
                double tempsExecution = jsonResponse.temps_execution;
                string tempsExecutionFormate = tempsExecution.ToString("0.###");
                ViewBag.TempsExecution = tempsExecutionFormate;
                //ViewBag.TempsExecution = jsonResponse.temps_execution;

                // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans ViewBag
                JArray detailsArray = JArray.Parse(jsonResponse.details_classement.ToString());
                List<dynamic> detailsList = detailsArray.ToObject<List<dynamic>>();
                ViewBag.DetailsClassement = detailsList;
                //var result = await response.Content.ReadAsStringAsync();

                //ViewBag.Result = result;

            }

            return View("Index");
        }


        public async Task<IActionResult> SVM(string Param1, string Param2, string Param3, string Param4)
        {
            using (var client = new HttpClient())
            {
                // Sp�cifier un d�lai d'attente plus long (par exemple, 5 minutes)
                client.Timeout = TimeSpan.FromMinutes(5);

                var requestData = new
                {
                    C = Param1,
                    Kernel = Param2,
                    n_plis = Param4,
                    Gamma = Param3,



                };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/SVM", content);

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                // Convertir la r�ponse JSON en un objet dynamique
                dynamic jsonResponse = JsonConvert.DeserializeObject(result);

                // Stocker les r�sultats dans ViewBag ou un autre objet
                ViewBag.Score = jsonResponse.score;
                ViewBag.MatriceDeConfusion = jsonResponse.matrice_de_confusion;
                double tempsExecution = jsonResponse.temps_execution;
                string tempsExecutionFormate = tempsExecution.ToString("0.###");
                ViewBag.TempsExecution = tempsExecutionFormate;
                //ViewBag.TempsExecution = jsonResponse.temps_execution;

                // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans ViewBag
                JArray detailsArray = JArray.Parse(jsonResponse.details_classement.ToString());
                List<dynamic> detailsList = detailsArray.ToObject<List<dynamic>>();
                ViewBag.DetailsClassement = detailsList;

                //var result = await response.Content.ReadAsStringAsync();

                //ViewBag.Result = result;

                /*var jsonObject = JObject.Parse(result);
                var score = (double)jsonObject["score"];
                var confusionMatrix = jsonObject["matrice de confusion"].ToString();
                var executionTime = (double)jsonObject["execution_time"];
                var detailsClassement = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonObject["details_classement"].ToString());

                // Stockage des valeurs dans ViewBag pour l'affichage dans la vue
                ViewBag.Score = score;
                ViewBag.ConfusionMatrix = confusionMatrix;
                ViewBag.ExecutionTime = executionTime;
                ViewBag.DetailsClassement = detailsClassement;*/


            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
