using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Certificate
    {
        [Key]
        public int Certificate_Id { get; set; }
        public int Total_Hours { get; set; }
        public int Total_Project { get; set; }
        public DateTime Date_Issued { get; set; }
        public int User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }
    }
}
