using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Staff
    {
        [Key]
        public int Staff_Id { get; set; }
       // public string? Staff_Number { get; set; }
        public string? Department { get; set; }

        [ForeignKey(nameof(Staff_Id))]
        public User User { get; set; }
    }
}
