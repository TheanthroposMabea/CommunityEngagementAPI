using CommunityEngagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MimeKit.Cryptography;
using StudentApp.API.Services;

namespace CommunityEngagementAPI.Services
{
    public class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public readonly EmailService _emailService;
        public BackgroundService(IServiceScopeFactory scopeFactory ,EmailService emailService)
        {
            _scopeFactory = scopeFactory;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateAsyncScope();

                var db = scope.ServiceProvider.GetRequiredService<CEAppContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                //check for the hours  of volunteering
                var completedVolunteers = await db.Volunteer.Where(v => v.Hours >= 80 && v.Is_Completed == false).ToListAsync(stoppingToken);
                string status = "COMPLETED";
               if (!Enum.TryParse<Status>(status,true, out var Status)){
                    Console.WriteLine("Could not convert the Status string");
                }
                foreach (var volunteer in completedVolunteers)
                {
                    volunteer.Is_Completed = true;
                    var email = await db.User.Where(u => u.User_Id == volunteer.Volunteer_Id).Select(u => u.Email).FirstAsync(stoppingToken);
                     await emailService.statusUpdateEmail(email, Status);
                }
                await db.SaveChangesAsync(stoppingToken);

                //check for the users that are suspended and it has expired or lapsed
                var unsuspendedUsers = await db.Suspension.Where(u => u.Is_Active == true && u.End_Date <= DateOnly.FromDateTime(DateTime.Now)).ToListAsync(stoppingToken);
                foreach(var user in unsuspendedUsers)
                {
                    var _user = await db.User.Where(u => u.User_Id == user.User_Id).FirstAsync(stoppingToken);
                    _user.Is_Suspended = false;
                    user.Is_Active = false;

                }
                await db.SaveChangesAsync(stoppingToken);

                //check projects automatically and notify the responsible user when a project requires attention
                var intelligenceService = scope.ServiceProvider.GetRequiredService<IIntelligenceService>();
                await intelligenceService.ProcessAutomaticProjectChecks(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
