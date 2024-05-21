using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Projet_Tech_Pag_Con.Models;
using System.Diagnostics;
using System.Text;
using Humanizer;
using System.Security.Cryptography;

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

        public async Task<IActionResult> Valider(string Param1, string Param2,string Param3, string Param4, string Param5,string Param6,string Param7,
            string Param8,string  Param9,string Param10,string Param11, string Param12,string Param13, string Param14, string Param15,string Param16,
            string Param17, string Param18, string Param19, string Param20, string Param21,string Param22, string Param23,string  Param24,string Param25,
            string Param26, string Param27, string Param28,string Param29, string Param30,string  Param31,string  Param32, string Param33, string Param34,
            string Param35,string Param36, string Param37,string Param38,string Param39,string Param40, string Param41, string Param42, 
            string method1, string method2,string method3,string method4)
        {

            if(method1 == "Analytique")
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
            }
            if (method4 == "RandomForest")
            {
                using (var client = new HttpClient())
                {
                    var requestData = new
                    {
                        n_arbres = Param24,
                        profondeur = Param26,
                        n_plis = Param42,
                        n_minimum_split = Param25,
                        criterion = Param27,
                        min_samples_leaf = Param29,
                        min_weight_fraction_leaf = Param30,
                        max_features = Param28,
                        max_leaf_nodes = Param31,
                        min_impurity_decrease = Param32,
                        bootstrap = Param33,
                        oob_score = Param34,
                        n_jobs = Param35,
                        random_state = Param36,
                        verbose = Param37,
                        warm_start = Param38,
                        class_weight = Param39,
                        ccp_alpha = Param40,
                        max_samples = Param41,

                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("http://localhost:5000/randomforest", content);
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    // Convertir la r�ponse JSON en un objet dynamique
                    dynamic jsonResponse = JsonConvert.DeserializeObject(result);

                    // Stocker les r�sultats dans ViewBag ou un autre objet
                    ViewBag.ScoreRF = jsonResponse.score;
                    ViewBag.MatriceDeConfusionRF = jsonResponse.matrice_de_confusion;
                    double tempsExecution = jsonResponse.temps_execution;
                    string tempsExecutionFormate = tempsExecution.ToString("0.###");
                    ViewBag.TempsExecutionRF = tempsExecutionFormate;

                    //ViewBag.TempsExecution = jsonResponse.temps_execution;

                    // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans ViewBag
                    JArray detailsArray = JArray.Parse(jsonResponse.details_classement.ToString());
                    List<dynamic> detailsList = detailsArray.ToObject<List<dynamic>>();
                    ViewBag.DetailsClassementRF = detailsList;


                    /*var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/randomforest", content);
                    var result = await response.Content.ReadAsStringAsync();

                    ViewBag.Result = result;*/

                }

            }


            if(method3=="KNN")
            {
                using (var client = new HttpClient())
                {
                    var requestData = new
                    {
                        metric = Param16,
                        n_neighbors = Param17,
                        n_plis = Param18,
                        weights = Param19,
                        algorithm = Param20,
                        leaf_size = Param21,
                        p = Param22,
                        n_jobs = Param23,
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/knn", content);
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    // Convertir la r�ponse JSON en un objet dynamique
                    dynamic jsonResponse = JsonConvert.DeserializeObject(result);

                    // Stocker les r�sultats dans ViewBag ou un autre objet
                    ViewBag.ScoreKNN = jsonResponse.score;
                    ViewBag.MatriceDeConfusionKNN = jsonResponse.matrice_de_confusion;
                    double tempsExecution = jsonResponse.temps_execution;
                    string tempsExecutionFormate = tempsExecution.ToString("0.###");
                    ViewBag.TempsExecutionKNN = tempsExecutionFormate;
                    //ViewBag.TempsExecution = jsonResponse.temps_execution;

                    // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans ViewBag
                    JArray detailsArray = JArray.Parse(jsonResponse.details_classement.ToString());
                    List<dynamic> detailsList = detailsArray.ToObject<List<dynamic>>();
                    ViewBag.DetailsClassementKNN = detailsList;
                    //var result = await response.Content.ReadAsStringAsync();

                    //ViewBag.Result = result;

                }
            }
            if (method2 == "SVM")
            {
                using (var client = new HttpClient())
                {
                    // Sp�cifier un d�lai d'attente plus long (par exemple, 5 minutes)
                    client.Timeout = TimeSpan.FromMinutes(5);

                    var requestData = new
                    {
                        regularisation = Param1,
                        CoefNoyau = Param3,
                        n_plis = Param4,
                        Noyau = Param2,
                        degree = Param5,
                        coef0 = Param6,
                        shrinking = Param7,
                        probability = Param8,
                        tol = Param9,
                        cache_size = Param10,
                        verbose = Param11,
                        max_iter = Param12,
                        decision_function_shape = Param13,
                        break_ties = Param14,
                        random_state = Param15,

                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/SVM", content);

                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    // Convertir la r�ponse JSON en un objet dynamique
                    dynamic jsonResponse = JsonConvert.DeserializeObject(result);

                    // Stocker les r�sultats dans ViewBag ou un autre objet
                    ViewBag.ScoreSVM = jsonResponse.score;
                    ViewBag.MatriceDeConfusionSVM = jsonResponse.matrice_de_confusion;
                    double tempsExecution = jsonResponse.temps_execution;
                    string tempsExecutionFormate = tempsExecution.ToString("0.###");
                    ViewBag.TempsExecutionSVM = tempsExecutionFormate;
                    //ViewBag.TempsExecution = jsonResponse.temps_execution;

                    // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans ViewBag
                    JArray detailsArray = JArray.Parse(jsonResponse.details_classement.ToString());
                    List<dynamic> detailsList = detailsArray.ToObject<List<dynamic>>();
                    ViewBag.DetailsClassementSVM = detailsList;

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
            }








                return View("Index");






        }







        
































  


        public IActionResult Privacy()
        {
            return View();
        }

    }
}
