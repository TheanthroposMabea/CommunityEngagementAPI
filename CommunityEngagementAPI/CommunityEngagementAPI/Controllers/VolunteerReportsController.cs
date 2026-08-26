using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/reports/volunteers")]
    public class VolunteerReportsController : ControllerBase
    {
        private readonly IVolunteerReportService _reportService;

        public VolunteerReportsController(IVolunteerReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [OutputCache(Duration = 900)]
        public async Task<ActionResult<UserReportDto>> GetVolunteerReport([FromQuery] int? campusId)
        {
            var report = await _reportService
                .GetVolunteerReportAsync(campusId);

            return Ok(report);
        }
    }
}
