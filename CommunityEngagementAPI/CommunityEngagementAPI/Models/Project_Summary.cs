using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Project_Summary
    {
             [Key]
            public int Summary_Id { get; set; }

            public int Project_Id { get; set; }

            public int Total_Registered { get; set; }

            public int Total_Volunteered { get; set; }

            public int Completed_Count { get; set; }

            public int Absent_Count { get; set; }

            public int Late_Count { get; set; }

            public double Total_Hours { get; set; }

            public double Average_Hours { get; set; }


            public int Male_Count { get; set; }

            public int Female_Count { get; set; }

            public int Student_Count { get; set; }

            public int Alumni_Count { get; set; }

            public string Completion_Notes { get; set; }

            public DateTime Generated_At { get; set; }


            [ForeignKey(nameof(Project_Id))]   
            public Project Project { get; set; } = null!;

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();

    }
}
