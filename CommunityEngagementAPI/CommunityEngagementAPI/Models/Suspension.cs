using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Suspension
    {
        [Key]
        public int Suspension_Id { get; set; }
        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }
        public bool Is_Active { get; set; }
        public int User_Id { get; set; }


        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }
    }
}
