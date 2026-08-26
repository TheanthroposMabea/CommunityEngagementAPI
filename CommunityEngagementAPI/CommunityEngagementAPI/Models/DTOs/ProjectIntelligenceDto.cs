namespace CommunityEngagementAPI.Models.DTOs
{
    public class ProjectIntelligenceDto
    {
        public ProjectHealthDto Health { get; set; } = new();
        public List<ProjectProblemDto> Problems { get; set; } = new();
        public List<ProjectRecommendationDto> Recommendations { get; set; } = new();
    }
}
