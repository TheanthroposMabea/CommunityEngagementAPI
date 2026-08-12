using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using CommunityEngagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityEngagementAPI.Controllers
{
    public class LeaderboardsController : ControllerBase
    {
        private readonly CEAppContext _db;
        private readonly INotificationService _notificationService;

        public LeaderboardsController(CEAppContext db, INotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }

        [HttpPost("leaderboards/role/{role}")]
        public async Task<IActionResult> VolunteeringLeaderboards(string role)
        {
            Enum.TryParse<UserRoleType>(role, true, out var Role);

            var usersQuery = _db.User
                .AsNoTracking().Where(u => u.Role == Role)
                .Include(u => u.Faculty)
                .AsQueryable();

            var userIds = usersQuery
                .Select(u => u.User_Id)
                .ToList();


            var attendanceEntries = await _db.Attendance_Entry
           .AsNoTracking()
           .Include(a => a.Attendance_Register)
           .Where(a => userIds.Contains(a.User_Id))
           .ToListAsync();

            var projectRegisters = await _db.Project_Register
              .AsNoTracking()
              .Where(p => userIds.Contains(p.User_Id))
              .ToListAsync();


            var projectCountByUser = projectRegisters
             .GroupBy(p => p.User_Id)
             .ToDictionary(
                 g => g.Key,
                 g => g.Count());

            var hoursByUser = attendanceEntries
                .GroupBy(a => a.User_Id)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(a => a.Hours_Logged ?? 0));

            var mostActiveUsers = usersQuery
                .AsEnumerable()
                .Select(u =>
                {
                    var projects = projectCountByUser.TryGetValue(u.User_Id, out var projectCount)
                        ? projectCount
                        : 0;

                    var hours = hoursByUser.TryGetValue(u.User_Id, out var totalHours)
                        ? totalHours
                        : 0;

                    return new MostActiveUserDto
                    {
                        UserId = u.User_Id,
                        Name = $"{u.First_Name} {u.Last_Name}",
                        Email = u.Email,
                        ProjectsJoined = projects,
                        Hours = hours
                    };
                })
                .OrderByDescending(x => x.ProjectsJoined)
                .ThenByDescending(x => x.Hours)
                .Take(10)
                .ToList();



            return Ok(mostActiveUsers);
        }

    }
}
