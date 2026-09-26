using CommunityEngagementAPI.Helpers;
using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using StudentApp.API.Services;
using System;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NGOController : ControllerBase
    {
        private readonly CEAppContext _db;
        private EmailService _emailService;
        private INotificationService _notificationService;

        public NGOController(CEAppContext db, EmailService emailService, INotificationService notificationService)
        {
            _db = db;
            _emailService = emailService;
            _notificationService = notificationService;
        }
        // handle the ngo apllications
        [HttpPost("ngo-application")]
        public async Task<IActionResult> handleNGOApplication(NGOApplicationDto ngoDto,[FromServices] IOutputCacheStore cacheStore)
        {
            var campuses = await _db.Campus.ToListAsync();
            var campus = campuses.Where(c => c.Code.ToString() == ngoDto.ClosestCampus).FirstOrDefault();
            if (campus == null)
            {
                return NotFound(new { message = "Campus location closest not found" });
            }
            var specialist = await _db.User.Where(u => u.Campus_Id == campus.Campus_Id && u.Role == UserRoleType.specialist).FirstOrDefaultAsync();
            if (specialist == null)
            {
                return NotFound(new { message = "Something went wrong while trying to get Campus closest to and Specialist" });
            }
            var application = await _db.NGO_Application.Where(U => U.Reg_Number == ngoDto.Reg_Number).FirstOrDefaultAsync();
            if (application == null)
            {
                var NGO_App = new NGO_Application
                {
                    Reg_Number = ngoDto.Reg_Number,
                    Name = ngoDto.Name,
                    Email = ngoDto.Email,
                    Close_Campus = ngoDto.ClosestCampus,
                    Address = ngoDto.Address,
                    Rep_Name = ngoDto.Rep_Name,
                    Rep_Phone = ngoDto.Rep_Phone,
                    Rep_Position = ngoDto.Rep_Position,
                    Key_Focus = ngoDto.Key_Focus,
                    Collaboration_Request = ngoDto.Collaboration_Request,
                    Benefits = ngoDto.Benefits,
                    Description = ngoDto.Description,
                    Status = Status.PendingReview,
                    Is_Viewed = false,
                    Date_Submitted = DateTime.Now,
                    Website = ngoDto.Website,
                    Specialist_Id = specialist.User_Id
                };

                if (ngoDto.Sdg != null && ngoDto.Sdg.Length > 0)
                {
                    foreach (var sdgId in ngoDto.Sdg)
                    {
                        NGO_App.NgoSdgs.Add(new NGO_Sdg
                        {
                            Sdg_Id = sdgId
                        });
                    }
                    //   return BadRequest("Sdgs are missing you can not register");
                }
                _db.NGO_Application.Add(NGO_App);
                await _db.SaveChangesAsync();
                await cacheStore.EvictByTagAsync("status-tag", default);

                Document doc;
                foreach (var file in ngoDto.Files)
                {
                    if (file == null || file.Length == 0)
                    {
                        return BadRequest("No file was uploaded.");
                    }
                    var fileStream = new MemoryStream();
                    await file.CopyToAsync(fileStream);

                    doc = new Document
                    {
                        Name = file.FileName,
                        Type = file.ContentType,
                        Size = (byte)file.Length,
                        Data = fileStream.ToArray(),
                        Entity_Type = "NGO_Application",
                        Entity_Id = NGO_App.Application_Id,
                        Date_Uploaded = DateTime.Now
                    };
                    _db.Document.Add(doc);
                    await _db.SaveChangesAsync();
                    // await cacheStore.EvictByTagAsync("status-tag", default);
                }




                return Ok(new { message = "Your Application Was Successfully Submitted", applied = true });
            }
            else { return BadRequest(new { message = "Application already exists", applied = false }); }

            //return BadRequest(new { message = "Something went wrong while applying, try again later", applied = false });
        }
        //get all the NGO applications
        [HttpGet("applications")]
       [OutputCache(PolicyName = "StatusPolicy")]
        public async Task<IActionResult> GetAllApplications()
        {
            var applications = await _db.NGO_Application.OrderByDescending(a=>a.Date_Submitted).ToListAsync();
            if(applications != null)
            {
                return Ok(applications);
            }
            else
            {
                return NotFound(new { message = "Could not retrieve the application, might be empty!" });
            }
        }

        //get application by its ngos registaion number for director
        [HttpGet("getcampusapplications/specialist/{Id}/application/{Reg_Number}/")]
        [OutputCache(PolicyName = "StatusPolicy")]
        public async Task<IActionResult> GetSpecificApplication(string Reg_Number, int Id )
        {
            var application = await _db.NGO_Application.Where(u => u.Reg_Number == Reg_Number && u.Specialist_Id == Id).FirstOrDefaultAsync();
                if(application != null)
                {
                    return Ok(application);
                }
                else
                {
                    return NotFound(new { message = "Something went wrong , the application is not found" });
                }
            
        }

        //get the applications by status
        [HttpGet("status/{Status}")]
        [OutputCache(PolicyName = "StatusPolicy")]
        public async Task<IActionResult> GetApplicationByStatus(string Status)
        {
            if (!Enum.TryParse<Status>(Status, true, out var appStatus))
            {
                return BadRequest("Invalid role.");
            }

            var ApplicationsInStatus = await _db.NGO_Application.Where(u => u.Status.Equals(appStatus)).OrderByDescending(a=>a.Date_Submitted).ToListAsync();
            if (ApplicationsInStatus != null)
            {
                return Ok(ApplicationsInStatus);
            }
            else
            {
                return NotFound(new { message = "Something went wrong" });
            }

        }

        //update the ngo status - approve or reject - this is for the director only
        [HttpPost("update-status")]
        
        public async Task<IActionResult> UpdateApplicationStatus([FromBody]UpdateStatusDto ngoDto, [FromServices] IOutputCacheStore cacheStore)
        {
            var application = await _db.NGO_Application.Where(u => u.Reg_Number == ngoDto.Reg_Number).FirstOrDefaultAsync(); 
            if(application == null)
            {
                return NotFound(new { messsage = "Applicaton not found" });
                
            }

            Enum.TryParse<Status>(ngoDto.Status, true, out var ngoStatus);
            application.Status = ngoStatus;
            await _db.SaveChangesAsync();
            //
             await cacheStore.EvictByTagAsync("status-tag", default);
             //
            if (application.Status == Status.Approved)
            {
                var partnership = new NGO_Partnership
                {
                    Start_Date = DateTime.Now,
                    Renewal_Date = DateTime.Now.AddMonths(12),
                    Is_Active =true,
                    Notes = application.Collaboration_Request,
                    Application_Id = application.Application_Id
                };
                _db.Add(partnership);
                await _db.SaveChangesAsync();
            }
                await _emailService.statusUpdateEmail(application.Email, application.Status);
            //send the email to the ngio
            return Ok(new {message = "Application status changed"});
        }

        //the specialist review the ngo application
        [HttpPost("specialist-review")]
        public async Task<IActionResult> applicationReview([FromBody]UpdateStatusDto reviewStatusDto,[FromServices] IOutputCacheStore cacheStore)
        {
            var application = await _db.NGO_Application.Where(a => a.Reg_Number == reviewStatusDto.Reg_Number).FirstOrDefaultAsync();
            //get speacialis
            var specialist = await _db.User.Where(u => u.Id_Number == reviewStatusDto.Id && u.Role == UserRoleType.specialist).FirstOrDefaultAsync();
           
            if (application == null || specialist == null)
            {
                return NotFound(new { messsage = "Applicaton review was not successful - wrong speacialist or void application" });

            }

            //get the campus closest to ngo
            var allCampuses = await _db.Campus.ToListAsync();
            var ngoCampus = allCampuses.Where(c => c.Code.ToString() == application.Close_Campus).FirstOrDefault();
            
            if(ngoCampus == null || ngoCampus.Campus_Id != specialist.Campus_Id)
            {
                return NotFound(new { messsage = "Applicaton review was not successful - wrong speacialist or void application" });

            }
            else
            {
                Enum.TryParse<Status>(reviewStatusDto.Status, true, out var ngoStatus);
                application.Status = ngoStatus;
                application.Is_Viewed = true;
                await _db.SaveChangesAsync();
                await cacheStore.EvictByTagAsync("status-tag", default);
                 if (application.Status == Status.MissingInformation)
                {
                    var token = new Random().Next(100000, 999999).ToString();
                    TempStorage.DocumentUpload[application.Email] = (token, DateTime.Now.AddDays(7), application.Application_Id);

                    await _emailService.sendReuploadLink(application.Email, token, application.Application_Id);
                    Console.WriteLine("Email " + application.Email + "Token " + token + "ApplicationId " + application.Application_Id);
                    return Ok(new { message = "Reupload token sent to email", sent = true });
                }
                 //parse the notification type enum
                var type = Enum.TryParse<NotificationType>("NGOStatus", true, out var parsed)
             ? parsed
             : NotificationType.NGOStatus;
                //send the notification to the director about the Reviewed Application
                var  director = await _db.User.FindAsync(1); if(director == null) { return NotFound("Director not found"); }
                //use the interface 
              await  _notificationService.NotifyUser(director.User_Id, "New Application to Approve", specialist.Campus.Code + " Specialist " + specialist.Last_Name + " has reviewed the application from " + application.Name +" waiting for approval", type);
                return Ok(application);
            }

        }


        //check status
        [HttpGet("checkstatus")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> CheckStatus(string Reg_Number)
        {
            var application = await _db.NGO_Application.Where(u => u.Reg_Number.Trim() == Reg_Number.Trim()).FirstOrDefaultAsync();
            if(application == null)
            {
                return NotFound(new { messsage = "Applicaton not found" });
            }
            
            return Ok(application.Status.ToString());

        }


        [HttpPost("re-upload-docs")]
        public async Task<IActionResult> ReuploadDocuments([FromForm] ReUploadDocumentDto reuploadDto)
        {
            var application = await _db.NGO_Application.Where(a => a.Application_Id == reuploadDto.Application_Id).FirstOrDefaultAsync();

            if (application == null)
            {
                return NotFound(new { messsage = "Applicaton not found" });
            }
            var documents = await _db.Document.Where(p => p.Entity_Id == reuploadDto.Application_Id).ToListAsync();
            
            foreach (var file in reuploadDto.Files)
            {
                //Console.WriteLine($"Uploaded: {file.FileName}");
                foreach (var doc in documents)
                {
                   // Console.WriteLine($"Database: {doc.Name}");
                    if (doc.Name == file.FileName)
                    {
                        Console.WriteLine("Matched!");
                        var fileStream = new MemoryStream();
                        await file.CopyToAsync(fileStream);
                        doc.Data = fileStream.ToArray();
                    }
                    await _db.SaveChangesAsync();
                }

            }

            application.Status = Status.PendingReview;
            await _db.SaveChangesAsync();

            return Ok(new { message = "You have successfully updated the documents" });
        }

        //get all the NGO applications
        [HttpGet("partnerships")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetAllPartnerships()
        {
            var partnerships = await _db.NGO_Partnership.Include(a=>a.NGO_Application).OrderByDescending(a=>a.Start_Date)
            .Select(p => new PartnershipDto
            { 
                ClosesCampus = p.NGO_Application.Close_Campus,
                    ApplicattionId = p.NGO_Application.Application_Id,
                    PartnershipId = p.Partnership_Id,
                    RegNumber = p.NGO_Application.Reg_Number,
                    CBO_Name = p.NGO_Application.Name,
                    CBO_Email = p.NGO_Application.Email,
                    CBO_Address = p.NGO_Application.Address,
                    CBO_Key_Focus = p.NGO_Application.Key_Focus,
                    //  Collaboration_Request = application.Collaboration_Request,
                    Description = p.NGO_Application.Description,
                    Benefits = p.NGO_Application.Benefits,
                    Notes = p.Notes,
                    Is_Active = p.Is_Active,
                    DaysToRenewal = p.DaysToRenewal
                }).ToListAsync();


            return Ok(partnerships);
            }
           //----------------------------------Validate-----------------------------------
        

        //get application by its ngos registaion number for director
        [HttpGet("campuspartnerships/specialist/{Id}/partnership/{PartnershipId}/")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetSpecificPartnership(int PartnershipId, int Id)
        {
            var partnership = await _db.NGO_Partnership.Where(p => p.Partnership_Id == PartnershipId).FirstOrDefaultAsync();
            if (partnership != null)
            {
                var application = await _db.NGO_Application.Where(a => a.Application_Id == partnership.Application_Id && a.Specialist_Id == Id).FirstOrDefaultAsync();
                var partner = new PartnershipDto
                {
                    ClosesCampus = application.Close_Campus,
                    ApplicattionId = application.Application_Id,
                    PartnershipId = partnership.Partnership_Id,
                    RegNumber = application.Reg_Number,
                    CBO_Name = application.Name,
                    CBO_Email = application.Email,
                    CBO_Address = application.Address,
                    CBO_Key_Focus = application.Key_Focus,
                  //  Collaboration_Request = application.Collaboration_Request,
                    Description = application.Description,
                    Benefits = application.Benefits,
                    Notes = partnership.Notes,
                    Is_Active = partnership.Is_Active,
                    DaysToRenewal = partnership.DaysToRenewal
                };

                return Ok(partner);
            }
            else
            {
                return NotFound(new { message = "Something went wrong , the application is not found" });
            }

        }

        //get the applications by status
        [HttpGet("partnership/status/{Is_Active}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetPartnershipByActivenesss(bool Is_Active)
        {
            //if (!Enum.TryParse<Status>(Status, true, out var appStatus))
            //{
            //    return BadRequest("Invalid role.");
            //}

            var partnershipsActiveness = await _db.NGO_Partnership.Where(p => p.Is_Active.Equals(Is_Active)).OrderByDescending(a=>a.Start_Date).Select(p => new PartnershipDto
            {
                ClosesCampus = p.NGO_Application.Close_Campus,
                ApplicattionId = p.NGO_Application.Application_Id,
                PartnershipId = p.Partnership_Id,
                RegNumber = p.NGO_Application.Reg_Number,
                CBO_Name = p.NGO_Application.Name,
                CBO_Email = p.NGO_Application.Email,
                CBO_Address = p.NGO_Application.Address,
                CBO_Key_Focus = p.NGO_Application.Key_Focus,
                //  Collaboration_Request = application.Collaboration_Request,
                Description = p.NGO_Application.Description,
                Benefits = p.NGO_Application.Benefits,
                Notes = p.Notes,
                Is_Active = p.Is_Active,
                DaysToRenewal = p.DaysToRenewal
            }).ToListAsync();

            return Ok(partnershipsActiveness);

        }

        [HttpGet("lapsedPartnerships")]
        public async Task<IActionResult> getLapsedPartnerships()
        {
            var lapsedPartnerships = await _db.NGO_Partnership.Where(p => p.Is_Active == false && p.Renewal_Date <= DateTime.Now).ToListAsync();
            return Ok(lapsedPartnerships);
        }

        [HttpGet("lapsingDeals")]
        public async Task<IActionResult> getLapsingPartnerships()
        {
            
            var lapsingDeals = await _db.NGO_Partnership.Where(p => p.Is_Active == true && (p.Renewal_Date >= DateTime.Now && p.Renewal_Date <= DateTime.Now.AddDays(15))).Include(p=>p.NGO_Application).ToListAsync();
            return Ok(new {count = lapsingDeals.Count , partnerships = lapsingDeals});
        }

        //renew the partnerhsi[ps that has lapsed
        [HttpPost("renewParnership/{partnerId}")]
        public async Task<IActionResult> RenewParnership(int partnerId)
        {
            var partner = await _db.NGO_Partnership.Where(p => p.Partnership_Id == partnerId && p.Is_Active == false).Include(p=> p.NGO_Application).FirstOrDefaultAsync();
            if (partner == null) { return NotFound(new { message = "Partner not found or still active" }); }
            partner.Renewal_Date.AddMonths(12);
            await _db.SaveChangesAsync();
             await _emailService.statusUpdateEmail(partner.NGO_Application.Email, Status.Approved);
            return Ok(new { messsage = "Renewed the parnership successful" });
        }

    }
}
