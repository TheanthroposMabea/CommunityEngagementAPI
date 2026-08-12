namespace CommunityEngagementAPI.Models.DTOs
{
    public class GenerateSummaryDto
    {
        public int Project_Id { get; set; }

        public string Notes { get; set; }

        public List<IFormFile> Photos { get; set; }
    }
}
