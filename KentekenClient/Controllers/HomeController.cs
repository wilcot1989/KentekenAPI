using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KentekenClient.Models;
using KentekenClient.ApiConnect;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace KentekenClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly KentekenAPI ApiConnection = new KentekenAPI();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Authorize]
        public ViewResult KentekenInfo()
        {
            return View();
        }

        [HttpPost, Authorize]
        public IActionResult KentekenInfo(KentekenInfoViewModel model)
        {
            if (model.Kenteken != null)
            {
                //Roep de web api aan om kentekengegevens op te vragen
                HttpClient client = ApiConnection.Initial();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                HttpResponseMessage response = client.GetAsync("/api/KentekenInfo/" + model.Kenteken).Result;

                //Wanneer de aanvraag goed gegaan is
                if (response.IsSuccessStatusCode)
                {
                    string stringKenteken = response.Content.ReadAsStringAsync().Result;
                    var responseKentekenInfo = JsonConvert.DeserializeObject<KentekenInfoViewModel>(stringKenteken);

                    return View(responseKentekenInfo);
                }
                else
                {
                    ViewBag.ErrorMessage = "Kentekengegevens niet gevonden";
                }
            }
            return View();
        }

        [HttpGet, Authorize]
        public ViewResult KentekenRdw()
        {
            return View();
        }

        [HttpPost, Authorize]
        public IActionResult KentekenRdw(KentekenRdwViewModel model)
        {
            if (model.Kenteken != null)
            {
                //Roept de database van de RDW aan. 
                var client = new HttpClient();
                HttpResponseMessage response = client.GetAsync("https://opendata.rdw.nl/resource/m9d7-ebf2.json?$$app_token=O4MRqRqEyFVD4wqjGima8C7Go&kenteken="
                                                               + model.Kenteken.ToUpper()).Result;
                //Wanneer de aanvraag goed gegaan is
                if (response.IsSuccessStatusCode)
                {
                    string stringKenteken = response.Content.ReadAsStringAsync().Result;
                    var kentekenList = JsonConvert.DeserializeObject<List<KentekenRdwViewModel>>(stringKenteken);

                    return View(kentekenList[0]);
                }
                else
                {
                    ViewBag.ErrorMessage = "Kentekengegevens niet gevonden";
                }
            }
            return View();
        }

        [HttpGet, Authorize]
        public IActionResult KentekenLog()
        {
            //Roep de web api aan om kentekengegevens op te vragen
            HttpClient client = ApiConnection.Initial();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            HttpResponseMessage response = client.GetAsync("/api/KentekenInfo/userkentekenlog").Result;

            //Wanneer de aanvraag goed gegaan is
            if (response.IsSuccessStatusCode)
            {
                string stringKentekenLog = response.Content.ReadAsStringAsync().Result;
                var kentekenList = JsonConvert.DeserializeObject<List<KentekenLogViewModel>>(stringKentekenLog);

                return View(kentekenList);
            }
            else
            {
                ViewBag.ErrorMessage = "Geen kentekenlog gegevens gevonden";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
