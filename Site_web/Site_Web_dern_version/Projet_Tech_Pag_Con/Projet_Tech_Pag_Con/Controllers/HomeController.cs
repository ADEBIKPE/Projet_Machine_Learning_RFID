using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Projet_Tech_Pag_Con.Models;
using System.Diagnostics;
using System.Text;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Projet_Tech_Pag_Con.Controllers;
using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Data;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol;
using System.Security.Claims;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.DotNet.MSIdentity.Shared;

namespace Projet_Tech_Pag_Con.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        private readonly UserManager<IdentityUser> _userManager;



        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    // L'utilisateur est dans le r�le "Admin"
                    return RedirectToAction("IndexAdmin", "Home");
                }
                if (User.IsInRole("Lamda"))
                {
                    // L'utilisateur n'est pas dans le r�le "Admin", rediriger vers la page "Guest"
                    return RedirectToAction("Guest", "Home");
                }
                if (!User.IsInRole("Lamda") && !User.IsInRole("Admin"))
                {
                    // L'utilisateur n'est pas dans le r�le "Admin", rediriger vers la page "Guest"
                    return RedirectToAction("Index", "ExecutionMethodesAdmins");
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
        public async Task<IActionResult> Upload()
        {
            foreach (var file in Request.Form.Files)
            {
                var fileName = Path.GetFileName(file.FileName);
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var filePath = Path.Combine(uploadDirectory, fileName);

                // V�rifier si le r�pertoire existe, sinon le cr�er
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using (var client = new HttpClient())
                {
                    var requestData = new
                    {
                        chemin = filePath,
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/Chemin", content);

                }

                // Retourner le chemin du fichier t�l�charg� dans l'en-t�te de r�ponse HTTP
                return View("Index");
            }

            // Si aucun fichier n'a �t� t�l�charg�, retourner une r�ponse BadRequest
            return BadRequest("Aucun fichier t�l�charg�.");
        }

        public IActionResult Guest()
        {
            return View();
        }
        public IActionResult SuperUtilisateur()
        {
            return View();
        }
        public IActionResult Histogramme_Boite_Moustache()
        {
            return View();
        }
        public IActionResult Result()
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

        public async Task<IActionResult> Valider(string Param1, string Param2, string Param3, string Param4, string Param5, string Param6, string Param7,
            string Param8, string Param9, string Param10, string Param11, string Param12, string Param13, string Param14, string Param15, string Param16,
            string Param17, string Param18, string Param19, string Param20, string Param21, string Param22, string Param23, string Param24, string Param25,
            string Param26, string Param27, string Param28, string Param29, string Param30, string Param31, string Param32, string Param33, string Param34,
            string Param35, string Param36, string Param37, string Param38, string Param39, string Param40, string Param41, string Param42,
            string method1, string method2, string method3, string method4,string Param1000, string Param1111)
        {
            // R�cup�rer l'utilisateur actuel
            var user = await _userManager.GetUserAsync(User);

            // V�rifier si l'utilisateur existe
            if (user != null)
            {
                // R�cup�rer l'ID de l'utilisateur
                var userId = user.Id;

                // R�cup�rer le r�le de l'utilisateur
                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                // Cr�er une nouvelle instance de Simulation
                var simu = new Simulation
                {
                    UtilisateurId = userId,
                    DateSimulation = DateTime.Now
                };

                _context.Simulation.Add(simu);
                await _context.SaveChangesAsync();

                // Votre logique pour la m�thode "Analytique"
                if (method1 == "Analytique")
                {
                    using (var client = new HttpClient())
                    {
                        var requestData = new
                        {
                            step= Param1000,
                            sec = Param1111,
                         };
                        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://localhost:5000/analytical",content);
                        response.EnsureSuccessStatusCode();
                        var result = await response.Content.ReadAsStringAsync();


                        var jsonObject = JObject.Parse(result);
                        var accuracy = jsonObject["accuracy"].ToObject<double>();
                        var executionTimeInSeconds = (double)jsonObject["execution_time"];
                        var formattedExecutionTime = executionTimeInSeconds.ToString("0.###");

                        ViewBag.Accuracy = accuracy;
                        ViewBag.ExecutionTime = formattedExecutionTime;
                        StringBuilder detailsBuilder = new StringBuilder();
                        detailsBuilder.AppendLine("Hyperparam�tres pour la m�thode analytique :");
                        foreach (var param in requestData.GetType().GetProperties())
                        {
                            detailsBuilder.AppendLine($"{param.Name} : {param.GetValue(requestData)}");
                        }

                        var execMeth = new ExecutionMethode
                        {
                            NomMethode = "Analytique",
                            Details = $"{detailsBuilder.ToString()}",
                            Performance = (float)accuracy,
                            MatriceConfusion = "Aucune",
                            Temps_Execution = formattedExecutionTime,
                            SimulationId = simu.Id
                        };

                        _context.ExecutionMethode.Add(execMeth);
                        await _context.SaveChangesAsync();

                        // Cr�er une instance d'ExecutionMethodesAdmin en utilisant les informations r�cup�r�es
                        var executionMethodesAdmin = new ExecutionMethodesAdmin
                        {
                            NomMethode = "Analytique",
                            Details = $"{detailsBuilder.ToString()}",
                            Performance = (float)accuracy,
                            MatriceConfusion = "Aucune",
                            Temps_Execution = formattedExecutionTime,
                            UserId = userId,
                            UserRoleId = "1",
                            SimulationId = simu.Id
                        };

                        _context.ExecutionMethodesAdmin.Add(executionMethodesAdmin);
                        await _context.SaveChangesAsync();

                    }
                }

                //return View("Index");
                // Ajoutez l'instance � DbContext et enregistrez les modifications

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



                        // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans Details
                        string detailsClassement = JsonConvert.SerializeObject(detailsList);

                        // Construction de la cha�ne de d�tails des hyperparam�tres
                        StringBuilder detailsBuilder = new StringBuilder();
                        detailsBuilder.AppendLine("Hyperparam�tres pour RandomForest :");
                        foreach (var param in requestData.GetType().GetProperties())
                        {
                            detailsBuilder.AppendLine($"{param.Name} : {param.GetValue(requestData)}|");
                        }

                        ExecutionMethode executionMethode = new ExecutionMethode
                        {
                            NomMethode = "RandomForest",
                            Details = $"{detailsBuilder.ToString()} \n\n D�tails de classement : \n{detailsClassement}", // D�tails sp�cifiques � RandomForest (hyperparam�tres et leurs valeurs associ�es)
                            Performance = jsonResponse.score, // Performance sp�cifique � RandomForest
                            MatriceConfusion = jsonResponse.matrice_de_confusion.ToString(), // Matrice de confusion sp�cifique � RandomForest
                            Temps_Execution = tempsExecutionFormate, // Temps d'ex�cution sp�cifique � RandomForest
                            SimulationId = simu.Id // Utiliser l'identifiant de la simulation cr��e
                        };
                        _context.ExecutionMethode.Add(executionMethode);
                        await _context.SaveChangesAsync();
                        // Cr�ez une nouvelle instance d'ExecutionMethodesAdmin avec les donn�es appropri�es
                        ExecutionMethodesAdmin executionMethodesAdmin = new ExecutionMethodesAdmin
                        {
                            NomMethode = "RandomForest",
                            Details = $"{detailsBuilder.ToString()} \n\n D�tails de classement : \n{detailsClassement}",
                            Performance = jsonResponse.score,
                            MatriceConfusion = jsonResponse.matrice_de_confusion.ToString(),
                            Temps_Execution = tempsExecutionFormate,
                            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Utilisez l'identifiant de l'utilisateur actuel
                            UserRoleId = "1", // Utilisez le r�le de l'utilisateur actuel
                            SimulationId = simu.Id
                        };

                        // Ajoutez l'instance � DbContext et enregistrez les modifications
                        _context.ExecutionMethodesAdmin.Add(executionMethodesAdmin);
                        await _context.SaveChangesAsync();

                    }

                }


                if (method3 == "KNN")
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

                        string detailsClassement = JsonConvert.SerializeObject(detailsList);



                        // Construction de la cha�ne de d�tails des hyperparam�tres
                        StringBuilder detailsBuilder = new StringBuilder();
                        detailsBuilder.AppendLine("Hyperparam�tres pour KNN :");
                        foreach (var param in requestData.GetType().GetProperties())
                        {
                            detailsBuilder.AppendLine($"{param.Name} : {param.GetValue(requestData)}");
                        }

                        // Enregistrement des r�sultats dans la table ExecutionMethode
                        ExecutionMethode executionMethode = new ExecutionMethode
                        {
                            NomMethode = "KNN",
                            Details = $"{detailsBuilder.ToString()} \n\n D�tails de classement : \n{detailsClassement}", // D�tails sp�cifiques � KNN (hyperparam�tres et d�tails de classement)
                            Performance = jsonResponse.score, // Performance sp�cifique � KNN
                            MatriceConfusion = jsonResponse.matrice_de_confusion.ToString(), // Matrice de confusion sp�cifique � KNN
                            Temps_Execution = tempsExecutionFormate, // Temps d'ex�cution sp�cifique � KNN
                            SimulationId = simu.Id // Utiliser l'identifiant de la simulation cr��e
                        };
                        _context.ExecutionMethode.Add(executionMethode);
                        await _context.SaveChangesAsync();

                        //var result = await response.Content.ReadAsStringAsync();

                        //ViewBag.Result = result;
                        ExecutionMethodesAdmin executionMethodesAdmin = new ExecutionMethodesAdmin
                        {
                            NomMethode = "KNN",
                            Details = $"{detailsBuilder.ToString()} \n\n D�tails de classement : \n{detailsClassement}",
                            Performance = jsonResponse.score,
                            MatriceConfusion = jsonResponse.matrice_de_confusion.ToString(),
                            Temps_Execution = tempsExecutionFormate,
                            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Utilisez l'identifiant de l'utilisateur actuel
                            UserRoleId = "1", // Utilisez le r�le de l'utilisateur actuel
                            SimulationId = simu.Id
                        };

                        // Ajoutez l'instance � DbContext et enregistrez les modifications
                        _context.ExecutionMethodesAdmin.Add(executionMethodesAdmin);
                        await _context.SaveChangesAsync();

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

                        // Convertir les d�tails de classement en une liste d'objets avant de les stocker dans Details
                        string detailsClassement = JsonConvert.SerializeObject(detailsList);


                        // Construction de la cha�ne de d�tails des hyperparam�tres
                        StringBuilder detailsBuilder = new StringBuilder();
                        detailsBuilder.AppendLine("Hyperparam�tres pour SVM :");
                        foreach (var param in requestData.GetType().GetProperties())
                        {
                            detailsBuilder.AppendLine($"{param.Name} : {param.GetValue(requestData)}");
                        }


                        // Enregistrement des r�sultats dans la table ExecutionMethode
                        ExecutionMethode executionMethode = new ExecutionMethode
                        {
                            NomMethode = "SVM",
                            Details = $"{detailsBuilder.ToString()} \n\n D�tails de classement : \n{detailsClassement}", // D�tails sp�cifiques � SVM (hyperparam�tres et d�tails de classement)
                            Performance = jsonResponse.score, // Performance sp�cifique � SVM
                            MatriceConfusion = jsonResponse.matrice_de_confusion.ToString(), // Matrice de confusion sp�cifique � SVM
                            Temps_Execution = tempsExecutionFormate, // Temps d'ex�cution sp�cifique � SVM
                            SimulationId = simu.Id // Utiliser l'identifiant de la simulation cr��e
                        };
                        _context.ExecutionMethode.Add(executionMethode);
                        await _context.SaveChangesAsync();

                        ExecutionMethodesAdmin executionMethodesAdmin = new ExecutionMethodesAdmin
                        {
                            NomMethode = "SVM",
                            Details = $"{detailsBuilder.ToString()} \n\n D�tails de classement : \n{detailsClassement}",
                            Performance = jsonResponse.score,
                            MatriceConfusion = jsonResponse.matrice_de_confusion.ToString(),
                            Temps_Execution = tempsExecutionFormate,
                            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Utilisez l'identifiant de l'utilisateur actuel
                            UserRoleId = "1", // Utilisez le r�le de l'utilisateur actuel
                            SimulationId = simu.Id
                        };

                        // Ajoutez l'instance � DbContext et enregistrez les modifications
                        _context.ExecutionMethodesAdmin.Add(executionMethodesAdmin);
                        await _context.SaveChangesAsync();
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



            }
            return View("Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Action pour afficher l'histogramme avec les r�sultats
        public IActionResult AfficherHistogramme(float resultatAnalytique, float resultatRandomForest, float resultatSVM, float resultatKNN)
        {
            ViewBag.ResultatAnalytique = resultatAnalytique;
            ViewBag.ResultatRandomForest = resultatRandomForest;
            ViewBag.ResultatSVM = resultatSVM;
            ViewBag.ResultatKNN = resultatKNN;
            return View();
        }

    }
}

