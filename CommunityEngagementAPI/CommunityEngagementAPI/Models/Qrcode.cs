using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Qrcode
    {
        [Key]
        public int Qrcode_Id { get; set; }
        public string Qrcode_Token { get; set; }
        public DateTime Time_Expires { get; set; }
        public int User_Id { get; set; }
        public int Project_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }

        [ForeignKey(nameof(Project_Id))]
        public Project Project { get; set; }
    }
}
