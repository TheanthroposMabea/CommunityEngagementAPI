using CommunityEngagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartnerPerfomanceController : ControllerBase
    {
        private readonly CEAppContext _db;

        public PartnerPerfomanceController(CEAppContext db)
        {
            _db = db;
        }



        [HttpPost("partner/{partnerId}")]
        public async Task<IActionResult> GetPartnerImpact(int partnerId)
        {
            var partner = await _db.NGO_Partnership.Where(p => p.Partnership_Id == partnerId).Include(p => p.NGO_Application).FirstOrDefaultAsync();
            if (partner == null) { return NotFound(new { messaage = "Partner not found, please try a new ID" }); };

            var projects = await _db.Project.Where(p => p.Partner_Id == partner.NGO_Application.Reg_Number).ToListAsync();
            if (projects.IsNullOrEmpty()) { return NotFound(new { message = "Nothing found for thisn partner" }); }
                
            return Ok(projects);
        }

        [HttpPost("leaderboards")]
        public async Task<IActionResult> bestPartner()
        {
            var leaderboard = await _db.Project.GroupBy(p => p.Partner_Id) .Select(g => new
       {
           PartnerId = g.Key,
           TotalProjects = g.Count(),
           CompletedProjects = g.Count(p => p.Status == ProjectStatus.Completed)
       }).OrderByDescending(p => p.CompletedProjects).ThenByDescending(p => p.TotalProjects).Take(10).ToListAsync();

            return Ok(leaderboard);
        }

        [HttpPost("community-impact")]
        public async Task<IActionResult> CommunityImpacted()
        {
            var impact = await _db.Project
        .GroupBy(p => p.Partner_Name)
        .Select(partnerGroup => new
        {
            PartnerName = partnerGroup.Key,

            Communities = partnerGroup
                .GroupBy(p => p.Location)
                .Select(communityGroup => new
                {
                    Location = communityGroup.Key,

                    Impact = communityGroup
                        .Select(p => p.Description)
                        .Distinct()
                        .ToList()
                })
                .ToList()
        })
        .ToListAsync();

            return Ok(impact); ;
        }
    }
}
