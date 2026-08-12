using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum ProjectStatus
    {
        Pending,
        Active,
        Completed,
        Cancelled
    }
    public enum ProjectScope
    {
        Local,
        Regional,
        International,
        National
    }

    public enum ProjectCategory
    {
        CommunityBasedResearch,
        ServiceLearning,
        OrganisedOutreach
    }

    public enum ProjectPartner
    {
        CBO,
        NPO,
        DEPARTMENT,
        STAFF
    }
    public class Project
    {
        [Key]
        public int Project_id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public ProjectScope Location_Scope { get; set; }
        public ProjectPartner Partner { get; set; }
        public string Partner_Name { get; set; }
        public string? Partner_Id { get; set; }
        public ProjectCategory Category { get; set; }
        public int Hours { get; set; }
        public int Num_Volunteers { get; set; }
        public ProjectStatus Status { get; set; }
        public string Description { get; set; }
        public string Target_Group { get; set; }
        public bool Is_Published { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public DateTime Date_Created { get; set; }

        public int Campus_Id { get; set; }


        public int User_Id { get; set; }

        [ForeignKey(nameof(Campus_Id))]
        public Campus Campus { get; set; }

        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }

        public ICollection<Project_Sdg> ProjectSdgs { get; set; } = new List<Project_Sdg>();
        public ICollection<Project_Register> ProjectRegisters { get; set; } = new List<Project_Register>();
        public ICollection<Attendance_Register> AttendanceRegisters { get; set; } = new List<Attendance_Register>();

        [NotMapped]
        public int Slots => Num_Volunteers - ProjectRegisters.Count;

        [NotMapped]
        public int DaysToProject => (Date.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber);
    }
}
