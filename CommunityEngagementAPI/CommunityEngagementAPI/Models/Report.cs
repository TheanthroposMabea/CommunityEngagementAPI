using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum ReportStatus
    {
        PendingReview,
        AssistantReviewed,
        Reviewed

    }
    public class Report
    {
        [Key]
        public int Report_Id { get; set; }
        public string Notes { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime Date_Created { get; set; }
        public int User_Id { get; set; }
        public int Project_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }
        [ForeignKey(nameof(Project_Id))]
        public Project Project { get; set; }
    }
}
