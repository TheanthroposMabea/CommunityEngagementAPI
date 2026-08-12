namespace CommunityEngagementAPI.Models.DTOs
{
    public class ProjectDto
    {
    
        public int Project_Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Location_Scope { get; set; }
        public string Partner { get; set; }
        public string Partner_Name { get; set; }
        public string? Partner_Id { get; set; }
        public string Category { get; set; }
        public int Hours { get; set; }
        public int Num_Volunteers { get; set; }
        public string? Status { get; set; }
        public string Description { get; set; }
        public string Target_Group { get; set; }
        public DateOnly Date { get; set; }
        public DateTime Date_Created { get; set; }
        public TimeOnly Time { get; set; }
        public int Slots { get; set; }
        public int User_Id { get; set; }
        public int[]? Sdg { get; set; }
    }
}
