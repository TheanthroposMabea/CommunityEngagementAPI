namespace CommunityEngagementAPI.Models.DTOs
{
    public class VolunteerIntelligenceDashboardDto
    {
        public int Volunteer_Id { get; set; }
        public string Volunteer_Name { get; set; } = "Volunteer";
        public int? Campus_Id { get; set; }
        public string Campus_Name { get; set; } = "Campus";
        public double Hours_Logged { get; set; }
        public int Projects_Registered { get; set; }
        public int Projects_Attended { get; set; }
        public int Certificates { get; set; }
        public double Attendance_Rate { get; set; }
        public double Engagement_Score { get; set; }
        public bool Completion_Eligible { get; set; }
        public List<VolunteerScoreItemDto> Score_Breakdown { get; set; } = new();
        public List<VolunteerOpportunityDto> Recommended_Opportunities { get; set; } = new();
        public List<ProjectHealthDto> My_Project_Health { get; set; } = new();
    }

    public class VolunteerScoreItemDto
    {
        public string Metric { get; set; } = "";
        public double Score { get; set; }
        public double Maximum { get; set; }
    }

    public class VolunteerOpportunityDto
    {
        public int Project_Id { get; set; }
        public string Project_Name { get; set; } = "Project";
        public DateOnly Date { get; set; }
        public int Hours { get; set; }
        public int Available_Slots { get; set; }
        public string Campus_Name { get; set; } = "Campus";
        public string Match_Reason { get; set; } = "Available opportunity";
    }
}
