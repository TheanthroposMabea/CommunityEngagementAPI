namespace CommunityEngagementAPI.Models.DTOs
{
    public class VolunteerDto
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
     
      
        public string Faculty_Code { get; set; }
        public string Volunteer_Type { get; set; }
        public bool Is_Completed { get; set; }
      //  public int? Recruiter_Id { get; set; }
        public double Hours { get; set; }

        public string? Geo_Area { get; set; }
        public string? Home_Language { get; set; }
        public string? Gender { get; set; }
        public bool? Nationality { get; set; }
    }
}
