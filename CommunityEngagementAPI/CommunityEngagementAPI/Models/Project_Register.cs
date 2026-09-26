using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Project_Register
    {
        [Key]
        public int Project_Id { get; set; }
       
        public int User_Id { get; set; }
        //public TimeOnly Time_Joined { get; set; }
        public bool Is_Committed { get; set; } = false;

       public DateTime Time { get; set; }

        [ForeignKey(nameof(Project_Id))]
        public Project Project { get; set; }
        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }
    }
}
