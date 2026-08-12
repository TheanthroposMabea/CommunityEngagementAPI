namespace CommunityEngagementAPI.Models.DTOs
{
    public class RetreiveNotificationDto
    {
       
        public int Notification_Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Is_Read { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created_At { get; set; }
    }
}
