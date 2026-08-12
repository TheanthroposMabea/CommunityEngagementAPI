using Microsoft.Identity.Client;

namespace CommunityEngagementAPI.Models.DTOs
{
    public class AdminsDashboardsDto
    {
        public int TotalVolunteers { get; set; }
        public int ActiveProjects { get; set; }
        public int MonthlyCompletedProjects { get; set; }
        public int NGOPartnerships { get; set; }
        public int MonthlyHours { get; set; }
        public List<PendingNGODto> NGOApplications { get; set; } = new List<PendingNGODto>();
        public List<SpecialistDto> Specialists { get; set; } = new List<SpecialistDto>();




    }
}
