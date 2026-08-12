using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Attendance_Entry
    {
        [Key]
        public int Attendance_Entry_Id { get; set; }

        public int Attendance_Id { get; set; }
       
        public int User_Id { get; set; }

        public DateTime Checkin_Time { get; set; }

        public DateTime? Checkout_Time { get; set; }

        public double? Hours_Logged { get; set; }

        [ForeignKey(nameof(Attendance_Id))]
        public Attendance_Register Attendance_Register { get; set; }

        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }

    }
}
