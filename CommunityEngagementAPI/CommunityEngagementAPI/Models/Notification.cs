using MailKit.Net.Smtp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum NotificationType
    {
        ProjectCreated,
        ProjectUpdated,
        ProjectCancelled,
        ProjectJoined,
        ProjectNotification,
        SummaryGenerated,
        GeneralAdmin,
        AttendanceConfirmed,
        ReportSubmitted,
        ApplicationSubmitted,
        NGOStatus,
        RoleChanged,
        AccountSuspension,
        CertificateIssued,
        ProjectRisk,
        ProjectEscalation,
        ProjectRecommendation
    }
    public class Notification
    {
        [Key]
        public int Notification_Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public DateTime Created_At { get; set; }
        public bool Is_Read { get; set; } = false;
        public int User_Id { get; set; }
        public int? Created_By_Id { get; set; }
        
        
        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }

        [ForeignKey(nameof(Created_By_Id))]
        public User CreatedBy { get; set; }

        //[NotMapped]
        //public int? Entity_Id { get; set; }
        //[NotMapped]
        //[ForeignKey(nameof(Entity_Id))]
        //public Project Project { get; set; } 
    }
}
