using System.ComponentModel.DataAnnotations;

namespace CommunityEngagementAPI.Models
{
    public class Document
    {
        [Key]
        public int Document_Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
        public byte Size { get; set; }
        public DateTime Date_Uploaded { get; set; }
        public string Entity_Type { get; set; }
        public int Entity_Id { get; set; }

    }
}
