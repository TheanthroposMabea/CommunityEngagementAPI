namespace CommunityEngagementAPI.Models.DTOs
{
    public class NotificationDto
    {
       public int Project_Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "GeneralAdmin";
        public int User_Id { get; set; }
       // public string CreatedBy { get; set; } 
        public DateTime Created_At { get; set; }
    }
}
