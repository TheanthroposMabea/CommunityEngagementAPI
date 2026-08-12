using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Alumni
    {
        [Key] 
        public int Alumni_Id { get; set; }
        public DateOnly? Year_Graduated { get; set; }
        public string? Department { get; set; }

        //nav property
        [ForeignKey(nameof(Alumni_Id))]
        public Volunteer Volunteer { get; set; }
    }
}
