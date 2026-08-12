
using CommunityEngagementAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MimeKit.Cryptography;
using StudentApp.API.Services;

namespace CommunityEngagementAPI.Services
{
    public class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EmailService _emailService;
        private readonly CEAppContext _db;

        public BackgroundService(CEAppContext db ,IServiceScopeFactory scopeFactory, EmailService emailService)
        {
            _emailService = emailService;
            _scopeFactory = scopeFactory;
            _db = db;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateAsyncScope();

                var db = scope.ServiceProvider.GetRequiredService<CEAppContext>();
                //check for the hours  of volunteering
                var completedVolunteers = await db.Volunteer.Where(v => v.Hours >= 80).ToListAsync();
                string status = "COMPLETED";
               if (!Enum.TryParse<Status>(status,true, out var Status)){
                    Console.WriteLine("Could not convert the Status string");
                }
                foreach (var volunteer in completedVolunteers)
                {
                    volunteer.Is_Completed = true;
                    var email = await db.User.Where(u => u.User_Id == volunteer.Volunteer_Id).Select(u => u.Email).FirstAsync();
                     await _emailService.statusUpdateEmail(email, Status);
                }
                await db.SaveChangesAsync();
                //check for the users that are suspended and it has expired or lapsed
                var unsuspendedUsers = await db.Suspension.Where(u => u.Is_Active == true && u.End_Date <= DateOnly.FromDateTime(DateTime.Now)).ToListAsync();
                foreach(var user in unsuspendedUsers)
                {
                    var _user = await db.User.Where(u => u.User_Id == user.User_Id).FirstAsync();
                    _user.Is_Suspended = false;
                    user.Is_Active = false;
                    
                }
                await db.SaveChangesAsync();

         
             

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
