using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntelligenceController : ControllerBase
    {
        private readonly IIntelligenceService _intelligenceService;

        public IntelligenceController(IIntelligenceService intelligenceService)
        {
            _intelligenceService = intelligenceService;
        }

        //get the health of all projects or a single project
        [HttpGet("project-health")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetProjectHealth([FromQuery] int? Project_Id = null)
        {
            try
            {
                var health = await _intelligenceService.GetProjectHealth(Project_Id);

                if (!health.Any())
                {
                    return NotFound(new { message = "No projects found" });
                }

                return Ok(health);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return project health",
                    error = ex.Message
                });
            }
        }

        //get problems detected in projects
        [HttpGet("project-problems")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetProjectProblems()
        {
            try
            {
                return Ok(await _intelligenceService.GetProjectProblems());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return project problems",
                    error = ex.Message
                });
            }
        }

        //get recommendations for projects
        [HttpGet("project-recommendations")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetProjectRecommendations()
        {
            try
            {
                return Ok(await _intelligenceService.GetProjectRecommendations());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return project recommendations",
                    error = ex.Message
                });
            }
        }

        //get community impact for the selected period
        [HttpGet("community-impact")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetCommunityImpact([FromQuery] int days = 30)
        {
            try
            {
                return Ok(await _intelligenceService.GetCommunityImpact(days));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return community impact",
                    error = ex.Message
                });
            }
        }

        //get areas that are performing well or require attention
        [HttpGet("area-performance")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetAreaPerformance()
        {
            try
            {
                return Ok(await _intelligenceService.GetAreaPerformance());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return area performance",
                    error = ex.Message
                });
            }
        }


        //get complete intelligence for one project
        [HttpGet("project/{projectId:int}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetProjectIntelligence(int projectId)
        {
            try
            {
                return Ok(await _intelligenceService.GetProjectIntelligence(projectId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return project intelligence",
                    error = ex.Message
                });
            }
        }

        //get intelligence for an assistant and the assistant's work
        [HttpGet("assistant-dashboard/{assistantId:int}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetAssistantDashboard(int assistantId)
        {
            try
            {
                return Ok(await _intelligenceService.GetAssistantDashboard(assistantId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the Assistant Intelligence Dashboard",
                    error = ex.Message
                });
            }
        }

        //get intelligence for a volunteer and the volunteer's engagement
        [HttpGet("volunteer-dashboard/{volunteerId:int}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetVolunteerDashboard(int volunteerId)
        {
            try
            {
                return Ok(await _intelligenceService.GetVolunteerDashboard(volunteerId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the Volunteer Intelligence Dashboard",
                    error = ex.Message
                });
            }
        }

        //get intelligence for a specialist and the specialist's campus
        [HttpGet("specialist-dashboard/{specialistId:int}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetSpecialistDashboard(int specialistId, [FromQuery] int days = 30)
        {
            try
            {
                var dashboard = await _intelligenceService.GetSpecialistDashboard(specialistId, days);
                return Ok(dashboard);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the Specialist Intelligence Dashboard",
                    error = ex.Message
                });
            }
        }

        //get the full intelligence dashboard
        [HttpGet("dashboard")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                return Ok(await _intelligenceService.GetDashboard());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the Intelligence Dashboard",
                    error = ex.Message
                });
            }
        }
    }
}
