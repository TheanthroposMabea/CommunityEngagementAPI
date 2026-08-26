namespace CommunityEngagementAPI.Models.DTOs
{
    public class SpecialistIntelligenceDashboardDto
    {
        public int Specialist_Id { get; set; }
        public string Specialist_Name { get; set; } = string.Empty;
        public int Campus_Id { get; set; }
        public string Campus_Name { get; set; } = string.Empty;
        public string Campus_Code { get; set; } = string.Empty;
        public int Projects_Requiring_Attention { get; set; }
        public int Critical_Projects { get; set; }
        public int Projects_On_Track { get; set; }
        public int Overdue_Reports { get; set; }
        public int Low_Participation_Projects { get; set; }
        public double Overall_Campus_Health { get; set; }
        public int Active_Projects { get; set; }
        public int Total_Volunteers { get; set; }
        public int Volunteer_Hours { get; set; }
        public double Attendance_Rate { get; set; }
        public List<ProjectHealthDto> My_Project_Health { get; set; } = new();
        public List<ProjectHealthDto> Campus_Project_Health { get; set; } = new();
        public List<ProjectProblemDto> My_Problems { get; set; } = new();
        public List<ProjectRecommendationDto> My_Recommendations { get; set; } = new();
        public List<ProjectProblemDto> Campus_Problems { get; set; } = new();
        public List<ProjectRecommendationDto> Campus_Recommendations { get; set; } = new();
        public List<SdgImpactDto> Sdg_Impact { get; set; } = new();
    }
}
