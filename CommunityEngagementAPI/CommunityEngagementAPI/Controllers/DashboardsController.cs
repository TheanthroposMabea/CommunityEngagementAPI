using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.OutputCaching;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardsController : ControllerBase
    {
        private readonly CEAppContext _db;

        public DashboardsController(CEAppContext db)
        {
            _db = db;
        }

        [HttpGet("admin")]
        [OutputCache(Duration = 900)]
        public async Task<ActionResult<AdminsDashboardsDto>> GetAdminsDash()
        {
            
            try
            {
                var directorDash = new AdminsDashboardsDto
                {
                    TotalVolunteers = await _db.Volunteer.CountAsync(),
                    ActiveProjects = await _db.Project.Where(p => p.Status.Equals(ProjectStatus.Pending)).CountAsync(),
                    MonthlyCompletedProjects = await _db.Project.Where(p => p.Status.Equals( ProjectStatus.Completed) && p.Date.Year == DateTime.Now.Year && p.Date.Month == DateTime.Now.Date.Month ).CountAsync(),
                    NGOPartnerships = await _db.NGO_Partnership.CountAsync(),
                    MonthlyHours = await _db.Project.Where(p => p.Status.Equals( ProjectStatus.Completed) && p.Date.Year == DateTime.Now.Year && p.Date.Month == DateTime.Now.Date.Month).Select(p => p.Hours).SumAsync(),
                    NGOApplications = await _db.NGO_Application.Where(a => a.Status.Equals(Status.Reviewed)).Select(a => new PendingNGODto
                    {
                        Id = a.Application_Id,
                        Name = a.Name,
                        Email = a.Email,
                        Campus = a.Close_Campus,
                        Status = a.Status.ToString(),
                    }).ToListAsync(),
                    Specialists = await _db.User.Where(u=>u.Role == UserRoleType.specialist).Select(u => new SpecialistDto
                    {
                        Id = u.User_Id,
                        First_Name = u.First_Name,
                        Last_Name = u.Last_Name,
                        Campus = _db.Campus.Where(c => c.Campus_Id == u.Campus_Id).Select(c => c.Name).FirstOrDefault(),
                        Phone = u.Phone,
                    }).ToListAsync(),
                };
                Console.WriteLine(directorDash.MonthlyCompletedProjects);
                Console.WriteLine(DateTime.UtcNow.Month);
                Console.WriteLine(DateTime.UtcNow.Year);

                return Ok(directorDash);
        }catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the Director Dashboard",
                    error = ex.Message
    });
            }
        }
        //give the volunteeers dashboard
        [HttpGet("volunteer/{Volunteer_Id}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetVolunteerDash(int Volunteer_Id)
        {
            try
            {
                var volunteerDash = new VolunteerDashboardDto()
            {
                HoursLogged = await _db.Volunteer.Where(v => v.Volunteer_Id == Volunteer_Id).Select(v => v.Hours).FirstOrDefaultAsync(),
                ProjectsRegistered = await _db.Project_Register.Where(pr => pr.User_Id == Volunteer_Id && pr.Is_Committed == true).CountAsync(),
                ProjectsAttended = await _db.Attendance_Entry.Where(pe => pe.User_Id == Volunteer_Id).CountAsync(),
                Certificates = await _db.Certificate.Where(c => c.User_Id == Volunteer_Id).CountAsync()

            };
            return Ok(volunteerDash);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the Volunteer Dashboard",
                    error = ex.ToString()
                });
            }
        }
        //get the assistant's dashborad
        [HttpGet("assistant/{User_Id}")]
        public async Task<IActionResult> GetAssistantDash(int User_Id)
        {

            try
            {
                var assistantAttendances = await _db.Attendance_Register.Where(pr => pr.Assistant_Id == User_Id && pr.Project.Status == ProjectStatus.Completed).Include(pr=>pr.Project).ToListAsync();
                var hours = assistantAttendances.Select(pr=>pr.Project.Hours).Sum();
               // var summaries = await _db.Project_Summary.Where(ps=> ps.Project_Id == assistantAttendances.Select(ar=>ar.Project.Project_Id)).CountAsync();
                var summaries = await _db.Project_Summary
                    .Where(ps => assistantAttendances
                        .Select(ar => ar.Project.Project_id)
                        .Contains(ps.Project_Id))
                    .CountAsync();
                var assistantDash = new AssistantDashboardDto()
                {
                    HoursLogged =  hours,
                    ProjectsRegistered = await _db.Project_Register.Where(pr => pr.User_Id == User_Id).CountAsync(),
                    ProjectsAttended = await _db.Attendance_Register.Where(pe => pe.Assistant_Id == User_Id && pe.Project.Status == ProjectStatus.Completed).CountAsync(),
                    Certificates = await _db.Certificate.Where(c => c.User_Id == User_Id).CountAsync(),
                    Reports_Submitted =  summaries,
                };
                return Ok(assistantDash);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the Volunteer Dashboard",
                    error = ex.Message
                });
            }
        }
    }
}



