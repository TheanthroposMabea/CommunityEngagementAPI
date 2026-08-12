namespace CommunityEngagementAPI.Models.DTOs
{
    public class ProjectSummaryDto
    {
       
            public int Summary_Id { get; set; }

            public int Project_Id { get; set; }

            public string Project_Name { get; set; } = string.Empty;

            public DateOnly Project_Date { get; set; }

            public string? Location { get; set; }


            // Attendance

            public int Total_Registered { get; set; }

            public int Total_Volunteers { get; set; }

            public int Completed_Count { get; set; }

            public int Absent_Count { get; set; }

            public int Late_Count { get; set; }


            // Hours

            public double Total_Hours { get; set; }

            public double Average_Hours { get; set; }


            // Demographics

            public int Male_Count { get; set; }

            public int Female_Count { get; set; }

            public int Student_Count { get; set; }

            public int Alumni_Count { get; set; }


            public string? Completion_Notes { get; set; }

            public DateTime Generated_At { get; set; }


            public List<SummaryPhotoDto> Photos { get; set; }
                = new();


           
            public List<AbsentVolunteerDto> Late_Volunteers { get; set; }
                = new();


            public List<FacultySummaryDto> Faculties { get; set; }
                = new();
        
    }
}
