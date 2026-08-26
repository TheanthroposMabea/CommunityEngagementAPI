using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
   // public enum Gender
   // {
    //    Male,
   //     Female
        
   // }
    public class UserDemographics
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int User_Id { get; set; }
        public string? Geo_Area { get; set; }
        public string? Home_Language { get; set; }
        public Gender? Gender { get; set; }
        public bool IsInternational { get; set; }

        //nav property
        public User User { get; set; }  
    }
}
