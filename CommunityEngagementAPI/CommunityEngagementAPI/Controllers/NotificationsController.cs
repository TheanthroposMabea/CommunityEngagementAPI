using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg;
using System.Security.Claims;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController :ControllerBase
    {
        private readonly CEAppContext _db;
        private readonly INotificationService _notificationService;

        public NotificationsController(CEAppContext db, INotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }

        [HttpPost("create-campus-notification")]
        public async Task<IActionResult> CreateCampusNotification(NotificationDto dto)
        {
          
            var specialist = await _db.User.FindAsync(dto.User_Id);
            if (specialist == null) return Unauthorized();

            var type = Enum.TryParse<NotificationType>(dto.Type, true, out var parsed)
                ? parsed
                : NotificationType.GeneralAdmin;

            await _notificationService.NotifyCampus(
                specialist.Campus_Id, dto.Title, dto.Message, type, excludeUserId: specialist.User_Id);

            return Ok(new { message = "Campus notification sent" });
        }
        [HttpPost("create-project-notification")]
        public async Task<IActionResult> CreateProjectNotification([FromBody]NotificationDto dto)
        {
            var user = await _db.Project_Register.Where(pr => pr.User_Id == dto.User_Id && pr.Project_Id == dto.Project_Id).FirstOrDefaultAsync();
            if(user == null)
            {
                return Forbid();
            }
            
            var type = Enum.TryParse<NotificationType>(dto.Type, true, out var parsed)
                ? parsed
                : NotificationType.ProjectNotification;
            // send a notification to all the people registered for the project
            await _notificationService.NotifyProjectRegister(
                dto.Project_Id, dto.Title, dto.Message, type, excludeUserId: user.User_Id);

            return Ok(new { message = "Project notification sent" });
        }

        [HttpGet("my-notifications/{User_Id}")]
        public async Task<IActionResult> GetMyNotifications(int User_Id)
        {
            int page = 1;int  pageSize = 20;

            var notifications = await _db.Notification.Where(n => n.User_Id == User_Id).OrderByDescending(n => n.Created_At)
                .Skip((page - 1) * pageSize).Take(pageSize).Select( n => new RetreiveNotificationDto()
                {
                  Notification_Id = n.Notification_Id,
                    Title = n.Title,
                    Message = n.Message,
                    Is_Read = n.Is_Read.ToString(),
                    Type = n.Type.ToString(),
                    CreatedBy =   _db.User.Where(u => u.User_Id == n.Created_By_Id).Select(p => p.Last_Name).FirstOrDefault(),
                    Created_At = n.Created_At
                }).ToListAsync();

            if (notifications.IsNullOrEmpty())
            {
                return NotFound(new { message = "User does not have any unread notifications." });
            }

            return Ok(notifications);
        }

        [HttpPatch("{UserId}/mark-read/notification/{notificationId}")]     
        public async Task<IActionResult> MarkAsRead(int UserId,int notificationId)
        {
            var user = await _db.User.FindAsync(UserId);
            var notification = await _db.Notification.FirstOrDefaultAsync(n => n.Notification_Id == notificationId && n.User_Id == UserId);
            if (notification == null) return NotFound();

            notification.Is_Read = true;
            await _db.SaveChangesAsync();
            return Ok("Marked as read");
        }

        [HttpGet("admin-notification/{Specialist_Id}")]
        public async Task<IActionResult> MyCreatedNotifications(int Specialist_Id)
        {
            int page = 1; int pageSize = 20;
            var query = _db.Notification.Where(n => n.Created_By_Id == Specialist_Id);
            if (query == null)
            {
                return NotFound(new { message = "User does not have any unread notifications." });
            }

            var representativeIds = await query
                .GroupBy(n => n.Created_At)
                .Select(g => g.Min(n => n.Notification_Id))
                .ToListAsync();

            var notifications = await query
                .Where(n => representativeIds.Contains(n.Notification_Id))
                .OrderByDescending(n => n.Created_At)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new
                { 
                    n.Notification_Id,
                    n.Title,
                    n.Message,
                    n.Type,
                    n.Created_At,
                    n.Is_Read
                })
                .ToListAsync();

            return Ok(notifications);
        }
    }
    
}
