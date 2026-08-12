using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Photo
    {
        [Key]
        public int Photo_Id { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
        public int Summary_Id { get; set; }

        [ForeignKey(nameof(Summary_Id))]
        public Project_Summary Project_Summary { get; set; } 
    }
}
