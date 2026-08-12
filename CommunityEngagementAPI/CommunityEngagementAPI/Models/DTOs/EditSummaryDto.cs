namespace CommunityEngagementAPI.Models.DTOs
{
    public class EditSummaryDto
    {
        public int Summary_Id { get; set; }

        public string? Notes { get; set; }

        public List<IFormFile>? New_Photos { get; set; }

        public List<int>? Deleted_Photo_Ids { get; set; }
    }
}
