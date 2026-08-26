namespace CommunityEngagementAPI.Models.DTOs
{
    public class AreaPerformanceDto
    {
        public string Area_Type { get; set; } = string.Empty;
        public int Area_Id { get; set; }
        public string Area_Name { get; set; } = string.Empty;
        public int Projects { get; set; }
        public int Completed_Projects { get; set; }
        public int Volunteers { get; set; }
        public double Attendance_Rate { get; set; }
        public double Average_Project_Health { get; set; }
        public string Performance { get; set; } = string.Empty;
        public List<string> Reasons { get; set; } = new();
    }
}
