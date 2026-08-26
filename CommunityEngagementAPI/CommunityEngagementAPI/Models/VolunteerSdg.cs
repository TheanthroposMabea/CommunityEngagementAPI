using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class VolunteerSdg
    {


        [ForeignKey(nameof(Volunteer))]
        public int Volunteer_Id { get; set; }
        [ForeignKey(nameof(Sdg))]
        public int Sdg_Id { get; set; }

      

       
        public Volunteer Volunteer { get; set; }
        public Sdg Sdg { get; set; }
        
    }
}
