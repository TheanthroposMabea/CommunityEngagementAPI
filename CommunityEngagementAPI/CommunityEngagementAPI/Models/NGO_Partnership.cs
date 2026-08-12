using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class NGO_Partnership
    {
        [Key]
        public int Partnership_Id { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime Renewal_Date { get; set; }
        public bool Is_Active { get; set; }
        public string Notes { get; set; }
        public int Application_Id { get; set; }

        [ForeignKey(nameof(Application_Id))]
        public NGO_Application NGO_Application { get; set; }

        [NotMapped]
        public int DaysToRenewal => (Renewal_Date.Date - DateTime.Today).Days;
    }
}
