namespace CommunityEngagementAPI.Models.DTOs
{
    public class ProjectHealthDto
    {
        public int Project_Id { get; set; }
        public string Project_Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Health_Score { get; set; }
        public string Health_Status { get; set; } = string.Empty;
        public int Registered_Volunteers { get; set; }
        public int Expected_Volunteers { get; set; }
        public int Attended_Volunteers { get; set; }
        public double Attendance_Rate { get; set; }
        public double Hours_Completed { get; set; }
        public double Hours_Target { get; set; }
        public bool Summary_Generated { get; set; }
        public bool Report_Submitted { get; set; }
		public string? Primary_Problem { get; set; }
		public string? Primary_Recommendation { get; set; }
		public int? Primary_Priority { get; set; }		
		public bool Summary_Overdue { get; set; }
		public bool Attendance_Applicable { get; set; }
		public List<string> Limitations { get; set; } = new();
        public List<string> Problems { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }
}
