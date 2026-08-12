namespace CommunityEngagementAPI.Models.DTOs
{
    public class AbsentVolunteerDto
    {
        public int User_Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
