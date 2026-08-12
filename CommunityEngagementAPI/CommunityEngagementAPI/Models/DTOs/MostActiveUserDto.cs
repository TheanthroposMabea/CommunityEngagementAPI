namespace CommunityEngagementAPI.Models.DTOs
{
    public class MostActiveUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ProjectsJoined { get; set; }
        public double Hours { get; set; }
    }
}
