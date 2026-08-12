using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
namespace CommunityEngagementAPI.Services
{
    public interface IVolunteerReportService
    {
        Task<UserReportDto> GetVolunteerReportAsync(int? campusId);
    }

    public class VolunteerReportService : IVolunteerReportService
    {
        private readonly CEAppContext _db;

        public VolunteerReportService(CEAppContext db)
        {
            _db = db;
        }

        public async Task<UserReportDto> GetVolunteerReportAsync(int? campusId)
        {
            var today = DateTime.Today;

            var startOfMonth = new DateTime(
                today.Year,
                today.Month,
                1);

            var startOfYear = new DateTime(
                today.Year,
                1,
                1);

            // -----------------------------------------
            // BASE USER QUERY
            // -----------------------------------------

            var usersQuery = _db.User
                .AsNoTracking().Where(u => u.Role == UserRoleType.volunteer || u.Role == UserRoleType.assistant)
                .Include(u => u.Faculty)
                .AsQueryable();

            //var usersQuery = _db.User
            //   .AsNoTracking()
            //   .AsQueryable();

            // null = ALL CAMPUSES
            if (campusId.HasValue)
            {
                usersQuery = usersQuery
                    .Where(u => u.Campus_Id == campusId.Value);
            }

            var users = await usersQuery
                .Include(u => u.Faculty)
                .Include(u => u.ProjectRegisters)
                .ToListAsync();

            var userIds = users
                .Select(u => u.User_Id)
                .ToList();

            // -----------------------------------------
            // STUDENTS
            // -----------------------------------------

            var students = await _db.Student
                .AsNoTracking()
                .Where(s => userIds.Contains(s.Student_Id))
                .ToListAsync();

            // -----------------------------------------
            // ALUMNI
            // -----------------------------------------

            var alumni = await _db.Alumni
                .AsNoTracking()
                .Where(a => userIds.Contains(a.Alumni_Id))
                .ToListAsync();

            // -----------------------------------------
            // STAFF
            // -----------------------------------------

            var staff = await _db.Staff
                .AsNoTracking()
                .Where(s => userIds.Contains(s.Staff_Id))
                .ToListAsync();

            // -----------------------------------------
            // ATTENDANCE
            // -----------------------------------------

            var attendanceEntries = await _db.Attendance_Entry
             .AsNoTracking()
             .Include(a => a.Attendance_Register)
             .Where(a => userIds.Contains(a.User_Id))
             .ToListAsync();

            // -----------------------------------------
            // PROJECT REGISTRATIONS
            // -----------------------------------------

            var projectRegisters = await _db.Project_Register
                .AsNoTracking()
                .Where(p => userIds.Contains(p.User_Id))
                .ToListAsync();

            // -----------------------------------------
            // BASIC COUNTS
            // -----------------------------------------

            var totalUsers = users.Count;

            var activeUsers = users.Count(u =>
                u.Is_Active &&
                !u.Is_Suspended);

            var suspendedInactiveUsers = users.Count(u =>
                !u.Is_Active ||
                u.Is_Suspended);

            var newThisMonth = users.Count(u =>
                u.Date_Created >= startOfMonth);

            var newThisYear = users.Count(u =>
                u.Date_Created >= startOfYear);

            // -----------------------------------------
            // PROJECT PARTICIPATION
            // -----------------------------------------

            var projectCountByUser = projectRegisters
                .GroupBy(p => p.User_Id)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count());

            var usersWithProjects = users.Count(u =>
                projectCountByUser.ContainsKey(u.User_Id));

            var usersWithoutProjects = totalUsers - usersWithProjects;

            var usersWithZeroProjects = users.Count(u =>
                !projectCountByUser.ContainsKey(u.User_Id));

            var usersWithOneToTwoProjects = users.Count(u =>
                projectCountByUser.ContainsKey(u.User_Id) &&
                projectCountByUser[u.User_Id] <= 2);

            var usersWithThreeOrMoreProjects = users.Count(u =>
                projectCountByUser.ContainsKey(u.User_Id) &&
                projectCountByUser[u.User_Id] >= 3);

            var totalProjectsJoined = projectRegisters.Count;

            var averageProjectsPerUser = totalUsers == 0
                ? 0
                : (double)totalProjectsJoined / totalUsers;

            // -----------------------------------------
            // HOURS
            // -----------------------------------------

            var totalVolunteerHours = attendanceEntries
                .Where(a => a.Hours_Logged.HasValue)
                .Sum(a => a.Hours_Logged ?? 0);

            var averageHoursPerUser = totalUsers == 0
                ? 0
                : totalVolunteerHours / totalUsers;

            // -----------------------------------------
            // ATTENDANCE
            // -----------------------------------------

            var registeredUserIds = projectRegisters
                .Select(p => p.User_Id)
                .Distinct()
                .ToHashSet();

            var attendedUserIds = attendanceEntries
                .Select(a => a.User_Id)
                .Distinct()
                .ToHashSet();

            var expectedAttendance = projectRegisters.Count;

            var actualAttendance = projectRegisters.Count(p =>
                attendedUserIds.Contains(p.User_Id));

            var attendanceRate = expectedAttendance == 0
                ? 0
                : (double)actualAttendance / expectedAttendance * 100;

            // -----------------------------------------
            // POOR ATTENDANCE
            // -----------------------------------------    

            var attendanceByUser = projectRegisters
                .GroupBy(p => p.User_Id)
                .Select(g =>
                {
                    var userId = g.Key;

                    var expected = g.Count();

                    var attended = g.Count(p =>
                        attendanceEntries.Any(a =>
                            a.User_Id == userId &&
                            a.Attendance_Register.Project_Id == p.Project_Id));

                    var rate = expected == 0
                        ? 0
                        : (double)attended / expected * 100;

                    return new
                    {
                        UserId = userId,
                        Expected = expected,
                        Attended = attended,
                        Rate = rate
                    };
                })
                .ToList();

            var usersWithPoorAttendance = attendanceByUser.Count(x =>
                x.Rate < 50);



            // -----------------------------------------
            // STUDENT / ALUMNI / STAFF
            // -----------------------------------------

            var studentUsers = students.Count;
            var alumniUsers = alumni.Count;
            var staffUsers = staff.Count;

            // -----------------------------------------
            // USERS BY YEAR OF STUDY
            // -----------------------------------------

            var usersByYear = students
                .GroupBy(s => s.Year_Of_Study)
                .Select(g => new UsersByYearDto
                {
                    Year = g.Key?.ToString() ?? "Unknown",
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ToList();

            // -----------------------------------------
            // USERS BY QUALIFICATION
            // -----------------------------------------

            var usersByQualification = students
                .Where(s => !string.IsNullOrWhiteSpace(s.Qualification))
                .GroupBy(s => s.Qualification)
                .Select(g => new UsersByQualificationDto
                {
                    Qualification = g.Key!,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            // -----------------------------------------
            // USERS BY FACULTY
            // -----------------------------------------

            var usersByFaculty = users
                .GroupBy(u => new
                {
                    u.Faculty_Id,
                    FacultyName = u.Faculty != null
                        ? u.Faculty.Name
                        : "No Faculty"
                })
                .Select(g => new UsersByFacultyDto
                {
                    FacultyId = g.Key.Faculty_Id,
                    FacultyName = g.Key.FacultyName ?? "No Faculty",
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            // -----------------------------------------
            // USERS BY PROGRAMME / DEPARTMENT
            // -----------------------------------------

            var usersByDepartment = students
                .Where(s => !string.IsNullOrWhiteSpace(s.Department))
                .GroupBy(s => s.Department)
                .Select(g => new UsersByDepartmentDto
                {
                    Department = g.Key!,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            // -----------------------------------------
            // REGISTRATION BY MONTH
            // -----------------------------------------

            var registrationsByMonth = users
                .GroupBy(u => new
                {
                    u.Date_Created.Year,
                    u.Date_Created.Month
                })
                .Select(g => new RegistrationByMonthDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(
                        g.Key.Year,
                        g.Key.Month,
                        1
                    ).ToString("MMMM"),

                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            // -----------------------------------------
            // REGISTRATION BY YEAR
            // -----------------------------------------

            var registrationsByYear = users
                .GroupBy(u => u.Date_Created.Year)
                .Select(g => new RegistrationByYearDto
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ToList();

            // -----------------------------------------
            // MOST ACTIVE USERS
            // -----------------------------------------

            var mostActiveUsers = users
                .Select(u =>
                {
                    var projects = projectCountByUser.ContainsKey(u.User_Id)
                        ? projectCountByUser[u.User_Id]
                        : 0;

                    var hours = attendanceEntries
                        .Where(a => a.User_Id == u.User_Id)
                        .Sum(a => a.Hours_Logged ?? 0);

                    return new MostActiveUserDto
                    {
                        UserId = u.User_Id,
                        Name = $"{u.First_Name} {u.Last_Name}",
                        Email = u.Email,
                        ProjectsJoined = projects,
                        Hours = hours
                    };
                })
                .OrderByDescending(x => x.ProjectsJoined)
                .ThenByDescending(x => x.Hours)
                .Take(10)
                .ToList();

            // -----------------------------------------
            // RETURN REPORT
            // -----------------------------------------

            return new UserReportDto
            {
                TotalRegisteredUsers = totalUsers,

                NewRegistrationsThisMonth = newThisMonth,

                NewRegistrationsThisYear = newThisYear,

                ActiveUsers = activeUsers,

                SuspendedInactiveUsers = suspendedInactiveUsers,

                UsersWithNoProjects = usersWithoutProjects,

                UsersWithProjects = usersWithProjects,

                AlumniUsers = alumniUsers,

                StudentUsers = studentUsers,

                StaffUsers = staffUsers,

                UsersByYear = usersByYear,

                UsersByQualification = usersByQualification,

                UsersByFaculty = usersByFaculty,

                UsersByProgramme = usersByDepartment,

                RegistrationsByMonth = registrationsByMonth,

                RegistrationsByYear = registrationsByYear,

                TotalProjectsJoined = totalProjectsJoined,

                AverageProjectsPerUser = Math.Round(
                    averageProjectsPerUser, 2),

                MostActiveUsers = mostActiveUsers,

                UsersWithZeroProjects = usersWithZeroProjects,

                UsersWithOneToTwoProjects = usersWithOneToTwoProjects,

                UsersWithThreeOrMoreProjects =
                    usersWithThreeOrMoreProjects,

                TotalVolunteerHours = Math.Round(
                    totalVolunteerHours, 2),

                AverageHoursPerUser = Math.Round(
                    averageHoursPerUser, 2),

                // Attendance calculation will be added below
                AttendanceRate = Math.Round(attendanceRate, 2),

                UsersWithPoorAttendance = usersWithPoorAttendance,

                // Based on Volunteer.Is_Completed
                UsersWhoCompletedProjects =
                    await GetCompletedUsersCount(userIds)
            };
        }

        private async Task<int> GetCompletedUsersCount(
            List<int> userIds)
        {
            return await _db.Volunteer
                .AsNoTracking()
                .CountAsync(v =>
                    userIds.Contains(v.Volunteer_Id) &&
                    v.Is_Completed);
        }
    }

}
