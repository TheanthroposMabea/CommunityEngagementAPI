using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using Microsoft.AspNetCore.OutputCaching;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly CEAppContext _db;
        public UsersController(CEAppContext db)
        {
            _db = db;
        }
        //get all the users in the system
        [HttpGet("Users")]
        [OutputCache(Duration = 900)]
        public async Task<ActionResult<UserDto>> GetAllUsers()
        {
            var allCampus = await _db.Campus.ToListAsync();
            //convert each campus code to string and compare to the one from the user
            foreach(var campus in allCampus)
            {
                Console.WriteLine(campus.Name + campus.Code);
            }
            var users = await _db.User.Select(u => new UserDto()
            {
                User_Id = u.User_Id,
                First_Name = u.First_Name,
                Last_Name = u.Last_Name,
                Email = u.Email,
                Phone = u.Phone,
                User_Role = u.Role.ToString(),
                Faculty_Code = _db.Faculty.Where(f => f.Faculty_Id == u.Faculty_Id).Select(f => f.Code).FirstOrDefault(),
                Is_Active = u.Is_Active.ToString(),
                Campus_Code = _db.Campus.Where(c => c.Campus_Id == u.Campus_Id).Select(c => c.Code).FirstOrDefault().ToString(),




            }).ToListAsync();

            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound(new { message = "Something went wrong while trying to query Users, users might not be available" });
            }
            //var users = from u in _db.User
            //            select new UserDto()
            //            {
            //                User_Id = u.User_Id,
            //                First_Name = u.First_Name,
            //                Last_Name = u.Last_Name,
            //            };
            // return Ok(users.)
        }


        //get the specific user details 
        [HttpGet("{User_Id}")]
        [OutputCache(Duration = 900)]
        public async Task<ActionResult<UserDto>> GetUser(int User_Id)
        {
            var user = await _db.User.Where(u => u.User_Id == User_Id).Select(u => new UserDto()
            {
                First_Name = u.First_Name,
                Last_Name = u.Last_Name,
                Email = u.Email,
                Phone = u.Phone,
                Is_Active = u.Is_Active.ToString(),
                User_Role = u.Role.ToString(),
                Faculty_Code = _db.Faculty.Where(f => f.Faculty_Id == u.Faculty_Id).Select(f => f.Code).FirstOrDefault(),

                Campus_Code = _db.Campus.Where(c => c.Campus_Id == u.Campus_Id).Select(c => c.Code).FirstOrDefault().ToString(),

            }).FirstOrDefaultAsync();
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound(new { message = "Something went wrong - User might not exist" });
            }
        }

        //get the user by their role 
        [HttpGet("role/{Role}")]
        [OutputCache(Duration = 900)]
        public async Task<ActionResult<UserDto>> GetUsersByRole(string Role)
        {
            if (!Enum.TryParse<UserRoleType>(Role, true, out var userRole))
            {
                return BadRequest("Invalid role.");
            }

            var usersInRole = await _db.User.Where(u => u.Role.Equals(userRole)).Select(u => new UserDto()
            {
                User_Id = u.User_Id,
                First_Name = u.First_Name,
                Last_Name = u.Last_Name,
                Email = u.Email,
                Phone = u.Phone,
                Is_Active = u.Is_Active.ToString(),
                Faculty_Code = _db.Faculty.Where(f => f.Faculty_Id == u.Faculty_Id).Select(f => f.Code).FirstOrDefault(),
                Campus_Code = _db.Campus.Where(c => c.Campus_Id == u.Campus_Id).Select(c => c.Code).FirstOrDefault().ToString(),
            }).ToListAsync();
            if (!usersInRole.IsNullOrEmpty())
            {
                return Ok(usersInRole);
            }
            else
            {
                return NotFound(new { message = "Something went wrong while retriving Users in -" + Role });
            }
        }


        //get the volunteer by volunter type
        [HttpGet("role/volunteer/{Volunteer_Type}")]
        [OutputCache(Duration = 900)]
        public async Task<ActionResult<VolunteerDto>> GetVolunteersByType(string Volunteer_Type)
        {
            if (!Enum.TryParse<VolunteerType>(Volunteer_Type, true, out var volunteerType))
            {
                return BadRequest("Volunteer type could not be parsed - otr might not exist");
            }
            var volunteersInType = await _db.Volunteer.Where(v => v.Volunteer_Type.Equals(volunteerType)).Select(v => v.Volunteer_Id).ToListAsync();
            if (volunteersInType.IsNullOrEmpty())
            {

                return NotFound(new { message = "Something went wrong while trying to retrieve Volunteers of type - " + Volunteer_Type });

            }
            var volunteerUsers = await _db.User.Where(u => volunteersInType.Contains(u.User_Id)).Select(u => new VolunteerDto()
            {
                First_Name = u.First_Name,
                Last_Name = u.Last_Name,
                Email = u.Email,
                Phone = u.Phone,
                Faculty_Code = _db.Faculty.Where(f => f.Faculty_Id == u.Faculty_Id).Select(f => f.Code).FirstOrDefault(),
                Volunteer_Type = Volunteer_Type,
                Is_Completed = _db.Volunteer.Where(v => v.Volunteer_Id == u.User_Id).Select(v => v.Is_Completed).FirstOrDefault(),
                Hours = _db.Volunteer.Where(v => v.Volunteer_Id == u.User_Id).Select(v => v.Hours).FirstOrDefault(),
                Geo_Area = _db.User_Demographics.Where(v => v.User_Id == u.User_Id).Select(v => v.Geo_Area).FirstOrDefault(),
                Gender = _db.User_Demographics.Where(v => v.User_Id == u.User_Id).Select(v => v.Gender.ToString()).FirstOrDefault(),
                Nationality = _db.User_Demographics.Where(v => v.User_Id == u.User_Id).Select(v => v.Is_International).FirstOrDefault(),
            }).ToListAsync();

            if (!volunteerUsers.IsNullOrEmpty())
            {
                return Ok(volunteerUsers);
            }
            else
            {
                return NotFound(new { message = "Something went wrong while trying to retrieve Volunteers  - " + volunteerUsers });
            }

        }

        //allow the user to make big changes by verifying their password first
        [HttpPost("verify-password")]
        public async Task<IActionResult> VerifyPassword([FromBody] LoginDto verifyDto)
        {
            var user = await _db.User.Where(u => u.Email.ToLower().Trim() == verifyDto.Email.ToLower().Trim()).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found - re enter email or register", loggedin = false, notexist = true });

            }
            if (user.Password.Trim() != Secrecy.HashPassword(verifyDto.Password.Trim()))
            {
                return BadRequest(new { message = "User password incorrect", loggedin = false, passwrong = true });

            }
            else
            {
                return Ok(new { message = "Verified successfully", verified = true,  });
            }

        }

        //volunteer should update their profiles
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var student = await _db.Student.FirstOrDefaultAsync(s => s.Student_Id == dto.User_Id);
            if(student != null)
            {
                Enum.TryParse<YearOfStudy>(dto.Year_Of_Study, true, out var StudyYear);

                student.Student_Number = dto.Student_Number;
                student.Qualification = dto.Qualification;
                student.Department = dto.Department;
                student.Year_Of_Study = StudyYear;

                await _db.SaveChangesAsync();

            }else if(student == null)
            {
                var alumni = await _db.Alumni.FirstOrDefaultAsync(s => s.Alumni_Id == dto.User_Id);

                alumni.Year_Graduated = dto.Year_Graduated;
                alumni.Department = dto.Department;

                await _db.SaveChangesAsync();

            }

            return Ok(new { message = "Profile Updated" });
        }
    }
}
