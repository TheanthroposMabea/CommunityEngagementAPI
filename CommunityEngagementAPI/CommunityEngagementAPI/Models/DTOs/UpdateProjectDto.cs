namespace CommunityEngagementAPI.Models.DTOs
{
    public class UpdateProjectDto
    {
        public int Project_Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Location_Scope { get; set; }
        public int Hours { get; set; }
        public int Num_Volunteers { get; set; }
        public string Description { get; set; }
        public string Target_Group { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int User_Id { get; set; }
    }
}
