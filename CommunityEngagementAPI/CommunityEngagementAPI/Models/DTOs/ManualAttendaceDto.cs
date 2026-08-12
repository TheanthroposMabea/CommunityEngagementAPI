namespace CommunityEngagementAPI.Models.DTOs
{
    public class ManualAttendaceDto
    {
        public int Project_Id { get; set; }
        public string Email { get; set; }
        public double Hours_Logged { get; set; }
        public DateTime Checkin_Time { get; set; }
        public DateTime? Checkout_Time { get; set; }

        //for unregistered
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Campus_Code { get; set; }
    }
}
