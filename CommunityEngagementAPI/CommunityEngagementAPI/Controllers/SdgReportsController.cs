using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/reports/sdgs")]
    public class SdgReportsController: ControllerBase
    {
        private readonly CEAppContext _db;
        private readonly ISdgReportService _sdgReportService;
        public SdgReportsController(CEAppContext db , ISdgReportService sdgReportService)
        {
            _db = db;
            _sdgReportService = sdgReportService;
        }

        [HttpGet]
        [OutputCache(Duration = 900)]
        public async Task<ActionResult<SdgReportDto>> GetSdgReport(
            [FromQuery] int? campusId)
        {
            var report =
                await _sdgReportService.GetSdgReport(campusId);

            return Ok(report);
        }

    }
}
