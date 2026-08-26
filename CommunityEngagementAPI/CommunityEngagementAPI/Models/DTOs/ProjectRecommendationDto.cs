namespace CommunityEngagementAPI.Models.DTOs
{
    public class ProjectRecommendationDto
    {
        public int Project_Id { get; set; }
        public string Project_Name { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
