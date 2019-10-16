using System.Collections.Generic;
using System.Threading.Tasks;
using KentekenClient.Models;
using Microsoft.AspNetCore.Mvc;
using KentekenClient.ApiConnect;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace KentekenClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly KentekenAPI ApiConnection = new KentekenAPI();

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            HttpClient client = ApiConnection.Initial();

            //Converteer de Registergegevens naar een string en zet de register gegevens om naar een JSON bestand
            string registerData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(registerData, System.Text.Encoding.UTF8, "application/json");

            //Roep de rest/web API aan
            HttpResponseMessage response = client.PostAsync("/api/auth/register", contentData).Result;

            //Checkt of de aanvraag goed gegaan is
            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Gebruiker succesvol geregistreerd";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Wanneer er een fout is bij het registeren, slaan we deze fouten op in het RegisterErrorModel
                string stringJwt = response.Content.ReadAsStringAsync().Result;
                var errorRegister = JsonConvert.DeserializeObject<List<RegisterErrorModel>>(stringJwt);

                foreach (RegisterErrorModel error in errorRegister)
                {
                    ViewBag.ErrorMessage += error.Description;
                }

                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            HttpClient client = ApiConnection.Initial();

            //Maakt een JSON string met de login gegevens
            string loginData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(loginData, System.Text.Encoding.UTF8, "application/json");

            //Roept de WebApi aan om een JWT token terug te krijgen
            HttpResponseMessage response = client.PostAsync("/api/auth/login", contentData).Result;

            //Check of response juist is
            if (response.IsSuccessStatusCode)
            {
                string stringJwt = response.Content.ReadAsStringAsync().Result;
                JwtModel jwt = JsonConvert.DeserializeObject<JwtModel>(stringJwt);

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Email) };
                var userIdentity = new ClaimsIdentity(claims, "login");

                //Login en sla op als cookie sessie. Heel belangrijk; login sessie stopt, wanneer JWT token verloopt
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(userIdentity),
                                              new AuthenticationProperties
                                              {
                                                  IsPersistent = true,
                                                  ExpiresUtc = jwt.Expiration
                                              });

                //Zet de token in een sessie. Deze hebben we later weer nodig
                HttpContext.Session.SetString("token", jwt.Token);

                ViewBag.SuccessMessage = "Gebruiker succesvol ingelogd";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Email en/of password onjuist";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //Logout en verwijder token 
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove("token");

            return RedirectToAction("Index", "Home");
        }
    }
}