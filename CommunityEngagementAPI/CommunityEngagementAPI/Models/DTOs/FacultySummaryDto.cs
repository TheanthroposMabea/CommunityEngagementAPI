namespace CommunityEngagementAPI.Models.DTOs
{
    public class FacultySummaryDto
    {
        public int? Faculty_Id { get; set; }
        public string? Faculty_Name { get; set; } = string.Empty;
        public int Volunteer_Count { get; set; }
    }
}
