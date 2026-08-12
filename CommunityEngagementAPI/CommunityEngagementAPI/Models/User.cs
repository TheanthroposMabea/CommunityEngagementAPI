using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum UserRoleType
    {
        director,  //MAKE LOWER BEFORE SENDING TO DB
        specialist,
        assistant,
        volunteer,
        staff
    }
    public class User
    {
        [Key]
        public int User_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Is_Active { get; set; }
        public string Id_Number { get; set; } //staff number for staff specialist and director
        public string Phone { get; set; }
        [Column(TypeName ="varchar(20")]
        public UserRoleType Role { get; set; }
        public bool Is_Suspended { get; set; } = false;
        public DateTime Date_Created { get; set; }
        [ForeignKey(nameof(Campus))]
        public int? Campus_Id { get; set; }
        [ForeignKey(nameof(Faculty))]
        public int? Faculty_Id { get; set; }

        public Faculty Faculty { get; set; }
        public Campus Campus { get; set; }
        public ICollection<Project_Register> ProjectRegisters { get; set; } = new List<Project_Register>();
        public ICollection<Attendance_Entry> AttendanceEntries { get; set; } = new List<Attendance_Entry>();

    }
}
