using System.ComponentModel.DataAnnotations;

namespace CommunityEngagementAPI.Models
{
    public class Faculty
    {
        [Key]
        public int Faculty_Id { get; set; }
        public string? Name { get; set; }

       // public int? Campus_Id { get; set; }

        public string Code { get; set; }

    }
}
