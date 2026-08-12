using System.ComponentModel.DataAnnotations;

namespace CommunityEngagementAPI.Models
{
    public class Sdg
    {
        [Key]
        public int Sdg_Id { get; set; }
        public string Name { get; set; }

        public ICollection<Volunteer_Sdg> VolunteerSdgs { get; set; } = new List<Volunteer_Sdg>();
        public ICollection<NGO_Sdg> NgoSdgs { get; set; } = new List<NGO_Sdg>();
        public ICollection<Project_Sdg> ProjectSdgs { get; set; } = new List<Project_Sdg>();
    }
}
