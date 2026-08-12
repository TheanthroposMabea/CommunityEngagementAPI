using CommunityEngagementAPI.Helpers;
using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Security;
using StudentApp.API.Services;
using System.Data;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;


namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        //create  db and email service
        private readonly CEAppContext _db;
        private readonly EmailService _emailService;
        private readonly INotificationService _notificationService;

        public AuthController(CEAppContext db, EmailService emailService,INotificationService notificationService)
        {
            _db = db;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        //send an otp to a user email
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDto otpDto)
        {
            var existingUser = await _db.User.Where(u => u.Email.Trim().Equals(otpDto.Email.Trim())).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return Conflict(new { message = "User already exist", exist = true });
            }
            //generate random 6 digit otp and store it temporarily
            var otp = new Random().Next(100000, 999999).ToString();
            TempStorage.Otp[otpDto.Email.Trim()] = (otp, DateTime.Now.AddMinutes(5));
            try
            {
                //send email to user
                await _emailService.SendOTP(otpDto.Email, otp);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new
                {
                    message = "Failed to send OTP email",
                    error = ex.Message
                });
            }
            return Ok(new { message = "OTP sent successfully to " + otpDto.Email, sent = true });
        }

        //verify the otp sent 
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto verifyDto)
        {
            if (!TempStorage.Otp.ContainsKey(verifyDto.Email.Trim()))
                return BadRequest(new { message = "No OTP found for this email" });

            var (storedOtp, expiry) = TempStorage.Otp[verifyDto.Email.Trim()];

            if (DateTime.Now > expiry)
                return BadRequest(new { message = "OTP has expired" });

            if (storedOtp != verifyDto.Otp)
                return BadRequest(new { message = "Invalid OTP" });


            TempStorage.Otp.Remove(verifyDto.Email.Trim());

            return Ok(new { message = "OTP verified", verified = true });
        }

        //register user - assistant, volunteers

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            /*  Convert string to enum when using
              if (!Enum.TryParse<CampusCode>(registerDto.Campus_Code, true, out var campusCode))
              {
                  return BadRequest(new { message = "could not parse" });

              }
               var campus = await _db.Campus.Where(c => c.Code.Equals(campusCode)).FirstOrDefaultAsync();
            */
            var existingUser = await _db.User.Where(u => u.Email == registerDto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("User already exist in the system , try to Login");
            }
            // retrun all the campus codes
            var allCampus = await _db.Campus.ToListAsync();
            //convert each campus code to string and compare to the one from the user
            var campus = allCampus.FirstOrDefault(c => c.Code.ToString().Trim() == registerDto.Campus_Code.Trim());

            //valiodate non existing campus
            if (campus == null)
            {
                Console.WriteLine("Campus code not there");
                return BadRequest(new { message = "Campus code does not exist" });
            }
            //create a user faculty 
            var faculty = await _db.Faculty.Where(f => f.Code.Trim() == registerDto.Faculty_Code.Trim()).FirstOrDefaultAsync();
            if (faculty == null)
            {
                Console.WriteLine(registerDto.Faculty_Code + "Faculty not found");
                return NotFound(new { message = " Faculty not found" });
            }

            //_db.Faculty.Add(faculty);
            //await _db.SaveChangesAsync();
            //Console.WriteLine("Faculty is saved to db successfuly");
            //   string[] sdg = new string[3];



            //create a user
            var user = new User
            {
                First_Name = registerDto.First_Name,
                Last_Name = registerDto.Last_Name,
                Email = registerDto.Email.Trim(),
                Password = Secrecy.HashPassword(registerDto.Password.Trim()),
                Is_Active = true,
                Phone = registerDto.Phone,
                Id_Number = registerDto.Id_Number,
                Date_Created = DateTime.Now,
                Campus_Id = campus.Campus_Id,
                Faculty_Id = faculty.Faculty_Id,
                Role = UserRoleType.volunteer
            };
            //add user to the db
            _db.User.Add(user);
            await _db.SaveChangesAsync();
            if (!Enum.TryParse<VolunteerType>(registerDto.Volunteer_Role, true, out var volunteerRole))
            {
                return BadRequest("Invalid Volunteer role - could not be parsed.");
            }
            if (user.Role == UserRoleType.volunteer)
            {
            var volunteer = new Volunteer
            {
                Volunteer_Id = user.User_Id,
                Volunteer_Type = volunteerRole,
                Is_Completed = false,
                Hours = 0

            };
            if (registerDto.Sdg != null && registerDto.Sdg.Length > 0)
            {
                foreach (var sdgId in registerDto.Sdg)
                {
                    volunteer.VolunteerSdgs.Add(new Volunteer_Sdg
                    {
                        Sdg_Id = sdgId
                    });
                }
                //   return BadRequest("Sdgs are missing you can not register");
            }

            //make a user a volunteer
            _db.Volunteer.Add(volunteer);
            await _db.SaveChangesAsync();
                //register a student 
            if (volunteer.Volunteer_Type == VolunteerType.Student)
            {
                var student = new Student
                {
                    Student_Id = volunteer.Volunteer_Id,
                    

                };
                    _db.Student.Add(student);
            } else if (volunteer.Volunteer_Type == VolunteerType.Alumni)
            {
                    //register an alumni
                var alumni = new Alumni
                {
                    Alumni_Id = volunteer.Volunteer_Id,
                };
                    _db.Alumni.Add(alumni);
            }
           
                //create volunteer demographics
                var userdemographics = new User_Demographics
                {
                    User_Id = user.User_Id,
                    Geo_Area = registerDto.Geo_Area,
                    Home_Language = registerDto.Home_Language,
                    Gender = registerDto.Gender

                };
                if (registerDto.Is_International == "Yes") { userdemographics.Is_International = true; }
                else userdemographics.Is_International = false;

                _db.User_Demographics.Add(userdemographics);
                await  _db.SaveChangesAsync();
                    //send an email to  the new user
                    var specialistEmail = await _db.User.Where(u => u.Campus_Id == campus.Campus_Id && u.Role == UserRoleType.specialist).Select(u => u.Email).FirstOrDefaultAsync();

            if (specialistEmail == null) { return NotFound("Specialist email not found for this campus"); }

            await _emailService.WelcomeEmail(user.First_Name, user.Email, campus.Name, specialistEmail);
        }else if (user.Role == UserRoleType.staff)
            {
                var staff = new Staff
                {
                    Staff_Id = user.User_Id,
                    Department = registerDto.Department
                };
            }
               
            
            return Ok(new { message = "User registered successfully", registered = true });

            //save the user to the database

        }

        //user login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _db.User.Where(u => u.Email.ToLower().Trim() == loginDto.Email.ToLower().Trim()).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found - re enter email or register", loggedin = false, notexist = true });


            }
            Console.WriteLine("Stored hash: " + user.Password);
            Console.WriteLine("Login hash: " + Secrecy.HashPassword(loginDto.Password));

            //user exist but has put incorrect password

            if (user.Password.Trim() != Secrecy.HashPassword(loginDto.Password.Trim()))
            {
                return BadRequest(new { message = "User password incorrect", loggedin = false, passwrong = true });
            }else if(user.Is_Active == false)
            {
                return BadRequest(new { message = "This account is deactivated" });
            }

                Console.WriteLine(user.Role);
            string userRole = user.Role.ToString();
            //user registered && is logging in
            return Ok(new { message = "Logged in successfully", loggedin = true, role = userRole, user_id = user.User_Id,id = user.Id_Number, email = user.Email});
        }

        //reset password link
        [HttpPost("send-reset-link")]
        public async Task<IActionResult> SendResetPassLink([FromBody] SendLinkDto sendlinkDto)
        {
            var existingUser = await _db.User.Where(u => u.Email == sendlinkDto.Email).FirstOrDefaultAsync();
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found or does not exist" });

            }

            var token = new Random().Next(100000, 999999).ToString();
            TempStorage.Otp[sendlinkDto.Email] = (token, DateTime.Now.AddMinutes(5));

            await _emailService.sendLink(sendlinkDto.Email, token);
            Console.WriteLine("Email " + sendlinkDto.Email + "Token " + token);
            return Ok(new { message = "Token sent to email", sent = true });
        }

        //verify the link 
        [HttpPost("verify-link")]
        public IActionResult VerifyResetLink([FromBody] VerifyLinkDto verifylinkDto)
        {
            if (!TempStorage.Otp.ContainsKey(verifylinkDto.Email.Trim()))
            {
                return BadRequest(new { message = "No such email for the reset password request", notexist = true });
            }
            var (Storedtoken, expiry) = TempStorage.Otp[verifylinkDto.Email.Trim()];
            if (DateTime.Now > expiry)
            {
                return BadRequest(new { message = "The link has expired, try requesting a new one", expired = true });
            }

            if (Storedtoken != verifylinkDto.Token)
            {
                return BadRequest(new { message = "Invalid reset password link token for the email", notfound = true });

            }
            return Ok(new { message = "You can successfully reset you pasword", verified = true });
        }

        //reset the user password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            
            var systemUser = await _db.User.Where(u=> u.Email.Trim() == resetPasswordDto.Email.Trim()).FirstOrDefaultAsync();

            if (systemUser == null)
            {
                return NotFound(new { message = $"{resetPasswordDto.Email} + {resetPasswordDto.Password}", notfound = true });
            }
            systemUser.Password = Secrecy.HashPassword(resetPasswordDto.Password.Trim());
            Console.WriteLine("Reset hash: " + systemUser.Password);
          //  systemUser.Password = Secrecy.HashPassword(resetPasswordDto.Password);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Password changed successfully", reset =  true});
        }

        //director registors a new specialist
        [HttpPost("register-specialist")]
        public async Task<IActionResult> RegisterSpecialist([FromBody] RegisterSpecialistDto registerSpecialistDto)
        {
            var specialist = await _db.User.Where(u => u.Email.Trim() == registerSpecialistDto.Email.Trim() || u.Id_Number == registerSpecialistDto.Id && u.Role == UserRoleType.specialist).FirstOrDefaultAsync();
            
            if (specialist == null || specialist.Role != UserRoleType.director)
            {
                //fetch all the campuses
                var allcampus = await _db.Campus.ToListAsync();
                //find the speciaist dinstict campus
                var campus = allcampus.Where(c => c.Code.ToString() == registerSpecialistDto.Campus).FirstOrDefault();
                if(campus == null)
                {
                    Console.WriteLine("Campus code not there");
                    return NotFound(new {messsage = "Campus code does not exist", campusnotfound = true});
                
                }
                if (!Enum.TryParse<CampusCode>(registerSpecialistDto.Campus, true, out var campusCode))
                {
                    return BadRequest(new { message = "could not parse" });

                }
              
               // var campus = await _db.Campus.Where(c => c.Code.Equals(campusCode)).FirstOrDefaultAsync();
                var existinmgSpecialist = await _db.User.Where(u => u.Role == UserRoleType.specialist && u.Campus.Code.Equals(campusCode) ).FirstOrDefaultAsync();
                if (existinmgSpecialist != null && existinmgSpecialist.Is_Active == true)
                {
                    return BadRequest(new { message = "There is already a Specialist for this Campus" });
                }

               var newSpecialist = new User
                {
                    First_Name = registerSpecialistDto.First_Name,
                    Last_Name = registerSpecialistDto.Last_Name,
                    Email = registerSpecialistDto.Email,
                    Password = Secrecy.HashPassword("specialistPass"),
                    Is_Active = true,
                    Id_Number = registerSpecialistDto.Id,
                    Phone =  registerSpecialistDto.Phone,
                    Role = UserRoleType.specialist,
                    Is_Suspended = false,
                    Date_Created = DateTime.Now,
                    Campus_Id = campus.Campus_Id,

                };
                _db.User.Add(newSpecialist);
                await _db.SaveChangesAsync();
                Console.WriteLine("Specialist registered successfully");

                //allow the specialist to reset the password
                try
                {
                    SendLinkDto sendLink = new SendLinkDto();
                    sendLink.Email = registerSpecialistDto.Email;

                    await SendResetPassLink(sendLink);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " couldn not send link ");
                    return BadRequest(new { message = "Could not send link to specialist register again" });
                }

                return Ok(new { message = "Specialist registered successfully", specialistReg = true });
               

            }
            Console.WriteLine("Specialist already exist");
            return BadRequest(new { message = "Sorry specialist already registred in the system", exist = true });
        }
        
        //suspend the user account
        [HttpPost("suspend-user")]
        public async Task<IActionResult> suspendUser([FromBody] SuspendDto suspendDto)
        {
            var systemUser = await _db.User.Where(u => u.Email == suspendDto.Email).FirstOrDefaultAsync();
            if (systemUser == null || systemUser.Role == UserRoleType.director)
            {
                return NotFound(new { message = "User does not exist or User is a Director" });
            }
           
            if(systemUser.Is_Active == true && systemUser.Is_Suspended ==false)
            {
                systemUser.Is_Suspended = true;
               
               var suspension =  new Suspension
                {
                    Start_Date = DateOnly.FromDateTime(DateTime.Now),
                    End_Date = DateOnly.FromDateTime(DateTime.Now).AddDays(suspendDto.Duration),
                    Is_Active = true,
                    User_Id = systemUser.User_Id

                };
                //save the changes to the db
                _db.Suspension.Add(suspension);
                await _db.SaveChangesAsync();

                // notify the user that they are suspended
                await _notificationService.NotifyUser(
                    systemUser.User_Id, "Account Suspended",
                    "Your account has been suspended until "+ suspension.End_Date + ". You won't be able to join project util then. Contact your campus specialist.", NotificationType.AccountSuspension);

                //send the message to the suspender
                return Ok(new { message = systemUser.Role.ToString().ToUpper() + " " + systemUser.Last_Name + " is Suspended", suspended = true });
            }
            return StatusCode(500, new
            {
                message = "User account inactive or already suspended"
            });
           

        }

        //make a volunteer an assistant
        [HttpPost("make-assistant")]
        public async Task<IActionResult> makeAssistant([FromBody] EmailDto emailDto)
        {
            var volunteer = await _db.User.Where(u => u.Email == emailDto.Email).FirstOrDefaultAsync();
            if(volunteer.Role != UserRoleType.volunteer)
            {
                return BadRequest(new { message = " User not Volunteer, can not be made an Assistant" });
            }

            if (volunteer.Is_Active == true && volunteer.Is_Suspended == false )
            {
                volunteer.Role = UserRoleType.assistant;
                await _db.SaveChangesAsync();
                await _notificationService.NotifyUser(volunteer.User_Id, "Role Change",
              "You have been promoted to being an Assistant. Thank you for you hard work. Please confirm this by checking your access privileges.", NotificationType.RoleChanged);
                return Ok(new { message = volunteer.Email + " has a new Assistant role", madeAssistant = true });
            }
            
            //send the notification to user
          
          
            return NotFound(new { message = "User does not exist, has inActive account or Suspended" });
        }

        //deactive & active the volunteer or assistant acccount 
        [HttpPost("deactive-account")]
        public async Task<IActionResult> accountActivation([FromBody] EmailDto emailDto)
        {
            var systemUser = await _db.User.Where(u => u.Email == emailDto.Email).FirstOrDefaultAsync();
            if(systemUser == null)
            {
                return NotFound(new { message = "User account does not exist, try again" });
            }
            if (systemUser.Is_Active == true)
            {
                systemUser.Is_Active = false;
                await _db.SaveChangesAsync();


                //send the email to the user concerning the change
                await _emailService.ActivationUpdate(emailDto.Email, "Activated");
                return Ok(new { message = " User account has been deactivated", deactivated = true });
            }
            else if (systemUser.Is_Active == false)
            {
                systemUser.Is_Active = true;
                await _db.SaveChangesAsync();

                //send the email to the user concerning the change
                await _emailService.ActivationUpdate(emailDto.Email, "Deactivated");
                return Ok(new { message = " User account has been reactivated, you now access the system", reactivated = true });

            }
            else
            {
                return StatusCode(500, "Something went really wrong while trying to change account activation");
            }
            

        }
            


        }
}