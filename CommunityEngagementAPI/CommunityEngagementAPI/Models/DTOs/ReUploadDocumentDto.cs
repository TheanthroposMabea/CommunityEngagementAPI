namespace CommunityEngagementAPI.Models.DTOs
{
    public class ReUploadDocumentDto
    {
        public   int Application_Id { get; set; }
        public ICollection<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
