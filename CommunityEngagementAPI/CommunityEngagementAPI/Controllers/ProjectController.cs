using CommunityEngagementAPI.Helpers;
using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentApp.API.Services;
using System;


namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly CEAppContext _db;
        private readonly EmailService _emailService;
        private readonly INotificationService _notificationService;
        public ProjectController(CEAppContext db, EmailService emailService, INotificationService notificationService)
        {
            _db = db;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto)
        {
            var project = await _db.Project.Where(p => p.Title == projectDto.Title && p.Date == projectDto.Date && p.User_Id == projectDto.User_Id).FirstOrDefaultAsync();
            if (project != null)
            {
                return BadRequest(new { message = "Project already exists", exist = true });
            }
            //parse the strings to enums before adding them
            Enum.TryParse<ProjectScope>(projectDto.Location_Scope, true, out var Location_Scope);
            Enum.TryParse<ProjectPartner>(projectDto.Partner, true, out var Partner);
            Enum.TryParse<ProjectCategory>(projectDto.Category, true, out var Category);

            int campus_id = 0;
            if (projectDto.Partner == "STAFF")
            {
                try
                {
                    campus_id = (int)await _db.User.Where(u => u.Id_Number == projectDto.Partner_Id).Select(u => u.Campus_Id).FirstOrDefaultAsync();
                    Console.WriteLine(campus_id);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        message = "Failed to return the Staff Campus",
                        error = ex.Message
                    });
                }
            }
            else if (projectDto.Partner == "CBO" || projectDto.Partner == "NPO")
            {
                try
                {
                    var partnerCampus = await _db.NGO_Partnership.Where(a => a.NGO_Application.Reg_Number == projectDto.Partner_Id).Select(a => a.NGO_Application.Close_Campus).FirstOrDefaultAsync();
                    Enum.TryParse<CampusCode>(partnerCampus, true, out var Code);
                    campus_id = await _db.Campus.Where(c => c.Code.Equals(Code)).Select(c => c.Campus_Id).FirstOrDefaultAsync();

                    Console.WriteLine(campus_id);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        message = "Failed to return the CBO or NPO Campus- Partnership might have lapsed",
                        error = ex.Message
                    });
                }
            }

            //make a new project and add it to the db
            var addProject = new Project
            {
                Title = projectDto.Title,
                Type = projectDto.Type,
                Location = projectDto.Location,
                Partner = Partner,
                Partner_Name = projectDto.Partner_Name,
                Partner_Id = projectDto.Partner_Id,
                Category = Category,
                Hours = projectDto.Hours,
                Num_Volunteers = projectDto.Num_Volunteers,
                Status = ProjectStatus.Pending,
                Description = projectDto.Description,
                Target_Group = projectDto.Target_Group,
                Is_Published = false,
                Date = projectDto.Date,
                Time = projectDto.Time,
                Date_Created = DateTime.Now,
                Campus_Id = campus_id,
                User_Id = projectDto.User_Id,

            };

            if (projectDto.Sdg != null && projectDto.Sdg.Length > 0)
            {
                foreach (var sdgId in projectDto.Sdg)
                {
                    addProject.ProjectSdgs.Add(new Project_Sdg
                    {
                        Sdg_Id = sdgId
                    });
                }
            }

            _db.Project.Add(addProject);
            await _db.SaveChangesAsync();
            var type = Enum.TryParse<NotificationType>("ProjectCreated", true, out var parsed)
            ? parsed
            : NotificationType.ProjectCreated;

            try
            {
                await _notificationService.NotifyUser(addProject.User_Id, "Project Created", "You have successfully created the project " + addProject.Title + ". taking place on  - " + addProject.Date + " -Id :" + addProject.Project_id , type);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the CBO or NPO Campus- Partnership might have lapsed",
                    error = ex.Message
                });
            }

            return Ok(new { message = "Project successfully created and saved" });
        }

        //specialsit or staff publish the project
        [HttpPatch("publish-project")]
        public async Task<IActionResult> PublishProject(JoinProjectDto dto)
        {
            var project = await _db.Project.Where(p => p.Project_id == dto.Project_Id && p.User_Id == dto.User_Id).FirstOrDefaultAsync();
            if (project == null)
            {
                return NotFound("Project does not exist");
            }
            else
            {
                project.Is_Published = true;
                await _db.SaveChangesAsync();

                var type = Enum.TryParse<NotificationType>("ProjectCreated", true, out var parsed)
            ? parsed
            : NotificationType.ProjectCreated;

                try
                {
                    await _notificationService.NotifyUser(project.User_Id, "Project Created", "You have published  the project " + project.Title + ". taking place on  - " + project.Date, type);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        message = "Something went wrong",
                        error = ex.Message
                    });
                }

                return Ok("Project successfully published");
            }

        }
        [HttpPatch("callback-project")]
        public async Task<IActionResult> CallBackProject(JoinProjectDto dto)
        {
            var project = await _db.Project.Where(p => p.Project_id == dto.Project_Id && p.User_Id == dto.User_Id).FirstOrDefaultAsync();
            if (project == null)
            {
                return NotFound("Project does not exist");
            }
            else
            {
                project.Is_Published = false;
                await _db.SaveChangesAsync();

                var type = Enum.TryParse<NotificationType>("ProjectCancelled", true, out var parsed)
            ? parsed
            : NotificationType.ProjectCreated;

                try
                {
                    await _notificationService.NotifyUser(project.User_Id, "Project CallBack", "You have called back  the project " + project.Title + ". taking place on  - " + project.Date, type);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        message = "Failed to return the CBO or NPO Campus- Partnership might have lapsed",
                        error = ex.Message
                    });
                }

                return Ok("Project Called Back");
            }

        }

        [HttpPost("edit-project")]
        public async Task<IActionResult> EditProject([FromBody] UpdateProjectDto dto)
        {
            var project = await _db.Project.Where(p => p.Project_id == dto.Project_Id && p.User_Id == dto.User_Id && p.Status == ProjectStatus.Pending).FirstOrDefaultAsync();
            if (project == null) { return NotFound(new { message = "Project not found." }); }

            string oldName = project.Title;

            //parse the strings to enums before adding them
            Enum.TryParse<ProjectScope>(dto.Location_Scope, true, out var Location_Scope);

            project.Title = dto.Title;
            project.Type = dto.Type;
            project.Location = dto.Location;
            project.Location_Scope = Location_Scope;
            project.Hours = dto.Hours;
            project.Num_Volunteers = dto.Num_Volunteers;
            project.Description = dto.Description;
            project.Target_Group = dto.Target_Group;
            project.Date = dto.Date;
            project.Time = dto.Time;

            await _db.SaveChangesAsync();

            var type = Enum.TryParse<NotificationType>("ProjectUpdated", true, out var parsed)
         ? parsed
         : NotificationType.ProjectUpdated;
            if (project.Is_Published == true)
            {
                await _notificationService.NotifyProjectRegister(project.Project_id, "Project Updated", "Dear Volunteer, please take note of changes made to project: " + oldName + " , Created on : " + project.Date_Created, type);
            }

            await _notificationService.NotifyUser(project.User_Id, "Project Updated", "Dear Specialist, please take note of changes you made to project: " + oldName + " , Created on : " + project.Date_Created, type);

            return Ok(new { message = "Project Updated." });
        }



        [HttpGet("retriveprojects/{User_Id}")]
        public async Task<ActionResult<ProjectDto>> GetProjects(int User_Id)
        {
            var user = await _db.User.Where(u => u.User_Id == User_Id).FirstOrDefaultAsync();

            if (user != null && user.Role == UserRoleType.specialist)
            {
                var campusProjects = await _db.Project.Where(p => p.Campus_Id == user.Campus_Id).Select(p => new ProjectDto
                {
                    Project_Id = p.Project_id,
                    Title = p.Title,
                    Type = p.Type,
                    Location = p.Location,
                    Partner = p.Partner.ToString(),
                    Partner_Name = p.Partner_Name,
                    Partner_Id = p.Partner_Id,
                    Category = p.Category.ToString(),
                    Hours = p.Hours,
                    Num_Volunteers = p.Num_Volunteers,
                    Description = p.Description,
                    Target_Group = p.Target_Group,
                    Status = p.Status.ToString(),
                    Date = p.Date,
                    Date_Created = p.Date_Created,
                    Time = p.Time,
                    Slots = p.Slots,
                    Sdg = _db.Project_Sdg.Where(ps => ps.Project_Id == p.Project_id).Select(s => s.Sdg_Id).ToArray()

                }).ToListAsync();

                return Ok(campusProjects);

            }
            else if (user != null && user.Role == UserRoleType.staff)

            {
                var projects = await _db.Project.Where(p => p.User_Id == user.User_Id).Select(p => new ProjectDto
                {
                    Project_Id = p.Project_id,
                    Title = p.Title,
                    Type = p.Type,
                    Location = p.Location,
                    Partner = p.Partner.ToString(),
                    Partner_Name = p.Partner_Name,
                    Partner_Id = p.Partner_Id,
                    Category = p.Category.ToString(),
                    Hours = p.Hours,
                    Num_Volunteers = p.Num_Volunteers,
                    Description = p.Description,
                    Target_Group = p.Target_Group,
                    Date = p.Date,
                    Date_Created = p.Date_Created,
                    Time = p.Time,
                    Slots = p.Slots,
                    Sdg = _db.Project_Sdg.Where(ps => ps.Project_Id == p.Project_id).Select(s => s.Sdg_Id).ToArray()
                }).ToListAsync();

                return Ok(projects);
            }
            else if (user != null && user.Role == UserRoleType.volunteer || user.Role == UserRoleType.assistant)
            {
                var campusProjects = await _db.Project.Where(p => p.Campus_Id == user.Campus_Id && p.Is_Published == true).Select(p => new ProjectDto
                {
                    Project_Id = p.Project_id,
                    Title = p.Title,
                    Type = p.Type,
                    Location = p.Location,
                    Partner = p.Partner.ToString(),
                    Partner_Name = p.Partner_Name,
                    Partner_Id = p.Partner_Id,
                    Category = p.Category.ToString(),
                    Hours = p.Hours,
                    Num_Volunteers = p.Num_Volunteers,
                    Description = p.Description,
                    Target_Group = p.Target_Group,
                    Status = p.Status.ToString(),
                    Date = p.Date,
                    Date_Created = p.Date_Created,
                    Time = p.Time,
                    Slots = p.Slots,
                    Sdg = _db.Project_Sdg.Where(ps => ps.Project_Id == p.Project_id).Select(s => s.Sdg_Id).ToArray()

                }).ToListAsync();

                return Ok(campusProjects);
            }
            else
            {
                return NotFound(new { message = "Something went wrong while trying to retrieve projects, might be empty" });
            }



        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _db.Project.ToListAsync();
            if (projects == null)
            {
                return NotFound(new { message = "Something went wrong while trying to retrieve projects, might be empty" });
            }

            return Ok(projects);
        }

        [HttpGet("{Project_Id}")]
        public async Task<IActionResult> GetSpecificProject(int Project_Id)
        {
            var project = await _db.Project.Where(p => p.Project_id == Project_Id).FirstOrDefaultAsync();
            if (project != null)
            {
                return Ok(project);
            }
            else
            {
                return NotFound(new { message = "Something went wrong while trying to retrieve project, might not exist" });
            }
        }

        [HttpGet("myproject-by-sdgs/{Volunteer_Id}")]
        public async Task<IActionResult> MyProjectsBySds(int Volunteer_Id)
        {
            var volunteer = await _db.Volunteer.Include(v => v.VolunteerSdgs).FirstOrDefaultAsync(v => v.Volunteer_Id == Volunteer_Id);

            if (volunteer == null)
                return NotFound();

            var volunteerSdgs = volunteer.VolunteerSdgs
                .Select(vs => vs.Sdg_Id);

            var projects = await _db.Project
                .Where(p => p.Is_Published &&
                            p.ProjectSdgs.Any(ps => volunteerSdgs.Contains(ps.Sdg_Id)))
                .ToListAsync();
            if (projects.IsNullOrEmpty()) { return NotFound(new { message = "There are no projects matching users Sdgs" }); }

            return Ok(projects);
        }

        //get project by status
        [HttpGet("projects/status/{Status}")]
        public async Task<IActionResult> GetProjectsByStatus(string Status)
        {
            if (!Enum.TryParse<ProjectStatus>(Status, true, out var appStatus))
            {
                return BadRequest("Invalid role.");
            }

            var ProjectsInStatus = await _db.Project.Where(u => u.Status.Equals(appStatus)).ToListAsync();
            if (!ProjectsInStatus.IsNullOrEmpty())
            {
                return Ok(ProjectsInStatus);
            }
            else
            {
                return NotFound(new { message = "Something went wrong, while retrieving project for this status" });
            }

        }



        //allow users to register for a project
        [HttpPost("join-project")]
        public async Task<IActionResult> JoinProject([FromBody] JoinProjectDto dto)
        {

            var user = await _db.User.FindAsync(dto.User_Id); if (user == null) return BadRequest(new { message = "User not found" });
            if (user.Is_Suspended == true) return BadRequest(new { message = "Project is not open for Volunteer.User is suspended" });

            var project = await _db.Project.FindAsync(dto.Project_Id); if (project == null) return BadRequest(new { message = "Project being joined does not exist" });
            if (project.Slots <= 0) return BadRequest(new { message = "There are no available slots for project, try again some time" });
            if (project.Status != ProjectStatus.Pending) return BadRequest(new { message = "Project is not open for registrations- only Pending projects can be joined" });


            var userRegistered = await _db.Project_Register.Where(pr => pr.User_Id == dto.User_Id && pr.Project_Id == dto.Project_Id && pr.Is_Committed == true).AnyAsync();

            if (userRegistered) return BadRequest(new { message = "User have already joined the project" });

            var userRejoin = await _db.Project_Register.Where(pr => pr.User_Id == dto.User_Id && pr.Project_Id == dto.Project_Id && pr.Is_Committed == false).FirstOrDefaultAsync();
            if (userRejoin != null)
            {
                userRejoin.Is_Committed = true;
                await _db.SaveChangesAsync();
                return Ok(new { message = "You have been rejoined the project" });
            }


            bool assistantExists = await _db.Project_Register.AnyAsync(pr => pr.Project_Id == dto.Project_Id && pr.Is_Committed && pr.User.Role == UserRoleType.assistant);
            if (user.Role == UserRoleType.assistant && assistantExists) return BadRequest(new { message = "Assistant already exist for this project" });

            //if all validations pass register user for project
            var addUser = new Project_Register
            {
                Project_Id = dto.Project_Id,
                User_Id = dto.User_Id,
                Is_Committed = true
            };
            _db.Project_Register.Add(addUser);

            await _db.SaveChangesAsync();

            var type = Enum.TryParse<NotificationType>("ProjectJoined", true, out var parsed)
           ? parsed
           : NotificationType.ProjectJoined;

            await _db.Entry(addUser).Reference(x => x.Project).LoadAsync();


            //take the user name and 
            string strProjName = addUser.Project.Title;
            try
            {
                await _notificationService.NotifyUser(addUser.User_Id, "Joined Project", "You have successfully joined the project " + addUser.Project.Title + ". Please look out for the Project date - " + addUser.Project.Date, type);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to return the CBO or NPO Campus- Partnership might have lapsed",
                    error = ex.Message
                });
            }

            return Ok(new { message = "User successfully registered for the project" });

        }

        [HttpPost("projects-joined/{Volunteer_Id}")]
        public async Task<IActionResult> GetProjectJoined(int Volunteer_Id)
        {
            var registeredProjects = await _db.Project_Register.Where(u => u.User_Id == Volunteer_Id && u.Is_Committed == true).Select(p => new ProjectDto
            {
                Project_Id = p.Project.Project_id,
                Title = p.Project.Title,
                Type = p.Project.Type,
                Location = p.Project.Location,
                Partner = p.Project.Partner.ToString(),
                Partner_Name = p.Project.Partner_Name,
                Partner_Id = p.Project.Partner_Id,
                Category = p.Project.Category.ToString(),
                Hours = p.Project.Hours,
                Num_Volunteers = p.Project.Num_Volunteers,
                Description = p.Project.Description,
                Target_Group = p.Project.Target_Group,
                Status = p.Project.Status.ToString(),
                Date = p.Project.Date,
                Date_Created = p.Project.Date_Created,
                Time = p.Project.Time,
                //Slots = 
                Sdg = _db.Project_Sdg.Where(ps => ps.Project_Id == p.Project.Project_id).Select(s => s.Sdg_Id).ToArray()
            }).ToListAsync();


            if (registeredProjects.IsNullOrEmpty())
            {
                return BadRequest(new { message = "There are no joined Project - please join atleast one." });
            }
            return Ok(registeredProjects);
        }

        [HttpPatch("quit-project/project/{Project_Id}/Volunteer/{Volunteer_Id}")]
        public async Task<IActionResult> DeregisterFromProject(int Volunteer_Id, int Project_Id)
        {
            var registeredProject = await _db.Project_Register.Where(u => u.User_Id == Volunteer_Id  && u.Project_Id == Project_Id && u.Is_Committed == true).FirstOrDefaultAsync();
            if (registeredProject == null)
            {
                return NotFound(new { message = "User is not registered for this project" });
            }
            else
            {
                registeredProject.Is_Committed = false;
                await _db.SaveChangesAsync();
                return Ok(new { message = "You have been deregistered from this project" });
            }

        }

        [HttpPost("create-register")]
        public async Task<IActionResult> CreateAttendanceRegister([FromBody] AttendanceRegisterDto dto)
        {
            var regAssistant = await _db.Project_Register.Where(pr => pr.User_Id == dto.Assistant_Id && pr.Project_Id == dto.Project_Id).FirstOrDefaultAsync();
            if (regAssistant == null) { return BadRequest(new { message = "Something went wrong while retriving project and assistant details - the project is might not active" }); }
            //check if the project date is correct and due
            var project = await _db.Project.FindAsync(dto.Project_Id); if (project == null) return BadRequest(new { message = "Project not found" });
            if (project.Date != DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest(new { message = "Wait for project's set date to create attenndance" });
            }

            var attendance = new Attendance_Register
            {
                Project_Id = dto.Project_Id,
                Assistant_Id = dto.Assistant_Id,
                Created_At = DateTime.Now,
                Checkinqr_Token = dto.CheckInQr_Token,
                Checkoutqr_Token = null,
            };

            _db.Attendance_Register.Add(attendance);

            await _db.SaveChangesAsync();


            return Ok(new { attendance.Attendance_Id, attendance.Checkinqr_Token });
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto)
        {

            var attendance = await _db.Attendance_Register.FirstOrDefaultAsync(ar => ar.Attendance_Id == dto.Attendance_Id && ar.Checkinqr_Token == dto.CheckInQr_Token);

            if (attendance == null) { return BadRequest(new { message = "Invalid QR Code" }); }

            // no duplicate check-in
            var alreadyChecked = await _db.Attendance_Entry.AnyAsync(ar => ar.Attendance_Id == dto.Attendance_Id && ar.User_Id == dto.Volunteer_id && ar.Checkout_Time == null);
            if (alreadyChecked)
            { return BadRequest(new { message = "Volunteer already checked in" }); }

            var entry = new Attendance_Entry
            {
                Attendance_Id = dto.Attendance_Id,

                User_Id = dto.Volunteer_id,

                Checkin_Time = DateTime.UtcNow
            };


            _db.Attendance_Entry.Add(entry);

            await _db.SaveChangesAsync();


            return Ok(new { message = "Check in successful" });
        }

        [HttpGet("present/{Attendance_Id}")]
        public async Task<IActionResult> CurrentVolunteers(int Attendance_Id)
        {

            var registerList = await _db.Attendance_Entry.Where(ae => ae.Attendance_Id == Attendance_Id && ae.Checkout_Time == null).Include(u => u.User).Select(v => new
                {
                    v.User.User_Id,
                    v.User.First_Name,
                    v.User.Last_Name,
                    v.Checkin_Time
                }).ToListAsync();

            return Ok(registerList);
        }

        [HttpPost("create-checkoutqr")]
        public async Task<IActionResult> OpenCheckout([FromBody] CheckOutDto dto)
        {

            var attendance = await _db.Attendance_Register.Include(ar=>ar.Project).FirstOrDefaultAsync(ar=>ar.Attendance_Id == dto.Attendance_Id);

            if (attendance == null) { return NotFound(new { message = "Attendance Register linked to project not found" }); }

            attendance.Checkoutqr_Token = dto.CheckOutQr_Token;
            attendance.Project.Status = ProjectStatus.Completed;
            await _db.SaveChangesAsync();



            return Ok(new { message = "Check out code created successful" });
        }
        [HttpPost("manual-attendance")]
        public async Task<IActionResult> AddAttendanceManually([FromBody] ManualAttendaceDto dto)
        {
            var volunteeerRegistered = await _db.Volunteer.FirstOrDefaultAsync(v => v.User.Email == dto.Email);
            var findRegister = await _db.Attendance_Entry.FirstOrDefaultAsync(ae => ae.Attendance_Register.Project_Id == dto.Project_Id);
            if (findRegister == null) return BadRequest(new { message = "Entry for this project not found" });
            if(volunteeerRegistered != null)
            {
                var entry = new Attendance_Entry
                {
                    Attendance_Id = findRegister.Attendance_Id,
                    User_Id = volunteeerRegistered.Volunteer_Id,
                    Checkin_Time = dto.Checkin_Time,
                    Checkout_Time = findRegister.Checkout_Time,
                    
                };
               entry.Hours_Logged = Math.Round((entry.Checkout_Time.Value - entry.Checkin_Time).TotalHours,2);
                volunteeerRegistered.Hours = Math.Round(volunteeerRegistered.Hours + (double)entry.Hours_Logged, 2);
                _db.Attendance_Entry.Add(entry);
                // volunteeerRegistered.Hours += dto.Hours_Logged;

            }
            else
            {

                Enum.TryParse<CampusCode>(dto.Campus_Code, true, out var campusCode);
            
               var campus = await _db.Campus.Where(c => c.Code.Equals(campusCode)).FirstOrDefaultAsync();

                var unregistered = new Unregistered_Volunteer
                {
                    Email = dto.Email,
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Campus_Id = campus.Campus_Id,
                    Hours = Math.Round((dto.Checkout_Time.Value - dto.Checkin_Time).TotalHours, 2)
                };
                _db.Unregistered_Volunteer.Add(unregistered);
            }
           
            await _db.SaveChangesAsync();
            return Ok(new { message = "Added unregistered volunteers for both system and project" });
        }
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutDto dto)
        {

            var attendance = await _db.Attendance_Register.FirstOrDefaultAsync(ar => ar.Attendance_Id == dto.Attendance_Id && ar.Checkoutqr_Token == dto.CheckOutQr_Token);


            if (attendance == null) { return BadRequest(new { message = "Invalid QR Code" }); }

            var entry = await _db.Attendance_Entry.FirstOrDefaultAsync(ar => ar.Attendance_Id == dto.Attendance_Id && ar.User_Id == dto.User_id && ar.Checkout_Time == null);

            if (entry == null) { return BadRequest(new { message = "No active attendance found" }); }

            entry.Checkout_Time = DateTime.UtcNow;

            entry.Hours_Logged = Math.Round((entry.Checkout_Time.Value - entry.Checkin_Time).TotalHours,2);
            var volunteer = await _db.Volunteer.FirstOrDefaultAsync(v => v.Volunteer_Id == dto.User_id);

            if (volunteer == null) { return NotFound(new {message = "Volunteer not found"}); }
            volunteer.Hours = Math.Round(
            volunteer.Hours + (double)entry.Hours_Logged,2);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                entry.Hours_Logged
            });
        }

        [HttpPost("retrieve-checks")]
        public async Task<IActionResult> RetrieveAttendances()
        {
            var attendances = await _db.Attendance_Register.ToListAsync();
            if (attendances.IsNullOrEmpty()) return NotFound(new { message = "There are no attendance register at this momemnt" });
            return Ok(attendances);
        }
    }
}