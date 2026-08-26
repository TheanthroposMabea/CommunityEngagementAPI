namespace CommunityEngagementAPI.Models.DTOs
{
    public class IntelligenceDashboardDto
    {
        public int Projects_Requiring_Attention { get; set; }
        public int Critical_Projects { get; set; }
        public int Projects_On_Track { get; set; }
        public int Overdue_Reports { get; set; }
        public int Low_Participation_Projects { get; set; }
        public double Overall_Project_Health { get; set; }
        public CommunityImpactDto Impact { get; set; } = new();
        public List<ProjectProblemDto> Problems { get; set; } = new();
        public List<ProjectRecommendationDto> Recommendations { get; set; } = new();
        public List<AreaPerformanceDto> Areas_Requiring_Attention { get; set; } = new();
    }
}
