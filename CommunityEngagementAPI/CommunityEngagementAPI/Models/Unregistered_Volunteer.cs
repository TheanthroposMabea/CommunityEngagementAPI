using Microsoft.AspNetCore.Routing.Constraints;

namespace CommunityEngagementAPI.Models
{
    public  class Unregistered_Volunteer
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Campus_Id { get; set; }
        public double Hours { get; set; }
    }
}