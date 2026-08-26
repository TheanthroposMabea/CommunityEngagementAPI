namespace CommunityEngagementAPI.Models.DTOs
{
    public class ProjectProblemDto
    {
        public int Project_Id { get; set; }
        public string Project_Name { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Problem { get; set; } = string.Empty;
        public string Recommended_Action { get; set; } = string.Empty;
    }
}
