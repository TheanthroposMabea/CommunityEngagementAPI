namespace CommunityEngagementAPI.Models.DTOs
{
    public class CheckOutDto
    {
        public int Attendance_Id { get; set; }
        public int User_id { get; set; }
        public string CheckOutQr_Token { get; set; }
    }
}
