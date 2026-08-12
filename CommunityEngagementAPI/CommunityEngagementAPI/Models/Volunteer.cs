using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum VolunteerType
    {
        Student,
        Alumni
    }
    public class Volunteer
    {
        [Key]
        public int Volunteer_Id { get; set; }
        public VolunteerType Volunteer_Type { get; set; }
        public bool Is_Completed { get; set; }
        public int? Recruiter_Id { get; set; }
        public double Hours { get; set; }

        //navigation property to User
        [ForeignKey(nameof(Volunteer_Id))]
        public User User { get; set; }

        public ICollection<Volunteer_Sdg> VolunteerSdgs { get; set; } = new List<Volunteer_Sdg>();


    }
}
