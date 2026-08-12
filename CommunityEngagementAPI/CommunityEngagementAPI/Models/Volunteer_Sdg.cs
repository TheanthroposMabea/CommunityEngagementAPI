using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Volunteer_Sdg
    {
       
        public int Volunteer_Id { get; set; }
        public int Sdg_Id { get; set; }

        [ForeignKey(nameof(Volunteer_Id))]
        public Volunteer Volunteer { get; set; }

        [ForeignKey(nameof(Sdg_Id))]
        public Sdg Sdg { get; set; }
    }
}
