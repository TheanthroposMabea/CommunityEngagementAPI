namespace CommunityEngagementAPI.Models.DTOs
{
    public class UserDto
    {
        public int User_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Campus_Code { get; set; }
        public string User_Role { get; set; }
        public string Faculty_Code { get; set; }
        public string Is_Active { get; set; }
        public string? Geo_Area { get; set; }
        public string? Home_Language { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
    }
}
