using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum Gender
    {
        Male,
        Female
        
    }
    public class User_Demographics
    {
        [Key]
       
        public int User_Id { get; set; }
        public string? Geo_Area { get; set; }
        public string? Home_Language { get; set; }
        public string? Gender { get; set; }
        public bool? Is_International { get; set; }

        //nav property
        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }  
    }
}
