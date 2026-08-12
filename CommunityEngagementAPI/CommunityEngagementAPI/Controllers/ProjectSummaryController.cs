using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectSummaryController :ControllerBase
    {
        private readonly CEAppContext _db;
        private INotificationService _notificationService;

        public ProjectSummaryController(CEAppContext db, INotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }


        [HttpPost("generatesummary")]
        public async Task<IActionResult> GenerateSummary([FromForm] GenerateSummaryDto dto)
        {
            //try
            //{
            var summaryExisting = await _db.Project_Summary.Where(s => s.Project_Id == dto.Project_Id).AnyAsync();
            if (summaryExisting)
            {
                return BadRequest(new { message = "the summary for this project already exist" });
            }
                var registrations = await _db.Project_Register.Where(pr => pr.Project_Id == dto.Project_Id && pr.Is_Committed == true).ToListAsync();


                var attendance = await _db.Attendance_Register.Include(a => a.Project).Where(x => x.Project_Id == dto.Project_Id).FirstOrDefaultAsync();


                var entry = await _db.Attendance_Entry.Where(e => e.Attendance_Id == attendance.Attendance_Id).ToListAsync();

                var studentCount = await _db.Student.Where(s => entry.Select(r => r.User_Id).Contains(s.Student_Id)).CountAsync();
                //var assistant_Id = attendance.Assistant_Id;

                var alumniCount = await _db.Alumni.Where(a => entry.Select(r => r.User_Id).Contains(a.Alumni_Id)).CountAsync();


                var demographics = await _db.User_Demographics.Where(d => registrations.Select(r => r.User_Id).Contains(d.User_Id)).ToListAsync();

               // var completed = attendance.AttendanceEntries.Select(a => a.User_Id).Count();

                var projHours = await _db.Attendance_Entry.Where(e => e.Attendance_Id == attendance.Attendance_Id).Select(e => e.Hours_Logged).SumAsync();

                var projVolunteers = await _db.Attendance_Entry.Where(e => e.Attendance_Id == attendance.Attendance_Id).CountAsync();

                var projectStart = attendance.Project.Date.ToDateTime(attendance.Project.Time);
                var lateLimitTime = projectStart.AddMinutes(10);
                var latecomers = await _db.Attendance_Entry.Where(a => a.Checkin_Time > lateLimitTime).CountAsync();

            var projectEnd = projectStart.AddHours(attendance.Project.Hours);
            var completed = await _db.Attendance_Entry.Where(a => a.Checkout_Time > projectEnd).CountAsync();

                var summary = new Project_Summary
                {

                    Project_Id = dto.Project_Id,
                    Total_Registered = registrations.Count,
                    Total_Volunteered = projVolunteers,
                    Completed_Count = completed,
                    Absent_Count = registrations.Count -
                        projVolunteers,
                    Late_Count = latecomers,
                    Student_Count = studentCount,
                    Alumni_Count = alumniCount,
                    Male_Count = demographics
                        .Count(d => d.Gender == "Male"),
                    Female_Count = demographics
                        .Count(d => d.Gender == "Female"),
                    Total_Hours = (double)projHours,
                    Completion_Notes = dto.Notes,
                    Generated_At = DateTime.UtcNow

                };


                if (summary.Total_Volunteered > 0)
                {
                    summary.Average_Hours =
                        summary.Total_Hours /
                        summary.Total_Volunteered;
                }



                _db.Project_Summary.Add(summary);
                await _db.SaveChangesAsync();

                foreach (var file in dto.Photos)
                {
                    using var memory = new MemoryStream();

                    await file.CopyToAsync(memory);


                    var photo = new Photo
                    {
                        Summary_Id = summary.Summary_Id,
                        Type = file.ContentType,
                        Data = memory.ToArray()
                    };


                    _db.Photo.Add(photo);
                }
                await _db.SaveChangesAsync();
              
              var type = Enum.TryParse<NotificationType>("SummaryGenerated", true, out var parsed)
             ? parsed
             : NotificationType.SummaryGenerated;

                await _notificationService.NotifyUser(attendance.Assistant_Id, "Summary Generated", "You have successfully created a project summary for " + attendance.Project.Title, type);
       // }
    //        catch(Exception ex)
    //        {
    //            Console.WriteLine(ex.ToString());
    //            return StatusCode(500, new
    //            {
    //                message = "Failed generate summary",
    //                error = ex.Message
    //});
           // }

            return Ok(new { message = "Generated  summary  successfully " });
        }


        //[HttpGet("summary/{Project_Id}")]
        //public async Task<IActionResult> GetSummary(int Project_Id)
        //{
        //    var summary = await _db.Project_Summary.Where(s => s.Project_Id == Project_Id).FirstOrDefaultAsync();
        //    if (summary == null) { return NotFound(new { message = "There is no summary yet for this project." }); }

        //    return Ok(summary);
        //}

        [HttpGet("summary/{Project_Id}")]
        public async Task<IActionResult> GetSummary(int Project_Id)
        {
            var summary = await _db.Project_Summary.Include(s => s.Project).Include(s => s.Photos).FirstOrDefaultAsync(s => s.Project_Id == Project_Id);

            if (summary == null)
            {
                return NotFound(new
                {
                    message = "Project summary not found"
                });
            }
            // Get all volunteers registered for project
            var registrations = await _db.Project_Register.Include(r => r.User).ThenInclude(u => u.Faculty).Where(r => r.Project_Id == summary.Project_Id).ToListAsync();

         
            var faculties = registrations.Where(r => r.User.Faculty != null).GroupBy(r => new
                {
                    r.User.Faculty_Id,
                    r.User.Faculty.Name
                }) .Select(g => new FacultySummaryDto
                {
                    Faculty_Id = g.Key.Faculty_Id,
                    Faculty_Name = g.Key.Name,
                    Volunteer_Count = g.Count()
                }).OrderByDescending(x => x.Volunteer_Count).ToList();

            var attendance = await _db.Attendance_Entry.Include(a => a.User).Where(a => a.Attendance_Register.Project_Id == summary.Project_Id).ToListAsync();
            //volunteer who attended
            var attendedVolunteers = attendance.Select(a => a.User_Id).ToHashSet();

          
            var absentVolunteers = registrations .Where(a => !attendedVolunteers.Contains(a.User_Id)).Select(a => new AbsentVolunteerDto
                {
                    User_Id = a.User_Id,
                    Name = a.User.First_Name + " " + a.User.Last_Name,
                    Email = a.User.Email
                }).ToList();


            var dto = new ProjectSummaryDto
            {
                Summary_Id = summary.Summary_Id,

                Project_Id = summary.Project_Id,

                Project_Name = summary.Project.Title,

                Project_Date = summary.Project.Date,

                Location = summary.Project.Location,

                Total_Registered = summary.Total_Registered,

                Total_Volunteers = summary.Total_Volunteered,

                Completed_Count = summary.Completed_Count,

                Absent_Count = summary.Absent_Count,

                Late_Count = summary.Late_Count,

                Total_Hours = summary.Total_Hours,
                Average_Hours = summary.Average_Hours,

                Male_Count = summary.Male_Count,
                Female_Count = summary.Female_Count,
                Student_Count = summary.Student_Count,
                Alumni_Count = summary.Alumni_Count,

                Completion_Notes = summary.Completion_Notes,

                Generated_At = summary.Generated_At,

                //Photos = summary.Photos.Select(p => new SummaryPhotoDto
                //    {
                //        Photo_Id = p.Photo_Id,
                //        Data = p.Data,
                //        Type = p.Type
                //    }).ToList(),

                Late_Volunteers = absentVolunteers,

                Faculties = faculties
            };


            return Ok(dto);
        }

        [HttpPost("edit-summary")]
        public async Task<IActionResult> EditSummary([FromForm] EditSummaryDto dto)
        {
            var summary = await _db.Project_Summary.Include(s => s.Photos).FirstOrDefaultAsync(s => s.Summary_Id == dto.Summary_Id);

            if (summary == null)
            {
                return NotFound(new
                {
                    message = "Project summary not found"
                });
            }

            summary.Completion_Notes = dto.Notes;

            // delete  photos
            if (dto.Deleted_Photo_Ids != null && dto.Deleted_Photo_Ids.Any())
            {
                var photosToDelete = summary.Photos.Where(p => dto.Deleted_Photo_Ids.Contains(p.Photo_Id)).ToList();
                if (photosToDelete.Any())
                {
                    _db.Photo.RemoveRange(photosToDelete);
                }
            }

            // add new photos
            if (dto.New_Photos != null && dto.New_Photos.Any())
            {
                foreach (var file in dto.New_Photos)
                {
                    using var memoryStream = new MemoryStream();

                    await file.CopyToAsync(memoryStream);

                    var photo = new Photo
                    {
                        Data = memoryStream.ToArray(),

                        Type = file.ContentType,

                        Summary_Id = summary.Summary_Id
                    };

                    _db.Photo.Add(photo);
                }
            }
            await _db.SaveChangesAsync();

            return Ok(new{message = "Project summary updated successfully" });
        }
    }
}
