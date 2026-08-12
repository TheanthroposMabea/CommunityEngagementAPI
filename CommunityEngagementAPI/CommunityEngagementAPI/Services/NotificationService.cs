using CommunityEngagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CommunityEngagementAPI.Services
{
    public interface INotificationService
    {
        Task NotifyUser(int userId, string title, string message, NotificationType type);
        Task NotifyCampus(int? campusId, string title, string message, NotificationType type, int? excludeUserId = null);
        Task NotifyProjectRegister(int projectId, string title, string message, NotificationType type, int? excludeUserId = null);
    }
    public class NotificationService : INotificationService
    {
        private readonly CEAppContext _db;
        public NotificationService(CEAppContext db) => _db = db;

        public async Task NotifyUser(int userId, string title, string message, NotificationType type)
        {
            _db.Notification.Add(new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                Created_At = DateTime.Now,
                User_Id = userId
            });
            await _db.SaveChangesAsync();
        }

        public async Task NotifyCampus(int? campusId, string title, string message, NotificationType type, int? excludeUserId = null)
        {
            var userIds = await _db.User
                .Where(u => u.Campus_Id == campusId && u.User_Id != excludeUserId)
                .Select(u => u.User_Id)
                .ToListAsync();

            await BulkInsert(userIds, title, message, type,excludeUserId);
        }

        public async Task NotifyProjectRegister(int projectId, string title, string message, NotificationType type, int? excludeUserId = null)
        {
            var userIds = await _db.Project_Register
                .Where(pm => pm.Project_Id == projectId
                          && pm.Is_Committed == true
                          && pm.User_Id != excludeUserId)
                .Select(pm => pm.User_Id)
                .ToListAsync();

            await BulkInsert(userIds, title, message, type,excludeUserId);
        }

        private async Task BulkInsert(List<int> userIds, string title, string message, NotificationType type, int? createdById)
        {
            if (!userIds.Any()) return;
            var now = DateTime.Now;
            var notifications = userIds.Select(id => new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                Created_At = now,
                User_Id = id,
                Created_By_Id = createdById
            });
            _db.Notification.AddRange(notifications);
            await _db.SaveChangesAsync();
        }
    }
}
