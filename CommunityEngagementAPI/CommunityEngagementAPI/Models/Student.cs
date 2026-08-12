using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public enum YearOfStudy
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }
    public class Student
    {
        [Key]
        public int Student_Id { get; set; }
        public string? Student_Number { get; set; }
        public YearOfStudy? Year_Of_Study { get; set; }
        public string? Qualification { get; set; }
        public string? Department { get; set; }

        //nav property
        [ForeignKey(nameof(Student_Id))]
        public Volunteer Volunteer { get; set; }
    }
}
