using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;

namespace CommunityEngagementAPI.Services
{
    public class ManagementReportService : IManagementReportService
    {
        private readonly CEAppContext _db;
        private readonly IIntelligenceService _intelligenceService;

        public ManagementReportService(
            CEAppContext db,
            IIntelligenceService intelligenceService)
        {
            _db = db;
            _intelligenceService = intelligenceService;
        }

        public async Task<ProjectReportDto> GetProjectReportAsync(int? campusId = null)
        {
            var projectsQuery = _db.Project
                .AsNoTracking()
                .AsQueryable();

            if (campusId.HasValue)
                projectsQuery = projectsQuery.Where(p => p.Campus_Id == campusId.Value);

            var projects = await projectsQuery.ToListAsync();
            var projectIds = projects.Select(p => p.Project_id).ToHashSet();

            var registrations = projectIds.Count == 0
                ? new List<Project_Register>()
                : await _db.Project_Register
                    .AsNoTracking()
                    .Where(r => projectIds.Contains(r.Project_Id))
                    .ToListAsync();

            var attendanceRows = projectIds.Count == 0
                ? new List<AttendanceRow>()
                : await (
                    from entry in _db.Attendance_Entry.AsNoTracking()
                    join register in _db.Attendance_Register.AsNoTracking()
                        on entry.Attendance_Id equals register.Attendance_Id
                    where projectIds.Contains(register.Project_Id)
                    select new AttendanceRow
                    {
                        Project_Id = register.Project_Id,
                        User_Id = entry.User_Id,
                        Hours = entry.Hours_Logged ?? 0
                    }
                ).ToListAsync();

            var campuses = await _db.Campus
                .AsNoTracking()
                .ToListAsync();

            var campusCodes = campuses
                .GroupBy(c => c.Campus_Id)
                .ToDictionary(
                    g => g.Key,
                    g => g.First().Code.ToString());

            var sdgRows = projectIds.Count == 0
                ? new List<ProjectSdgRow>()
                : await (
                    from ps in _db.Project_Sdg.AsNoTracking()
                    join sdg in _db.Sdg.AsNoTracking()
                        on ps.Sdg_Id equals sdg.Sdg_Id
                    where projectIds.Contains(ps.Project_Id)
                    select new ProjectSdgRow
                    {
                        Project_Id = ps.Project_Id,
                        Sdg_Id = sdg.Sdg_Id,
                        Sdg_Name = sdg.Name
                    }
                ).ToListAsync();

            
            var health = await _intelligenceService.GetProjectHealth();
            var scopedHealth = health
                .Where(h => projectIds.Contains(h.Project_Id))
                .ToList();

            var report = new ProjectReportDto
            {
                Total_Projects = projects.Count,
                Pending_Projects = projects.Count(p => p.Status == ProjectStatus.Pending),
                Active_Projects = projects.Count(p => p.Status == ProjectStatus.Active),
                Completed_Projects = projects.Count(p => p.Status == ProjectStatus.Completed),
                Cancelled_Projects = projects.Count(p => p.Status == ProjectStatus.Cancelled),
                Published_Projects = projects.Count(p => p.Is_Published),

                Total_Target_Volunteers = projects.Sum(p => Math.Max(p.Num_Volunteers, 0)),
                Total_Project_Registrations = registrations.Count,
                Unique_Volunteers = registrations
                    .Select(r => r.User_Id)
                    .Distinct()
                    .Count(),

                Total_Target_Hours = projects.Sum(p => Math.Max(p.Hours, 0)),
                Total_Volunteer_Hours = attendanceRows.Sum(a => a.Hours),

                Completion_Rate = projects.Count == 0
                    ? 0
                    : Round((double)projects.Count(p => p.Status == ProjectStatus.Completed) / projects.Count * 100),

                Average_Volunteers_Per_Project = projects.Count == 0
                    ? 0
                    : Round((double)registrations.Count / projects.Count),

               
                // Intelligence/Command Centre reports.
                Overdue_Reports = scopedHealth.Count(h => h.Summary_Overdue)
            };

            var registeredUsers = registrations
                .Select(r => (r.Project_Id, r.User_Id))
                .Distinct()
                .ToList();

            var attendedUsers = attendanceRows
                .Select(a => (a.Project_Id, a.User_Id))
                .Distinct()
                .ToHashSet();

            // Attendance is based on project/user pairs, not on the project date.
         
            report.Attendance_Rate = registeredUsers.Count == 0
                ? 0
                : Round((double)registeredUsers.Count(attendedUsers.Contains) / registeredUsers.Count * 100);

            report.Projects_By_Campus = projects
                .GroupBy(p => p.Campus_Id)
                .Select(g =>
                {
                    var ids = g.Select(p => p.Project_id).ToHashSet();
                    var regs = registrations.Where(r => ids.Contains(r.Project_Id)).ToList();
                    var attendance = attendanceRows.Where(a => ids.Contains(a.Project_Id)).ToList();
                    var registered = regs.Select(r => (r.Project_Id, r.User_Id)).Distinct().ToList();
                    var attended = attendance.Select(a => (a.Project_Id, a.User_Id)).Distinct().ToHashSet();

                    return new ProjectCampusReportDto
                    {
                        Campus_Id = g.Key,
                        Campus_Code = campusCodes.TryGetValue(g.Key, out var code)
                            ? code
                            : $"Campus {g.Key}",
                        Projects = g.Count(),
                        Registered_Volunteers = regs.Count,
                        Volunteer_Hours = Round(attendance.Sum(a => a.Hours)),
                        Attendance_Rate = registered.Count == 0
                            ? 0
                            : Round((double)registered.Count(attended.Contains) / registered.Count * 100),
                        Completion_Rate = g.Count() == 0
                            ? 0
                            : Round((double)g.Count(p => p.Status == ProjectStatus.Completed) / g.Count() * 100)
                    };
                })
                .OrderByDescending(x => x.Projects)
                .ToList();

            report.Projects_By_Month = projects
                .GroupBy(p => new { p.Date_Created.Year, p.Date_Created.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new ProjectMonthReportDto
                {
                    Month = $"{new DateTime(g.Key.Year, g.Key.Month, 1):MMM yyyy}",
                    Count = g.Count()
                })
                .ToList();

            report.Projects_By_Sdg = sdgRows
                .GroupBy(x => new { x.Sdg_Id, x.Sdg_Name })
                .Select(g => new ProjectSdgReportDto
                {
                    Sdg_Id = g.Key.Sdg_Id,
                    Sdg_Name = g.Key.Sdg_Name ?? "Unknown SDG",
                    Projects = g.Select(x => x.Project_Id).Distinct().Count()
                })
                .OrderByDescending(x => x.Projects)
                .ToList();

            return report;
        }

        public async Task<OverallPerformanceReportDto> GetOverallPerformanceReportAsync(int? campusId = null)
        {
            var projectReport = await GetProjectReportAsync(campusId);

            var usersQuery = _db.User
                .AsNoTracking()
                .Where(u =>
                    u.Role == UserRoleType.volunteer ||
                    u.Role == UserRoleType.assistant);

            if (campusId.HasValue)
                usersQuery = usersQuery.Where(u => u.Campus_Id == campusId.Value);

            var users = await usersQuery.ToListAsync();
            var activeVolunteers = users.Count(u => u.Is_Active && !u.Is_Suspended);

            var partnershipsQuery =
                from partnership in _db.NGO_Partnership.AsNoTracking()
                join application in _db.NGO_Application.AsNoTracking()
                    on partnership.Application_Id equals application.Application_Id
                join specialist in _db.User.AsNoTracking()
                    on application.Specialist_Id equals specialist.User_Id
                where partnership.Is_Active
                select new
                {
                    partnership.Partnership_Id,
                    specialist.Campus_Id
                };

            if (campusId.HasValue)
                partnershipsQuery = partnershipsQuery.Where(x => x.Campus_Id == campusId.Value);

            var activePartnerships = await partnershipsQuery.CountAsync();

            var targetAchievement = projectReport.Total_Target_Volunteers == 0
                ? 0
                : Math.Min(
                    100,
                    (double)projectReport.Total_Project_Registrations /
                    projectReport.Total_Target_Volunteers * 100);

            var completion = projectReport.Completion_Rate;
            var attendance = projectReport.Attendance_Rate;
            var participation = projectReport.Total_Projects == 0
                ? 0
                : Math.Min(100, projectReport.Average_Volunteers_Per_Project * 20);

            var overallScore = Round(
                completion * 0.30 +
                attendance * 0.30 +
                targetAchievement * 0.25 +
                participation * 0.15);

           
            var health = await _intelligenceService.GetProjectHealth();
            var projectIds = await GetScopedProjectIdsAsync(campusId);
            var scopedHealth = health
                .Where(h => projectIds.Contains(h.Project_Id))
                .ToList();

            var projectsNeedingAttention = scopedHealth.Count(h => h.Health_Score < 80);

            var result = new OverallPerformanceReportDto
            {
                Overall_Score = overallScore,
                Performance_Status = GetPerformanceStatus(overallScore),

                Total_Projects = projectReport.Total_Projects,
                Completed_Projects = projectReport.Completed_Projects,
                Active_Volunteers = activeVolunteers,
                Unique_Volunteers = projectReport.Unique_Volunteers,
                Volunteer_Hours = projectReport.Total_Volunteer_Hours,
                Attendance_Rate = projectReport.Attendance_Rate,
                Completion_Rate = projectReport.Completion_Rate,
                Volunteer_Target_Achievement = Round(targetAchievement),
                Projects_Needing_Attention = projectsNeedingAttention,
                Overdue_Reports = scopedHealth.Count(h => h.Summary_Overdue),
                Active_Partnerships = activePartnerships
            };

            result.Dimensions = new List<PerformanceDimensionDto>
            {
                new() { Name = "Completion", Score = completion },
                new() { Name = "Attendance", Score = attendance },
                new() { Name = "Participation", Score = participation },
                new() { Name = "Volunteer Target", Score = targetAchievement }
            };

            result.Campus_Performance = await BuildCampusPerformanceAsync(null);
            result.Performance_Trend = projectReport.Projects_By_Month
                .Select(x => new PerformanceTrendDto
                {
                    Period = x.Month,
                    Projects = x.Count,
                    Score = projectReport.Total_Projects == 0
                        ? 0
                        : Round((double)x.Count / projectReport.Total_Projects * 100)
                })
                .ToList();

            BuildInsightsAndRecommendations(result);
            return result;
        }

        private async Task<HashSet<int>> GetScopedProjectIdsAsync(int? campusId)
        {
            var query = _db.Project.AsNoTracking().AsQueryable();

            if (campusId.HasValue)
                query = query.Where(p => p.Campus_Id == campusId.Value);

            return (await query.Select(p => p.Project_id).ToListAsync()).ToHashSet();
        }

        private async Task<List<PerformanceCampusDto>> BuildCampusPerformanceAsync(int? selectedCampusId)
        {
            var campusesQuery = _db.Campus.AsNoTracking().AsQueryable();

            if (selectedCampusId.HasValue)
                campusesQuery = campusesQuery.Where(c => c.Campus_Id == selectedCampusId.Value);

            var campuses = await campusesQuery.ToListAsync();
            var result = new List<PerformanceCampusDto>();

            foreach (var campus in campuses)
            {
                var report = await GetProjectReportAsync(campus.Campus_Id);

                result.Add(new PerformanceCampusDto
                {
                    Campus_Id = campus.Campus_Id,
                    Campus_Code = campus.Code.ToString(),
                    Performance_Score = CalculateSimpleScore(report),
                    Projects = report.Total_Projects,
                    Volunteers = report.Unique_Volunteers,
                    Attendance_Rate = report.Attendance_Rate
                });
            }

            return result
                .OrderByDescending(x => x.Performance_Score)
                .ToList();
        }

        private static void BuildInsightsAndRecommendations(OverallPerformanceReportDto result)
        {
            if (result.Total_Projects == 0)
            {
                result.Insights.Add("There are no projects in the selected scope.");
                result.Recommendations.Add("Create and publish projects so engagement activity can be measured.");
                return;
            }

            if (result.Completion_Rate >= 75)
                result.Insights.Add("Project completion is strong in the selected scope.");
            else
                result.Insights.Add("Project completion is below the level expected for a healthy programme.");

            if (result.Attendance_Rate >= 80)
                result.Insights.Add("Volunteer attendance is strong.");
            else if (result.Attendance_Rate > 0)
                result.Insights.Add("Volunteer attendance is a key area requiring attention.");

            if (result.Volunteer_Target_Achievement < 60)
            {
                result.Insights.Add("Project registrations are below the combined volunteer targets.");
                result.Recommendations.Add("Increase volunteer recruitment and targeted communication for projects with low registration.");
            }

            if (result.Overdue_Reports > 0)
            {
                result.Insights.Add($"{result.Overdue_Reports} project report summary(ies) are overdue.");
                result.Recommendations.Add("Prioritise outstanding project summaries so completed project evidence is up to date.");
            }

            if (result.Projects_Needing_Attention > 0)
            {
                result.Recommendations.Add("Review the highest-priority project health issues first and follow the recommended actions in Command Centre.");
            }

            if (result.Active_Volunteers == 0)
                result.Recommendations.Add("Review volunteer onboarding and campus outreach because no active volunteer accounts were found.");

            if (result.Active_Partnerships == 0)
                result.Recommendations.Add("Review NGO partnership activity and identify opportunities for new or renewed partnerships.");

            if (result.Attendance_Rate < 60 && result.Attendance_Rate > 0)
                result.Recommendations.Add("Investigate attendance barriers and improve reminders, scheduling and project communication.");

            if (result.Performance_Status == "Strong")
                result.Recommendations.Add("Maintain current delivery practices and replicate successful approaches across weaker campuses.");
        }

        private static double CalculateSimpleScore(ProjectReportDto report)
        {
            var targetAchievement = report.Total_Target_Volunteers == 0
                ? 0
                : Math.Min(100, (double)report.Total_Project_Registrations / report.Total_Target_Volunteers * 100);

            var participation = report.Total_Projects == 0
                ? 0
                : Math.Min(100, report.Average_Volunteers_Per_Project * 20);

            return Round(
                report.Completion_Rate * 0.30 +
                report.Attendance_Rate * 0.30 +
                targetAchievement * 0.25 +
                participation * 0.15);
        }

        private static double Round(double value) =>
            Math.Round(value, 1, MidpointRounding.AwayFromZero);

        private static string GetPerformanceStatus(double score)
        {
            if (score >= 75) return "Strong";
            if (score >= 50) return "Attention";
            return "Critical";
        }

        private sealed class AttendanceRow
        {
            public int Project_Id { get; set; }
            public int User_Id { get; set; }
            public double Hours { get; set; }
        }

        private sealed class ProjectSdgRow
        {
            public int Project_Id { get; set; }
            public int Sdg_Id { get; set; }
            public string? Sdg_Name { get; set; }
        }
    }
}
