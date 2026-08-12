using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Attendance_Register
    {
        [Key]
        public int Attendance_Id { get; set; }
        public DateTime Created_At { get; set; }
        public int Assistant_Id { get; set; }
        public int Project_Id { get; set; }
        public string Checkinqr_Token { get; set; }
        public string? Checkoutqr_Token { get; set; }

        [ForeignKey(nameof(Assistant_Id))]
        public User User { get; set; }

        [ForeignKey(nameof(Project_Id))]
        public Project Project { get; set; }

        public ICollection<Attendance_Entry> AttendanceEntries { get; set; } = new List<Attendance_Entry>();


        //public int CheckIn_Qr { get; set; }
        //public int CheckOut_Qr { get; set; }
        //[ForeignKey(nameof(CheckIn_Qr))]
        //public Qrcode CheckInQrcode { get; set; }
        //[ForeignKey(nameof(CheckOut_Qr))]
        //public Qrcode CheckOutQrCode { get; set; }
    }
}
