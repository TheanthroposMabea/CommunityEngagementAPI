namespace CommunityEngagementAPI.Models.DTOs
{
    public class AttendanceRegisterDto
    {
       
        public int Assistant_Id { get; set; }
        public int Project_Id { get; set; }
        public DateTime Created_At { get; set; }
        public string CheckInQr_Token { get; set; }
        public string? CheckOutQr_Token { get; set; }
    }
}
