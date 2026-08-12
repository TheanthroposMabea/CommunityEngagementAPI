using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Picture
    {
        [Key]
        public int Picture_Id { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
        public int User_Id { get; set; }
        
        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }
    }
}
