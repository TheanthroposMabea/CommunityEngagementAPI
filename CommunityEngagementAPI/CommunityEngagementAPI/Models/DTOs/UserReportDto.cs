namespace CommunityEngagementAPI.Models.DTOs
{
    public class UserReportDto
    {
        // Summary
        public int TotalRegisteredUsers { get; set; }
        public int NewRegistrationsThisMonth { get; set; }
        public int NewRegistrationsThisYear { get; set; }

        public int ActiveUsers { get; set; }
        public int SuspendedInactiveUsers { get; set; }

        public int UsersWithNoProjects { get; set; }
        public int UsersWithProjects { get; set; }

        public int AlumniUsers { get; set; }
        public int StudentUsers { get; set; }
        public int StaffUsers { get; set; }

        // Academic
        public List<UsersByYearDto> UsersByYear { get; set; } = new();
        public List<UsersByQualificationDto> UsersByQualification { get; set; } = new();
        public List<UsersByFacultyDto> UsersByFaculty { get; set; } = new();
        public List<UsersByDepartmentDto> UsersByProgramme { get; set; } = new();

        // Registration
        public List<RegistrationByMonthDto> RegistrationsByMonth { get; set; } = new();
        public List<RegistrationByYearDto> RegistrationsByYear { get; set; } = new();

        // Engagement
        public int TotalProjectsJoined { get; set; }
        public double AverageProjectsPerUser { get; set; }

        public List<MostActiveUserDto> MostActiveUsers { get; set; } = new();

        public int UsersWithZeroProjects { get; set; }
        public int UsersWithOneToTwoProjects { get; set; }
        public int UsersWithThreeOrMoreProjects { get; set; }

        public double TotalVolunteerHours { get; set; }
        public double AverageHoursPerUser { get; set; }

        public double AttendanceRate { get; set; }
        public int UsersWithPoorAttendance { get; set; }
        public int UsersWhoCompletedProjects { get; set; }
    }
}
