using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum Status
    {
        PendingReview,
        MissingInformation,
        Approved,
        Declined,
        Reviewed
    }
    public class NGO_Application
    {
        [Key]
        public int Application_Id { get; set; }
        public string Reg_Number { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Close_Campus { get; set; }
        public string Address { get; set; }
        public string Rep_Name { get; set; }
        public string Rep_Position { get; set; }
        public string Rep_Phone { get; set; }
        public string Key_Focus { get; set; }
        public string Collaboration_Request { get; set; }
        public string Description { get; set; }
        public  string Benefits { get; set; }
        public Status Status { get; set; }
        public string? Website { get; set; }
        public bool Is_Viewed { get; set; }

        public DateTime Date_Submitted { get; set; }
        [ForeignKey(nameof(User))]
        public int Specialist_Id { get; set; }

        public User User { get; set; }

        public ICollection<NGO_Sdg> NgoSdgs { get; set; } = new List<NGO_Sdg>();
    }
}
