using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CommunityEngagementAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController:ControllerBase
    {
        CEAppContext _db;

        public SponsorController(CEAppContext db)
        {
            _db = db;
        }

        [HttpPost("addSponsor")]
        public async Task<IActionResult> AddSponsor (SponsorDto dto)
        {
            var sponsor = await _db.Sponsor.Where(s => s.Name == dto.Name && s.Telephone == dto.Telephone && s.Address == dto.Address).FirstOrDefaultAsync();
            if(sponsor != null) { return BadRequest(new { message = "Sponsor alreaady exist" }); };

            var addSponsor = new Sponsor
            {
                Name = dto.Name,
                Address = dto.Address,
                Telephone = dto.Telephone,
                Pledge = dto.Pledge
            };
            _db.Sponsor.Add(addSponsor);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Added new sponsor to database" });
        }

        [HttpGet("getSponsors")]
        public async Task<IActionResult> getSponsors()
        {
            var sponsors = await _db.Sponsor.ToListAsync();
            if (sponsors.IsNullOrEmpty()) { return NotFound(new { message = "Sponsosrs not found at all" }); }

            return Ok(sponsors);

        }
    }
}
