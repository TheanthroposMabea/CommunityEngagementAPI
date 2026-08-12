using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<UserReportDto>> GetVolunteerReport([FromQuery] int? campusId)
        {
            var report = await _reportService
                .GetVolunteerReportAsync(campusId);

            return Ok(report);
        }
    }
}
