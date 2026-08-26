namespace CommunityEngagementAPI.Models.DTOs
{
    public class CommunityImpactDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Projects_Completed { get; set; }
        public int Active_Projects { get; set; }
        public int Volunteers_Involved { get; set; }
        public int Volunteer_Hours { get; set; }
        public int Attendance_Entries { get; set; }
        public int Sdg_Count { get; set; }
        public List<SdgImpactDto> Sdg_Impact { get; set; } = new();
    }

    public class SdgImpactDto
    {
        public int Sdg_Id { get; set; }
        public string Sdg_Name { get; set; } = string.Empty;
        public int Projects { get; set; }
        public int Volunteers { get; set; }
        public double Hours { get; set; }
    }
}
