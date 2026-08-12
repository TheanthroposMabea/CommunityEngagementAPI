namespace CommunityEngagementAPI.Models.DTOs
{
    public class CheckInDto
    {
        public int Attendance_Id { get; set; }
        public int Volunteer_id { get; set; }
        public string CheckInQr_Token { get; set; }
    }
}
