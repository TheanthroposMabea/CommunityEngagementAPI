namespace CommunityEngagementAPI.Models.DTOs
{
    public class AssistantIntelligenceDashboardDto
    {
        public int Assistant_Id { get; set; }
        public string Assistant_Name { get; set; } = "Assistant";
        public int? Campus_Id { get; set; }
        public string Campus_Name { get; set; } = "Campus";
        public int Attendance_Sessions { get; set; }
        public int Volunteers_Recorded { get; set; }
        public int Reports_Submitted { get; set; }
        public int Pending_Summaries { get; set; }
        public double Attendance_Rate { get; set; }
        public double Operational_Score { get; set; }
        public List<ProjectHealthDto> Project_Health { get; set; } = new();
        public List<ProjectProblemDto> Problems { get; set; } = new();
        public List<ProjectRecommendationDto> Recommendations { get; set; } = new();
    }
}
