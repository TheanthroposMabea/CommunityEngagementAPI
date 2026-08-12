namespace CommunityEngagementAPI.Models.DTOs
{
   
    public class RegisterDto
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string ?Id_Number { get; set; }
        public  string Campus_Code { get; set; }
        public string User_Role { get; set; }
        public string Volunteer_Role { get; set; }
        public string Faculty_Code { get;set; }

        // for use demographics 

        public string? Geo_Area { get; set; }
        public string? Home_Language { get; set; }
        public string? Gender { get; set; }
        public string? Is_International { get; set; }
        //staff
        public string? Department { get; set; }

        public int[]? Sdg { get; set; }
    }
}
