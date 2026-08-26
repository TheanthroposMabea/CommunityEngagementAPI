using System.Collections.Generic;

namespace CommunityEngagementAPI.Models.DTOs
{
    public class ProjectReportDto
    {
        public int Total_Projects { get; set; }
        public int Pending_Projects { get; set; }
        public int Active_Projects { get; set; }
        public int Completed_Projects { get; set; }
        public int Cancelled_Projects { get; set; }
        public int Published_Projects { get; set; }

        public int Total_Target_Volunteers { get; set; }
        public int Total_Project_Registrations { get; set; }
        public int Unique_Volunteers { get; set; }
        public double Average_Volunteers_Per_Project { get; set; }

        public double Total_Target_Hours { get; set; }
        public double Total_Volunteer_Hours { get; set; }
        public double Attendance_Rate { get; set; }
        public double Completion_Rate { get; set; }
        public int Overdue_Reports { get; set; }

        public List<ProjectCampusReportDto> Projects_By_Campus { get; set; } = new();
        public List<ProjectMonthReportDto> Projects_By_Month { get; set; } = new();
        public List<ProjectSdgReportDto> Projects_By_Sdg { get; set; } = new();
    }

    public class ProjectCampusReportDto
    {
        public int Campus_Id { get; set; }
        public string Campus_Code { get; set; } = string.Empty;
        public int Projects { get; set; }
        public int Registered_Volunteers { get; set; }
        public double Volunteer_Hours { get; set; }
        public double Attendance_Rate { get; set; }
        public double Completion_Rate { get; set; }
    }

    public class ProjectMonthReportDto
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ProjectSdgReportDto
    {
        public int Sdg_Id { get; set; }
        public string Sdg_Name { get; set; } = string.Empty;
        public int Projects { get; set; }
    }

    public class OverallPerformanceReportDto
    {
        public double Overall_Score { get; set; }
        public string Performance_Status { get; set; } = string.Empty;

        public int Total_Projects { get; set; }
        public int Completed_Projects { get; set; }
        public int Active_Volunteers { get; set; }
        public int Unique_Volunteers { get; set; }
        public double Volunteer_Hours { get; set; }
        public double Attendance_Rate { get; set; }
        public double Completion_Rate { get; set; }
        public double Volunteer_Target_Achievement { get; set; }
        public int Projects_Needing_Attention { get; set; }
        public int Overdue_Reports { get; set; }
        public int Active_Partnerships { get; set; }

        public List<PerformanceCampusDto> Campus_Performance { get; set; } = new();
        public List<PerformanceTrendDto> Performance_Trend { get; set; } = new();
        public List<PerformanceDimensionDto> Dimensions { get; set; } = new();
        public List<string> Insights { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }

    public class PerformanceCampusDto
    {
        public int Campus_Id { get; set; }
        public string Campus_Code { get; set; } = string.Empty;
        public double Performance_Score { get; set; }
        public int Projects { get; set; }
        public int Volunteers { get; set; }
        public double Attendance_Rate { get; set; }
    }

    public class PerformanceTrendDto
    {
        public string Period { get; set; } = string.Empty;
        public double Score { get; set; }
        public int Projects { get; set; }
    }

    public class PerformanceDimensionDto
    {
        public string Name { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}
