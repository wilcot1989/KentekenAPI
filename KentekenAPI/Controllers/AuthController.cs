using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using KentekenAPI.Models;


namespace KentekenAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //Kijkt of de gebruiker aanwezig in de database
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //Maakt de claims aan voor de JWT token
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Aud, user.Id)
                };

                //Maakt de JWT Token hier aan, en stuurt hem mee met de web API
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jeweetzelluftokenkeyverzinmaarwat"));
                var token = new JwtSecurityToken(
                    issuer: "KentekenAPI",
                    audience: "https://localhost:44341",
                    expires: DateTime.Now.AddMinutes(60),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            //Declareren van de user model
            var user = new ApplicationUser { UserName = model.Email, 
                                             Email = model.Email };
            //Slaat de gebruiker op in de database
            var result = await _userManager.CreateAsync(user, model.Password);

            //Wanneer het aanmaken goed is gegaan, wordt er OK terug gestuurd. Wanneer fout een error list 
            if (result.Succeeded)
            {
                return new OkObjectResult(ModelState);
            }
            else
            {
                return new BadRequestObjectResult(result.Errors.ToList());
            }
        }
    }
}