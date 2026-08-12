using System.ComponentModel.DataAnnotations;

namespace CommunityEngagementAPI.Models
{
    public enum CampusCode
    {
        APK,
        APB,
        DFC,
        SWC
    }
    public class Campus
    {
        [Key]
        public int Campus_Id { get; set; }
        public CampusCode Code { get; set; }
        public string? Name { get; set; }
    }
}
