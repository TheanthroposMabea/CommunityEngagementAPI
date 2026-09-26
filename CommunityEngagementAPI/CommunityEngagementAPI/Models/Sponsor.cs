using System.ComponentModel.DataAnnotations;

namespace CommunityEngagementAPI.Models
{
    public class Sponsor
    {
        [Key]
        public int Sponsor_id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public int Pledge { get; set; }
    }
}
