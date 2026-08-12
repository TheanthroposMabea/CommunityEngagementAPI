using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunityEngagementAPI.Controllers
{

    [ApiController]
    [Route("api/reports/ngos")]
    public class NgoReportsController:ControllerBase
    {
        private readonly INgoReportService _ngoReportService;

        public NgoReportsController(INgoReportService ngoReportService)
        {
            _ngoReportService = ngoReportService;
        }

        [HttpGet]
        public async Task<ActionResult<NgoReportDto>> GetNgoReport(
            [FromQuery] int? campusId)
        {
            var report =
                await _ngoReportService.GetNgoReport(campusId);

            return Ok(report);
        }

    }
}
