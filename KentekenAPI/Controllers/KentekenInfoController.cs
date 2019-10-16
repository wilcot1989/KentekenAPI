using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KentekenAPI.Data;
using KentekenAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace KentekenAPI.Controllers
{
    //Opvraag van kenteken gegevens beveiligd met Authorize. Er is nu een JWT token benodigd om de gevens op te vragen
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class KentekenInfoController : ControllerBase
    {
        private readonly KentekenDbContext _context;

        public KentekenInfoController(KentekenDbContext context)
        {
            _context = context;
        }

        // GET: api/KentekenInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KentekenInfo>>> GetKentekenInfo()
        {
            //Geeft maximaal 5 items terug. 
            return await _context.KentekenInfo.Take(5).ToListAsync();
        }

        //We gebruiken alleen maar de GET functie. 
        // GET: api/KentekenInfo/Kentekennummer
        [HttpGet("{kenteken}")]
        public async Task<ActionResult<KentekenInfo>> GetKentekenInfo(string kenteken)
        {
            //Zoek het kenteken op in de database
            var kentekenInfo = await _context.KentekenInfo.FindAsync(kenteken);

            //Als het kenteken niet gevonden is, geven we not found terug
            if (kentekenInfo == null)
            {
                return NotFound();
            }

            //Sla het kenteken op wat de gebruiker op dat moment aanroept
            var kentekenLog = new UserKentekenLog
            {
                Kenteken = kenteken,
                Log_Datetime = DateTime.UtcNow.ToLocalTime(),
                User_Id = User.FindFirstValue(JwtRegisteredClaimNames.Aud)
            };
            _context.UserKentekenLog.Add(kentekenLog);
            await _context.SaveChangesAsync();

            return kentekenInfo;
        }

        // GET: api/KentekenInfo/userkentekenlog
        [HttpGet("userkentekenlog")]
        public async Task<ActionResult<IEnumerable<UserKentekenLog>>> GetUserKentekenLog()
        {
            //Zoek de userID op vanuit de JWT token. Deze hebben we eerder meegegeven in Aud
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Aud);

            //Zoek alle kenteken opvragingen van deze gebruiker op
            var kentekenLogs = await _context.UserKentekenLog.Where(x => x.User_Id == userId).ToListAsync(); 

            if (kentekenLogs == null)
            {
                return NotFound();
            }

            return kentekenLogs;
        }
    }
}
