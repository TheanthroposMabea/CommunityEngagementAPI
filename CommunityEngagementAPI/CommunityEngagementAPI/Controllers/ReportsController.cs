using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IManagementReportService _reportService;
		
        public ReportsController(IManagementReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("projects")]
        public async Task<ActionResult<ProjectReportDto>> GetProjectReport([FromQuery] int? campusId = null)
        {
            var report = await _reportService.GetProjectReportAsync(campusId);
            return Ok(report);
        }

        [HttpGet("overall-performance")]
        public async Task<ActionResult<OverallPerformanceReportDto>> GetOverallPerformanceReport([FromQuery] int? campusId = null)
        {
            var report = await _reportService.GetOverallPerformanceReportAsync(campusId);
            return Ok(report);
        }
    }
}
