using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CommunityEngagementAPI.Services
{
    public interface IIntelligenceService
    {
        Task<List<ProjectHealthDto>> GetProjectHealth(int? projectId = null);

        Task<List<ProjectProblemDto>> GetProjectProblems();

        Task<List<ProjectRecommendationDto>> GetProjectRecommendations();

        Task<CommunityImpactDto> GetCommunityImpact(int days = 30);

        Task<List<AreaPerformanceDto>> GetAreaPerformance();

        Task<IntelligenceDashboardDto> GetDashboard(int? campusId = null);

        Task<SpecialistIntelligenceDashboardDto> GetSpecialistDashboard(
            int specialistId,
            int days = 30);

        Task<ProjectIntelligenceDto> GetProjectIntelligence(int projectId);

        Task<AssistantIntelligenceDashboardDto> GetAssistantDashboard(
            int assistantId);

        Task<VolunteerIntelligenceDashboardDto> GetVolunteerDashboard(
            int volunteerId);

        Task ProcessAutomaticProjectChecks(
            CancellationToken cancellationToken);
    }


    public class IntelligenceService : IIntelligenceService
    {
        private readonly CEAppContext _db;
        private readonly INotificationService _notificationService;

        public IntelligenceService(
            CEAppContext db,
            INotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }


        // ============================================================
        // PROJECT HEALTH
        // ============================================================

        public async Task<List<ProjectHealthDto>> GetProjectHealth(
            int? projectId = null)
        {
            var query = _db.Project
                .Include(p => p.ProjectRegisters)
                .Include(p => p.AttendanceRegisters)
                .AsQueryable();

            if (projectId.HasValue)
            {
                query = query.Where(
                    p => p.Project_id == projectId.Value);
            }

            var projects = await query
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            if (!projects.Any())
            {
                return new List<ProjectHealthDto>();
            }

            var projectIds = projects
                .Select(p => p.Project_id)
                .ToList();

            // --------------------------------------------------------
            // ATTENDANCE
            // --------------------------------------------------------

            var attendance = await _db.Attendance_Entry
                .Where(a =>
                    projectIds.Contains(
                        a.Attendance_Register.Project_Id))
                .Select(a => new
                {
                    Project_Id =
                        a.Attendance_Register.Project_Id,

                    a.User_Id,

                    a.Hours_Logged
                })
                .ToListAsync();


            // --------------------------------------------------------
            // PROJECT SUMMARIES
            // --------------------------------------------------------

            var summaries = await _db.Project_Summary
                .Where(s =>
                    projectIds.Contains(s.Project_Id))
                .Select(s => s.Project_Id)
                .Distinct()
                .ToListAsync();

            var summaryIds = summaries.ToHashSet();


            var today =
                DateOnly.FromDateTime(DateTime.Today);

            var result =
                new List<ProjectHealthDto>();


            foreach (var project in projects)
            {
                // ====================================================
                // BASIC PROJECT INFORMATION
                // ====================================================

                var registered =
                    project.ProjectRegisters
                        .Count(r => r.Is_Committed);

                var projectAttendance =
                    attendance
                        .Where(a =>
                            a.Project_Id ==
                            project.Project_id)
                        .ToList();

                var attended =
                    projectAttendance
                        .Select(a => a.User_Id)
                        .Distinct()
                        .Count();

                var hours =
                    projectAttendance
                        .Sum(a => a.Hours_Logged ?? 0);

                var hasSummary =
                    summaryIds.Contains(
                        project.Project_id);


                // ====================================================
                // PROJECT LIFECYCLE
                // ====================================================

                var isCompleted =
                    project.Status ==
                    ProjectStatus.Completed;

                var isCancelled =
                    project.Status ==
                    ProjectStatus.Cancelled;

                var isFuture =
                    project.Date > today;

                var isToday =
                    project.Date == today;

                var isPast =
                    project.Date < today;

                var hasStarted =
                    isToday || isPast;


                // ====================================================
                // ATTENDANCE
                // ====================================================

                double attendanceRate = 0;

                if (hasStarted &&
                    !isCancelled &&
                    registered > 0)
                {
                    attendanceRate =
                        (double)attended /
                        registered *
                        100;
                }


                // ====================================================
                // ISSUES
                // ====================================================

                var issues =
                    new List<ProjectIssue>();

                var limitations =
                    new List<string>();


                // ====================================================
                // COMPLETED PROJECT
                // ====================================================

                if (isCompleted)
                {
                    /*
                     * A completed project is no longer an
                     * operational project.
                     *
                     * Therefore:
                     *
                     * - Do NOT complain that it is overdue.
                     * - Do NOT complain about low registration.
                     * - Do NOT complain about attendance.
                     *
                     * Those are historical limitations.
                     *
                     * The only current problem here is a
                     * missing Project Summary.
                     */

                    if (!hasSummary)
                    {
                        issues.Add(new ProjectIssue
                        {
                            Priority = 2,

                            Severity = "Attention",

                            Problem =
                                "Project summary is missing.",

                            Recommendation =
                                "Submit the project summary for this completed project."
                        });
                    }


                    // ------------------------------------------------
                    // HISTORICAL LIMITATIONS
                    // ------------------------------------------------

                    if (project.Num_Volunteers > 0)
                    {
                        var fillRate =
                            (double)registered /
                            project.Num_Volunteers;

                        if (fillRate < 0.8)
                        {
                            limitations.Add(
                                $"Volunteer participation reached only " +
                                $"{Math.Round(fillRate * 100, 1)}% of the target."
                            );
                        }
                    }


                    if (registered > 0 &&
                        attendanceRate < 80)
                    {
                        limitations.Add(
                            $"Attendance was " +
                            $"{Math.Round(attendanceRate, 1)}% " +
                            $"of committed volunteers."
                        );
                    }
                }


                // ====================================================
                // CANCELLED PROJECT
                // ====================================================

                else if (isCancelled)
                {
                    /*
                     * A cancelled project should not appear as
                     * a Critical operational project.
                     *
                     * It is simply cancelled.
                     */

                    attendanceRate = 0;
                }


                // ====================================================
                // FUTURE PROJECT
                // ====================================================

                else if (isFuture)
                {
                    /*
                     * A future project has not started.
                     *
                     * THAT IS NORMAL.
                     *
                     * Therefore:
                     *
                     * DO NOT add:
                     * "Project has not started."
                     */

                    attendanceRate = 0;

                    var daysUntil =
                        project.Date.DayNumber -
                        today.DayNumber;


                    // ------------------------------------------------
                    // VOLUNTEER RECRUITMENT
                    // ------------------------------------------------

                    if (project.Num_Volunteers > 0 &&
                        registered < project.Num_Volunteers &&
                        daysUntil <= 3)
                    {
                        var fillRate =
                            (double)registered /
                            project.Num_Volunteers;


                        if (fillRate < 0.5)
                        {
                            issues.Add(new ProjectIssue
                            {
                                Priority = 2,

                                Severity = "High",

                                Problem =
                                    "Volunteer registration is below 50% of the target and the project is approaching.",

                                Recommendation =
                                    "Increase volunteer recruitment before the project date."
                            });
                        }
                        else if (fillRate < 0.8)
                        {
                            issues.Add(new ProjectIssue
                            {
                                Priority = 3,

                                Severity = "Attention",

                                Problem =
                                    "Volunteer registration is below 80% of the target.",

                                Recommendation =
                                    "Encourage more volunteers to register before the project date."
                            });
                        }
                    }
                }


                // ====================================================
                // ACTIVE / PAST BUT NOT COMPLETED
                // ====================================================

                else
                {
                    /*
                     * At this point:
                     *
                     * - Not completed
                     * - Not cancelled
                     * - Not future
                     *
                     * Therefore the project is either:
                     *
                     * - happening today
                     * - or its date has already passed.
                     */


                    // ------------------------------------------------
                    // PROJECT DATE PASSED
                    // ------------------------------------------------

                    if (isPast)
                    {
                        issues.Add(new ProjectIssue
                        {
                            Priority = 1,

                            Severity = "Critical",

                            Problem =
                                "Project date has passed without completion.",

                            Recommendation =
                                "Review the project status and update the project record."
                        });
                    }


                    // ------------------------------------------------
                    // VOLUNTEER REGISTRATION
                    // ------------------------------------------------

                    if (project.Num_Volunteers > 0 &&
                        registered < project.Num_Volunteers)
                    {
                        var fillRate =
                            (double)registered /
                            project.Num_Volunteers;


                        if (fillRate < 0.5)
                        {
                            issues.Add(new ProjectIssue
                            {
                                Priority = 2,

                                Severity = "High",

                                Problem =
                                    "Volunteer registration is below 50% of the target.",

                                Recommendation =
                                    "Increase volunteer recruitment or review the project capacity."
                            });
                        }
                        else if (fillRate < 0.8)
                        {
                            issues.Add(new ProjectIssue
                            {
                                Priority = 3,

                                Severity = "Attention",

                                Problem =
                                    "Volunteer registration is below 80% of the target.",

                                Recommendation =
                                    "Encourage more volunteers to participate."
                            });
                        }
                    }


                    // ------------------------------------------------
                    // ATTENDANCE
                    // ------------------------------------------------

                    if (hasStarted &&
                        registered > 0)
                    {
                        if (attendanceRate < 50)
                        {
                            issues.Add(new ProjectIssue
                            {
                                Priority = 2,

                                Severity = "High",

                                Problem =
                                    "Attendance is below 50% of committed volunteers.",

                                Recommendation =
                                    "Investigate the attendance drop and follow up with registered volunteers."
                            });
                        }
                        else if (attendanceRate < 80)
                        {
                            issues.Add(new ProjectIssue
                            {
                                Priority = 3,

                                Severity = "Attention",

                                Problem =
                                    "Attendance is below 80% of committed volunteers.",

                                Recommendation =
                                    "Follow up with volunteers who registered but did not attend."
                            });
                        }
                    }


                    // ------------------------------------------------
                    // MISSING PROJECT SUMMARY
                    // ------------------------------------------------

                    if (isPast && !hasSummary)
                    {
                        issues.Add(new ProjectIssue
                        {
                            Priority = 2,

                            Severity = "High",

                            Problem =
                                "Project summary is missing.",

                            Recommendation =
                                "Submit the project summary for the completed project."
                        });
                    }
                }


                // ====================================================
                // ORDER ISSUES
                // ====================================================

                var orderedIssues =
                    issues
                        .OrderBy(i => i.Priority)
                        .ThenBy(i => i.Problem)
                        .ToList();


                // ====================================================
                // HEALTH SCORE
                // ====================================================

                var score = 100;


                foreach (var issue in orderedIssues)
                {
                    score -= issue.Priority switch
                    {
                        1 => 35,
                        2 => 20,
                        3 => 10,
                        4 => 5,
                        _ => 0
                    };
                }


                score =
                    Math.Max(
                        0,
                        Math.Min(100, score));


                // ====================================================
                // HEALTH STATUS
                // ====================================================

                string healthStatus;

                if (isCancelled)
                {
                    healthStatus = "Cancelled";
                }
                else if (score < 60)
                {
                    healthStatus = "Critical";
                }
                else if (score < 80)
                {
                    healthStatus = "Attention";
                }
                else
                {
                    healthStatus = "On Track";
                }


                // ====================================================
                // PRIMARY PROBLEM
                // ====================================================

                var primaryIssue =
                    orderedIssues.FirstOrDefault();


                // ====================================================
                // SUMMARY OVERDUE
                // ====================================================

                var summaryOverdue =
                    !isCancelled &&
                    !hasSummary &&
                    (
                        isPast ||
                        isCompleted
                    );


                // ====================================================
                // DTO
                // ====================================================

                result.Add(new ProjectHealthDto
                {
                    Project_Id =
                        project.Project_id,

                    Project_Name =
                        project.Title,

                    Status =
                        project.Status.ToString(),

                    Health_Score =
                        score,

                    Health_Status =
                        healthStatus,

                    Registered_Volunteers =
                        registered,

                    Expected_Volunteers =
                        project.Num_Volunteers,

                    Attended_Volunteers =
                        attended,

                    Attendance_Rate =
                        Math.Round(
                            attendanceRate,
                            2),

                    Hours_Completed =
                        Math.Round(
                            hours,
                            2),

                    Hours_Target =
                        project.Hours,

                    Summary_Generated =
                        hasSummary,

                    Report_Submitted =
                        hasSummary,

                    Problems =
                        orderedIssues
                            .Select(i => i.Problem)
                            .Distinct()
                            .ToList(),

                    Recommendations =
                        orderedIssues
                            .Select(i => i.Recommendation)
                            .Distinct()
                            .ToList(),

                    Primary_Problem =
                        primaryIssue?.Problem,

                    Primary_Recommendation =
                        primaryIssue?.Recommendation,

                    Primary_Priority =
                        primaryIssue?.Priority,

                    Limitations =
                        limitations
                            .Distinct()
                            .ToList(),

                    Attendance_Applicable =
                        hasStarted &&
                        !isCancelled,

                    Summary_Overdue =
                        summaryOverdue
                });
            }

            return result;
        }


        // ============================================================
        // PROJECT PROBLEMS
        // ============================================================

        public async Task<List<ProjectProblemDto>>
            GetProjectProblems()
        {
            var health =
                await GetProjectHealth();


            /*
             * IMPORTANT:
             *
             * Command Centre should show only ONE
             * primary problem per project.
             *
             * The full list remains available through
             * GetProjectIntelligence().
             */

            return health
                .Where(p =>
                    !string.IsNullOrWhiteSpace(
                        p.Primary_Problem))
                .Select(p => new ProjectProblemDto
                {
                    Project_Id =
                        p.Project_Id,

                    Project_Name =
                        p.Project_Name,

                    Severity =
                        p.Health_Status,

                    Problem =
                        p.Primary_Problem!,

                    Recommended_Action =
                        p.Primary_Recommendation
                        ?? "Review the project and take the appropriate corrective action."
                })
                .OrderBy(p =>
                    GetPriorityFromSeverity(
                        p.Severity))
                .ThenBy(p => p.Project_Name)
                .ToList();
        }


        // ============================================================
        // PROJECT RECOMMENDATIONS
        // ============================================================

        public async Task<List<ProjectRecommendationDto>>
            GetProjectRecommendations()
        {
            var health =
                await GetProjectHealth();


            /*
             * One primary recommendation per project
             * for the Command Centre.
             *
             * Full recommendations are still available
             * in Project Intelligence.
             */

            return health
                .Where(p =>
                    !string.IsNullOrWhiteSpace(
                        p.Primary_Recommendation))
                .Select(p => new ProjectRecommendationDto
                {
                    Project_Id =
                        p.Project_Id,

                    Project_Name =
                        p.Project_Name,

                    Priority =
                        p.Health_Status,

                    Recommendation =
                        p.Primary_Recommendation!,

                    Reason =
                        p.Primary_Problem
                        ?? "Project activity indicates that attention may be required."
                })
                .OrderBy(p =>
                    GetPriorityFromSeverity(
                        p.Priority))
                .ThenBy(p => p.Project_Name)
                .ToList();
        }


        // ============================================================
        // COMMUNITY IMPACT
        // ============================================================

        public async Task<CommunityImpactDto>
            GetCommunityImpact(int days = 30)
        {
            if (days < 1)
                days = 30;

            if (days > 3650)
                days = 3650;


            var to = DateTime.Now;

            var from =
                to.AddDays(-days);

            var fromDate =
                DateOnly.FromDateTime(from);

            var toDate =
                DateOnly.FromDateTime(to);


            var projects =
                await _db.Project
                    .Where(p =>
                        p.Date >= fromDate &&
                        p.Date <= toDate)
                    .ToListAsync();


            var projectIds =
                projects
                    .Select(p => p.Project_id)
                    .ToList();


            if (!projectIds.Any())
            {
                return new CommunityImpactDto
                {
                    From = from,
                    To = to,
                    Projects_Completed = 0,
                    Active_Projects = 0,
                    Volunteers_Involved = 0,
                    Volunteer_Hours = 0,
                    Attendance_Entries = 0,
                    Sdg_Count = 0,
                    Sdg_Impact = new List<SdgImpactDto>()
                };
            }


            var attendance =
                await _db.Attendance_Entry
                    .Where(a =>
                        projectIds.Contains(
                            a.Attendance_Register.Project_Id))
                    .Select(a => new
                    {
                        Project_Id =
                            a.Attendance_Register.Project_Id,

                        a.User_Id,

                        a.Hours_Logged
                    })
                    .ToListAsync();


            var sdgData =
                await _db.Project_Sdg
                    .Include(p => p.Sdg)
                    .Where(p =>
                        projectIds.Contains(
                            p.Project_Id))
                    .ToListAsync();


            var sdgImpact =
                sdgData
                    .GroupBy(p => new
                    {
                        p.Sdg_Id,
                        p.Sdg.Name
                    })
                    .Select(g =>
                    {
                        var ids =
                            g.Select(x => x.Project_Id)
                                .Distinct()
                                .ToList();

                        var rows =
                            attendance
                                .Where(a =>
                                    ids.Contains(
                                        a.Project_Id))
                                .ToList();

                        return new SdgImpactDto
                        {
                            Sdg_Id =
                                g.Key.Sdg_Id,

                            Sdg_Name =
                                g.Key.Name,

                            Projects =
                                ids.Count,

                            Volunteers =
                                rows
                                    .Select(a => a.User_Id)
                                    .Distinct()
                                    .Count(),

                            Hours =
                                Math.Round(
                                    rows.Sum(
                                        a => a.Hours_Logged ?? 0),
                                    2)
                        };
                    })
                    .OrderByDescending(x => x.Hours)
                    .ToList();


            return new CommunityImpactDto
            {
                From = from,

                To = to,

                Projects_Completed =
                    projects.Count(
                        p =>
                            p.Status ==
                            ProjectStatus.Completed),

                Active_Projects =
                    projects.Count(
                        p =>
                            p.Status ==
                            ProjectStatus.Active),

                Volunteers_Involved =
                    attendance
                        .Select(a => a.User_Id)
                        .Distinct()
                        .Count(),

                Volunteer_Hours =
                    (int)Math.Round(
                        attendance.Sum(
                            a => a.Hours_Logged ?? 0)),

                Attendance_Entries =
                    attendance.Count,

                Sdg_Count =
                    sdgImpact.Count,

                Sdg_Impact =
                    sdgImpact
            };
        }


        // ============================================================
        // AREA / FACULTY PERFORMANCE
        // ============================================================

        public async Task<List<AreaPerformanceDto>>
            GetAreaPerformance()
        {
            var faculties =
                await _db.Faculty
                    .ToListAsync();


            var users =
                await _db.User
                    .Where(u =>
                        u.Faculty_Id != null)
                    .Select(u => new
                    {
                        u.User_Id,
                        u.Faculty_Id
                    })
                    .ToListAsync();


            var registrations =
                await _db.Project_Register
                    .Where(r =>
                        r.Is_Committed)
                    .Select(r => new
                    {
                        r.Project_Id,
                        r.User_Id
                    })
                    .ToListAsync();


            var attendance =
                await _db.Attendance_Entry
                    .Select(a => new
                    {
                        Project_Id =
                            a.Attendance_Register.Project_Id,

                        a.User_Id
                    })
                    .ToListAsync();


            var health =
                await GetProjectHealth();


            var result =
                new List<AreaPerformanceDto>();


            foreach (var faculty in faculties)
            {
                var facultyUsers =
                    users
                        .Where(u =>
                            u.Faculty_Id ==
                            faculty.Faculty_Id)
                        .Select(u => u.User_Id)
                        .ToHashSet();


                var projectIds =
                    registrations
                        .Where(r =>
                            facultyUsers.Contains(
                                r.User_Id))
                        .Select(r => r.Project_Id)
                        .Distinct()
                        .ToList();


                if (!projectIds.Any())
                    continue;


                var facultyRegistrations =
                    registrations
                        .Where(r =>
                            projectIds.Contains(
                                r.Project_Id) &&
                            facultyUsers.Contains(
                                r.User_Id))
                        .ToList();


                var facultyAttendance =
                    attendance
                        .Where(a =>
                            projectIds.Contains(
                                a.Project_Id) &&
                            facultyUsers.Contains(
                                a.User_Id))
                        .ToList();


                var rate =
                    facultyRegistrations.Count > 0
                        ? (double)
                            facultyAttendance
                                .Select(a => a.User_Id)
                                .Distinct()
                                .Count()
                            /
                            facultyRegistrations
                                .Select(a => a.User_Id)
                                .Distinct()
                                .Count()
                            * 100
                        : 0;


                var healthValues =
                    health
                        .Where(h =>
                            projectIds.Contains(
                                h.Project_Id))
                        .Select(h =>
                            h.Health_Score)
                        .ToList();


                var averageHealth =
                    healthValues.Any()
                        ? healthValues.Average()
                        : 0;


                var reasons =
                    new List<string>();


                if (rate < 60)
                {
                    reasons.Add(
                        "Low participation or attendance.");
                }


                if (averageHealth < 60)
                {
                    reasons.Add(
                        "Projects involving this faculty have low average health.");
                }


                result.Add(
                    new AreaPerformanceDto
                    {
                        Area_Type =
                            "Faculty",

                        Area_Id =
                            faculty.Faculty_Id,

                        Area_Name =
                            faculty.Name ??
                            "Unknown Faculty",

                        Projects =
                            projectIds.Count,

                        Completed_Projects =
                            health.Count(
                                h =>
                                    projectIds.Contains(
                                        h.Project_Id) &&
                                    h.Status ==
                                        "Completed"),

                        Volunteers =
                            facultyRegistrations
                                .Select(r => r.User_Id)
                                .Distinct()
                                .Count(),

                        Attendance_Rate =
                            Math.Round(rate, 2),

                        Average_Project_Health =
                            Math.Round(
                                averageHealth,
                                2),

                        Performance =
                            averageHealth >= 80 &&
                            rate >= 80
                                ? "Strong"
                                : averageHealth >= 60 &&
                                  rate >= 60
                                    ? "Attention"
                                    : "Underperforming",

                        Reasons =
                            reasons
                    });
            }


            return result
                .OrderBy(x =>
                    x.Performance ==
                        "Underperforming"
                        ? 0
                        : x.Performance ==
                          "Attention"
                            ? 1
                            : 2)
                .ThenBy(x =>
                    x.Average_Project_Health)
                .ToList();
        }


        // ============================================================
        // DIRECTOR DASHBOARD
        // ============================================================

        public async Task<IntelligenceDashboardDto>
            GetDashboard(int? campusId = null)
        {
            var health =
                await GetProjectHealth();


            // --------------------------------------------------------
            // CAMPUS FILTER
            // --------------------------------------------------------

            if (campusId.HasValue)
            {
                var campusProjectIds =
                    await _db.Project
                        .Where(p =>
                            p.Campus_Id ==
                            campusId.Value)
                        .Select(p =>
                            p.Project_id)
                        .ToListAsync();


                health =
                    health
                        .Where(h =>
                            campusProjectIds.Contains(
                                h.Project_Id))
                        .ToList();
            }


            var problems =
                health
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.Primary_Problem))
                    .Select(p =>
                        new ProjectProblemDto
                        {
                            Project_Id =
                                p.Project_Id,

                            Project_Name =
                                p.Project_Name,

                            Severity =
                                p.Health_Status,

                            Problem =
                                p.Primary_Problem!,

                            Recommended_Action =
                                p.Primary_Recommendation
                                ??
                                "Review the project and take the appropriate corrective action."
                        })
                    .OrderBy(p =>
                        GetPriorityFromSeverity(
                            p.Severity))
                    .ThenBy(p =>
                        p.Project_Name)
                    .ToList();


            var recommendations =
                health
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.Primary_Recommendation))
                    .Select(p =>
                        new ProjectRecommendationDto
                        {
                            Project_Id =
                                p.Project_Id,

                            Project_Name =
                                p.Project_Name,

                            Priority =
                                p.Health_Status,

                            Recommendation =
                                p.Primary_Recommendation!,

                            Reason =
                                p.Primary_Problem
                                ??
                                "Project activity indicates that attention may be required."
                        })
                    .OrderBy(p =>
                        GetPriorityFromSeverity(
                            p.Priority))
                    .ThenBy(p =>
                        p.Project_Name)
                    .ToList();


            var impact =
                await GetCommunityImpact(30);


            var areas =
                await GetAreaPerformance();


            // --------------------------------------------------------
            // OVERDUE SUMMARIES
            // --------------------------------------------------------

            var overdueSummaries =
                health.Count(
                    h =>
                        h.Summary_Overdue);


            // --------------------------------------------------------
            // LOW PARTICIPATION
            // --------------------------------------------------------

            var lowParticipation =
                health.Count(
                    h =>
                        h.Attendance_Applicable &&
                        h.Registered_Volunteers > 0 &&
                        h.Attendance_Rate < 50);


            return new IntelligenceDashboardDto
            {
                Projects_Requiring_Attention =
                    health.Count(
                        h =>
                            h.Health_Status ==
                                "Attention" ||
                            h.Health_Status ==
                                "Critical"),

                Critical_Projects =
                    health.Count(
                        h =>
                            h.Health_Status ==
                            "Critical"),

                Projects_On_Track =
                    health.Count(
                        h =>
                            h.Health_Status ==
                            "On Track"),

                Overdue_Reports =
                    overdueSummaries,

                Low_Participation_Projects =
                    lowParticipation,

                Overall_Project_Health =
                    health.Any()
                        ? Math.Round(
                            health.Average(
                                h => h.Health_Score),
                            2)
                        : 0,

                Impact =
                    impact,

                /*
                 * Command Centre gets only the highest
                 * priority problem for each project.
                 */

                Problems =
                    problems
                        .Take(20)
                        .ToList(),

                Recommendations =
                    recommendations
                        .Take(20)
                        .ToList(),

                Areas_Requiring_Attention =
                    areas
                        .Where(a =>
                            a.Performance !=
                            "Strong")
                        .Take(10)
                        .ToList()
            };
        }


        // ============================================================
        // SPECIALIST DASHBOARD
        // ============================================================

        public async Task<SpecialistIntelligenceDashboardDto>
            GetSpecialistDashboard(
                int specialistId,
                int days = 30)
        {
            if (days < 1)
                days = 30;

            if (days > 3650)
                days = 3650;


            var specialist =
                await _db.User
                    .Include(u => u.Campus)
                    .FirstOrDefaultAsync(
                        u =>
                            u.User_Id ==
                            specialistId &&
                            u.Role ==
                            UserRoleType.specialist);


            if (specialist == null)
            {
                throw new KeyNotFoundException(
                    "Specialist not found.");
            }


            if (!specialist.Campus_Id.HasValue ||
                specialist.Campus == null)
            {
                throw new InvalidOperationException(
                    "The specialist is not assigned to a campus.");
            }


            var campusId =
                specialist.Campus_Id.Value;


            // --------------------------------------------------------
            // CAMPUS PROJECTS
            // --------------------------------------------------------

            var campusProjects =
                await _db.Project
                    .Where(p =>
                        p.Campus_Id ==
                        campusId)
                    .Select(p =>
                        p.Project_id)
                    .ToListAsync();


            // --------------------------------------------------------
            // SPECIALIST'S OWN PROJECTS
            // --------------------------------------------------------

            var myProjectIds =
                await _db.Project
                    .Where(p =>
                        p.Campus_Id ==
                            campusId &&
                        p.User_Id ==
                            specialistId)
                    .Select(p =>
                        p.Project_id)
                    .ToListAsync();


            var myProjectIdSet =
                myProjectIds.ToHashSet();


            var allHealth =
                await GetProjectHealth();


            var campusHealth =
                allHealth
                    .Where(p =>
                        campusProjects.Contains(
                            p.Project_Id))
                    .OrderBy(p =>
                        p.Primary_Priority ??
                        99)
                    .ThenBy(p =>
                        p.Health_Score)
                    .ThenBy(p =>
                        p.Project_Name)
                    .ToList();


            var myHealth =
                campusHealth
                    .Where(p =>
                        myProjectIdSet.Contains(
                            p.Project_Id))
                    .ToList();


            // --------------------------------------------------------
            // CAMPUS PROBLEMS
            // --------------------------------------------------------

            var campusProblems =
                campusHealth
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.Primary_Problem))
                    .Select(p =>
                        new ProjectProblemDto
                        {
                            Project_Id =
                                p.Project_Id,

                            Project_Name =
                                p.Project_Name,

                            Severity =
                                p.Health_Status,

                            Problem =
                                p.Primary_Problem!,

                            Recommended_Action =
                                p.Primary_Recommendation
                                ??
                                "Review the project and take the appropriate corrective action."
                        })
                    .OrderBy(p =>
                        GetPriorityFromSeverity(
                            p.Severity))
                    .ThenBy(p =>
                        p.Project_Name)
                    .ToList();


            // --------------------------------------------------------
            // CAMPUS RECOMMENDATIONS
            // --------------------------------------------------------

            var campusRecommendations =
                campusHealth
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.Primary_Recommendation))
                    .Select(p =>
                        new ProjectRecommendationDto
                        {
                            Project_Id =
                                p.Project_Id,

                            Project_Name =
                                p.Project_Name,

                            Priority =
                                p.Health_Status,

                            Recommendation =
                                p.Primary_Recommendation!,

                            Reason =
                                p.Primary_Problem
                                ??
                                "Project activity indicates that attention may be required."
                        })
                    .OrderBy(p =>
                        GetPriorityFromSeverity(
                            p.Priority))
                    .ThenBy(p =>
                        p.Project_Name)
                    .ToList();


            var myProblems =
                campusProblems
                    .Where(p =>
                        myProjectIdSet.Contains(
                            p.Project_Id))
                    .ToList();


            var myRecommendations =
                campusRecommendations
                    .Where(p =>
                        myProjectIdSet.Contains(
                            p.Project_Id))
                    .ToList();


            // --------------------------------------------------------
            // CAMPUS VOLUNTEERS
            // --------------------------------------------------------

            var volunteers =
                await _db.User
                    .Where(u =>
                        u.Campus_Id ==
                            campusId &&
                        u.Role ==
                            UserRoleType.volunteer &&
                        u.Is_Active &&
                        !u.Is_Suspended)
                    .Select(u =>
                        u.User_Id)
                    .ToListAsync();


            // --------------------------------------------------------
            // PERIOD
            // --------------------------------------------------------

            var fromDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(-days));

            var toDate =
                DateOnly.FromDateTime(
                    DateTime.Now);


            var periodProjectIds =
                await _db.Project
                    .Where(p =>
                        p.Campus_Id ==
                            campusId &&
                        p.Date >=
                            fromDate &&
                        p.Date <=
                            toDate)
                    .Select(p =>
                        p.Project_id)
                    .ToListAsync();


            // --------------------------------------------------------
            // CAMPUS ATTENDANCE
            // --------------------------------------------------------

            var campusAttendance =
                await _db.Attendance_Entry
                    .Where(a =>
                        periodProjectIds.Contains(
                            a.Attendance_Register.Project_Id))
                    .Select(a => new
                    {
                        Project_Id =
                            a.Attendance_Register.Project_Id,

                        a.User_Id,

                        a.Hours_Logged
                    })
                    .ToListAsync();


            // --------------------------------------------------------
            // CAMPUS REGISTRATIONS
            // --------------------------------------------------------

            var campusRegistrations =
                await _db.Project_Register
                    .Where(r =>
                        periodProjectIds.Contains(
                            r.Project_Id) &&
                        r.Is_Committed)
                    .Select(r => new
                    {
                        r.Project_Id,
                        r.User_Id
                    })
                    .ToListAsync();


            var registrationUsers =
                campusRegistrations
                    .Select(r => r.User_Id)
                    .Distinct()
                    .Count();


            var attendedUsers =
                campusAttendance
                    .Select(a => a.User_Id)
                    .Distinct()
                    .Count();


            var attendanceRate =
                registrationUsers > 0
                    ? (double)
                        attendedUsers /
                        registrationUsers *
                        100
                    : 0;


            // --------------------------------------------------------
            // SDG IMPACT
            // --------------------------------------------------------

            var sdgData =
                await _db.Project_Sdg
                    .Include(p => p.Sdg)
                    .Where(p =>
                        periodProjectIds.Contains(
                            p.Project_Id))
                    .ToListAsync();


            var sdgImpact =
                sdgData
                    .GroupBy(p => new
                    {
                        p.Sdg_Id,
                        p.Sdg.Name
                    })
                    .Select(g =>
                    {
                        var ids =
                            g.Select(x =>
                                x.Project_Id)
                                .Distinct()
                                .ToHashSet();

                        var rows =
                            campusAttendance
                                .Where(a =>
                                    ids.Contains(
                                        a.Project_Id))
                                .ToList();

                        return new SdgImpactDto
                        {
                            Sdg_Id =
                                g.Key.Sdg_Id,

                            Sdg_Name =
                                g.Key.Name,

                            Projects =
                                ids.Count,

                            Volunteers =
                                rows
                                    .Select(a =>
                                        a.User_Id)
                                    .Distinct()
                                    .Count(),

                            Hours =
                                Math.Round(
                                    rows.Sum(
                                        a =>
                                            a.Hours_Logged ??
                                            0),
                                    2)
                        };
                    })
                    .OrderByDescending(
                        x => x.Hours)
                    .ToList();


            var firstName =
                specialist.First_Name ??
                string.Empty;

            var lastName =
                specialist.Last_Name ??
                string.Empty;


            var campusName =
                string.IsNullOrWhiteSpace(
                    specialist.Campus.Name)
                    ? specialist.Campus.Code.ToString()
                    : specialist.Campus.Name;


            return new SpecialistIntelligenceDashboardDto
            {
                Specialist_Id =
                    specialist.User_Id,

                Specialist_Name =
                    (firstName + " " +
                     lastName).Trim(),

                Campus_Id =
                    campusId,

                Campus_Name =
                    campusName,

                Campus_Code =
                    specialist.Campus.Code.ToString(),

                Projects_Requiring_Attention =
                    campusHealth.Count(
                        h =>
                            h.Health_Status ==
                                "Attention" ||
                            h.Health_Status ==
                                "Critical"),

                Critical_Projects =
                    campusHealth.Count(
                        h =>
                            h.Health_Status ==
                            "Critical"),

                Projects_On_Track =
                    campusHealth.Count(
                        h =>
                            h.Health_Status ==
                            "On Track"),

                Overdue_Reports =
                    campusHealth.Count(
                        h =>
                            h.Summary_Overdue),

                Low_Participation_Projects =
                    campusHealth.Count(
                        h =>
                            h.Attendance_Applicable &&
                            h.Registered_Volunteers > 0 &&
                            h.Attendance_Rate < 50),

                Overall_Campus_Health =
                    campusHealth.Any()
                        ? Math.Round(
                            campusHealth.Average(
                                h =>
                                    h.Health_Score),
                            2)
                        : 0,

                Active_Projects =
                    campusHealth.Count(
                        h =>
                            h.Status ==
                            ProjectStatus.Active
                                .ToString()),

                Total_Volunteers =
                    volunteers.Count,

                Volunteer_Hours =
                    (int)Math.Round(
                        campusAttendance.Sum(
                            a =>
                                a.Hours_Logged ??
                                0)),

                Attendance_Rate =
                    Math.Round(
                        attendanceRate,
                        2),

                My_Project_Health =
                    myHealth,

                Campus_Project_Health =
                    campusHealth
                        .Take(20)
                        .ToList(),

                My_Problems =
                    myProblems
                        .Take(10)
                        .ToList(),

                My_Recommendations =
                    myRecommendations
                        .Take(10)
                        .ToList(),

                Campus_Problems =
                    campusProblems
                        .Take(15)
                        .ToList(),

                Campus_Recommendations =
                    campusRecommendations
                        .Take(15)
                        .ToList(),

                Sdg_Impact =
                    sdgImpact
                        .Take(8)
                        .ToList()
            };
        }


        // ============================================================
        // COMPLETE PROJECT INTELLIGENCE
        // ============================================================

        public async Task<ProjectIntelligenceDto>
            GetProjectIntelligence(int projectId)
        {
            var health =
                (await GetProjectHealth(projectId))
                    .FirstOrDefault();


            if (health == null)
            {
                throw new KeyNotFoundException(
                    "Project not found");
            }


            /*
             * IMPORTANT:
             *
             * Do NOT call GetProjectProblems()
             * because that intentionally returns only
             * one primary problem per project.
             *
             * The detail page needs ALL problems.
             */

            var problems =
                health.Problems
                    .Select(
                        (problem, index) =>
                            new ProjectProblemDto
                            {
                                Project_Id =
                                    health.Project_Id,

                                Project_Name =
                                    health.Project_Name,

                                Severity =
                                    index == 0
                                        ? health.Health_Status
                                        : "Attention",

                                Problem =
                                    problem,

                                Recommended_Action =
                                    index <
                                    health.Recommendations.Count
                                        ? health.Recommendations[index]
                                        : "Review the project and take the appropriate corrective action."
                            })
                    .ToList();


            var recommendations =
                health.Recommendations
                    .Select(
                        (recommendation, index) =>
                            new ProjectRecommendationDto
                            {
                                Project_Id =
                                    health.Project_Id,

                                Project_Name =
                                    health.Project_Name,

                                Priority =
                                    index == 0
                                        ? health.Health_Status
                                        : "Attention",

                                Recommendation =
                                    recommendation,

                                Reason =
                                    index <
                                    health.Problems.Count
                                        ? health.Problems[index]
                                        : "Project activity indicates that attention may be required."
                            })
                    .ToList();


            return new ProjectIntelligenceDto
            {
                Health =
                    health,

                Problems =
                    problems,

                Recommendations =
                    recommendations
            };
        }


        // ============================================================
        // ASSISTANT DASHBOARD
        // ============================================================

        public async Task<AssistantIntelligenceDashboardDto>
            GetAssistantDashboard(int assistantId)
        {
            var assistant =
                await _db.User
                    .Include(u => u.Campus)
                    .FirstOrDefaultAsync(
                        u =>
                            u.User_Id ==
                                assistantId &&
                            u.Role ==
                                UserRoleType.assistant);


            if (assistant == null)
            {
                throw new KeyNotFoundException(
                    "Assistant not found");
            }


            var registers =
                await _db.Attendance_Register
                    .Where(a =>
                        a.Assistant_Id ==
                        assistantId)
                    .Select(a => new
                    {
                        a.Attendance_Id,
                        a.Project_Id
                    })
                    .ToListAsync();


            var projectIds =
                registers
                    .Select(r => r.Project_Id)
                    .Distinct()
                    .ToList();


            var projectHealth =
                projectIds.Count == 0
                    ? new List<ProjectHealthDto>()
                    : await GetProjectHealthForIds(
                        projectIds);


            var attendanceEntries =
                projectIds.Count == 0
                    ? new List<AttendanceEntryForIntelligence>()
                    : await _db.Attendance_Entry
                        .Where(a =>
                            projectIds.Contains(
                                a.Attendance_Register.Project_Id))
                        .Select(a =>
                            new AttendanceEntryForIntelligence
                            {
                                ProjectId =
                                    a.Attendance_Register.Project_Id,

                                UserId =
                                    a.User_Id
                            })
                        .ToListAsync();


            var registeredVolunteers =
                projectIds.Count == 0
                    ? 0
                    : await _db.Project_Register
                        .Where(r =>
                            projectIds.Contains(
                                r.Project_Id) &&
                            r.Is_Committed)
                        .Select(r =>
                            new
                            {
                                r.Project_Id,
                                r.User_Id
                            })
                        .Distinct()
                        .CountAsync();


            var volunteersRecorded =
                attendanceEntries
                    .Select(a => a.UserId)
                    .Distinct()
                    .Count();


            var attendanceRate =
                registeredVolunteers > 0
                    ? (double)
                        volunteersRecorded /
                        registeredVolunteers *
                        100
                    : 0;


            var totalProjectsWithSummaries =
                projectIds.Count == 0
                    ? 0
                    : await _db.Project_Summary
                        .Where(s =>
                            projectIds.Contains(
                                s.Project_Id))
                        .Select(s =>
                            s.Project_Id)
                        .Distinct()
                        .CountAsync();


            var pendingSummaryCount =
                Math.Max(
                    0,
                    projectIds.Count -
                    totalProjectsWithSummaries);


            var reportsSubmitted =
                await _db.Report
                    .CountAsync(
                        r =>
                            r.User_Id ==
                            assistantId);


            var healthAverage =
                projectHealth.Count > 0
                    ? projectHealth.Average(
                        p =>
                            p.Health_Score)
                    : 100;


            var operationalScore =
                Math.Round(
                    (healthAverage * 0.6) +
                    (Math.Min(
                        attendanceRate,
                        100) * 0.3) +
                    (pendingSummaryCount == 0
                        ? 10
                        : 0),
                    1);


            // --------------------------------------------------------
            // ASSISTANT PROBLEMS
            // --------------------------------------------------------

            var problems =
                projectHealth
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.Primary_Problem))
                    .Select(p =>
                        new ProjectProblemDto
                        {
                            Project_Id =
                                p.Project_Id,

                            Project_Name =
                                p.Project_Name,

                            Severity =
                                p.Health_Status,

                            Problem =
                                p.Primary_Problem!,

                            Recommended_Action =
                                p.Primary_Recommendation
                                ??
                                "Review the project activity and update the project record."
                        })
                    .OrderBy(p =>
                        GetPriorityFromSeverity(
                            p.Severity))
                    .ThenBy(p =>
                        p.Project_Name)
                    .Take(20)
                    .ToList();


            // --------------------------------------------------------
            // ASSISTANT RECOMMENDATIONS
            // --------------------------------------------------------

            var recommendations =
                projectHealth
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.Primary_Recommendation))
                    .Select(p =>
                        new ProjectRecommendationDto
                        {
                            Project_Id =
                                p.Project_Id,

                            Project_Name =
                                p.Project_Name,

                            Priority =
                                p.Health_Status,

                            Recommendation =
                                p.Primary_Recommendation!,

                            Reason =
                                p.Primary_Problem
                                ??
                                "Project health requires attention."
                        })
                    .OrderBy(p =>
                        GetPriorityFromSeverity(
                            p.Priority))
                    .ThenBy(p =>
                        p.Project_Name)
                    .Take(20)
                    .ToList();


            return new AssistantIntelligenceDashboardDto
            {
                Assistant_Id =
                    assistantId,

                Assistant_Name =
                    $"{assistant.First_Name} " +
                    $"{assistant.Last_Name}".Trim(),

                Campus_Id =
                    assistant.Campus_Id,

                Campus_Name =
                    assistant.Campus?.Name ??
                    "Campus",

                Attendance_Sessions =
                    registers.Count,

                Volunteers_Recorded =
                    volunteersRecorded,

                Reports_Submitted =
                    reportsSubmitted,

                Pending_Summaries =
                    pendingSummaryCount,

                Attendance_Rate =
                    Math.Round(
                        attendanceRate,
                        1),

                Operational_Score =
                    Math.Min(
                        100,
                        operationalScore),

                Project_Health =
                    projectHealth,

                Problems =
                    problems,

                Recommendations =
                    recommendations
            };
        }


        // ============================================================
        // VOLUNTEER DASHBOARD
        // ============================================================

        public async Task<VolunteerIntelligenceDashboardDto>
            GetVolunteerDashboard(int volunteerId)
        {
            var volunteer =
                await _db.Volunteer
                    .Include(v => v.User)
                    .ThenInclude(u => u.Campus)
                    .FirstOrDefaultAsync(
                        v =>
                            v.Volunteer_Id ==
                            volunteerId);


            if (volunteer == null)
            {
                throw new KeyNotFoundException(
                    "Volunteer not found");
            }


            var registrations =
                await _db.Project_Register
                    .Where(r =>
                        r.User_Id ==
                            volunteerId &&
                        r.Is_Committed)
                    .Select(r =>
                        r.Project_Id)
                    .ToListAsync();


            var attendedProjectIds =
                await _db.Attendance_Entry
                    .Where(a =>
                        a.User_Id ==
                        volunteerId)
                    .Select(a =>
                        a.Attendance_Register.Project_Id)
                    .Distinct()
                    .ToListAsync();


            var projectsRegistered =
                registrations.Count;


            var projectsAttended =
                attendedProjectIds.Count;


            var attendanceRate =
                projectsRegistered > 0
                    ? (double)
                        projectsAttended /
                        projectsRegistered *
                        100
                    : 0;


            var certificates =
                await _db.Certificate
                    .CountAsync(
                        c =>
                            c.User_Id ==
                            volunteerId);


            var hours =
                volunteer.Hours;


            var hourScore =
                Math.Min(
                    40,
                    hours / 40 * 40);


            var projectScore =
                Math.Min(
                    25,
                    projectsRegistered /
                    5.0 *
                    25);


            var attendanceScore =
                Math.Min(
                    25,
                    attendanceRate /
                    100 *
                    25);


            var certificateScore =
                (double)Math.Min(
                    10,
                    certificates * 5);


            var engagementScore =
                Math.Round(
                    hourScore +
                    projectScore +
                    attendanceScore +
                    certificateScore,
                    1);


            var myHealth =
                registrations.Count == 0
                    ? new List<ProjectHealthDto>()
                    : await GetProjectHealthForIds(
                        registrations);


            var existingProjectIds =
                registrations.ToHashSet();


            var today =
                DateOnly.FromDateTime(
                    DateTime.Today);


            var opportunities =
                await _db.Project
                    .Include(p => p.Campus)
                    .Where(p =>
                        p.Is_Published &&
                        p.Status ==
                            ProjectStatus.Active &&
                        p.Date >= today &&
                        !existingProjectIds.Contains(
                            p.Project_id))
                    .OrderBy(p =>
                        p.Campus_Id ==
                        volunteer.User.Campus_Id
                            ? 0
                            : 1)
                    .ThenBy(p =>
                        p.Date)
                    .Take(5)
                    .Select(p =>
                        new VolunteerOpportunityDto
                        {
                            Project_Id =
                                p.Project_id,

                            Project_Name =
                                p.Title,

                            Date =
                                p.Date,

                            Hours =
                                p.Hours,

                            Available_Slots =
                                Math.Max(
                                    0,
                                    p.Num_Volunteers -
                                    p.ProjectRegisters
                                        .Count(
                                            r =>
                                                r.Is_Committed)),

                            Campus_Name =
                                p.Campus != null
                                    ? p.Campus.Name
                                    : "Campus",

                            Match_Reason =
                                p.Campus_Id ==
                                volunteer.User.Campus_Id
                                    ? "Available on your campus"
                                    : "Available opportunity"
                        })
                    .ToListAsync();


            return new VolunteerIntelligenceDashboardDto
            {
                Volunteer_Id =
                    volunteerId,

                Volunteer_Name =
                    $"{volunteer.User.First_Name} " +
                    $"{volunteer.User.Last_Name}".Trim(),

                Campus_Id =
                    volunteer.User.Campus_Id,

                Campus_Name =
                    volunteer.User.Campus?.Name ??
                    "Campus",

                Hours_Logged =
                    hours,

                Projects_Registered =
                    projectsRegistered,

                Projects_Attended =
                    projectsAttended,

                Certificates =
                    certificates,

                Attendance_Rate =
                    Math.Round(
                        attendanceRate,
                        1),

                Engagement_Score =
                    engagementScore,

                Completion_Eligible =
                    volunteer.Is_Completed ||
                    hours >= 80,

                Score_Breakdown =
                    new List<VolunteerScoreItemDto>
                    {
                        new()
                        {
                            Metric =
                                "Service hours",

                            Score =
                                Math.Round(
                                    hourScore,
                                    1),

                            Maximum = 40
                        },

                        new()
                        {
                            Metric =
                                "Project participation",

                            Score =
                                Math.Round(
                                    projectScore,
                                    1),

                            Maximum = 25
                        },

                        new()
                        {
                            Metric =
                                "Attendance consistency",

                            Score =
                                Math.Round(
                                    attendanceScore,
                                    1),

                            Maximum = 25
                        },

                        new()
                        {
                            Metric =
                                "Certificates",

                            Score =
                                Math.Round(
                                    certificateScore,
                                    1),

                            Maximum = 10
                        }
                    },

                Recommended_Opportunities =
                    opportunities,

                My_Project_Health =
                    myHealth
            };
        }


        // ============================================================
        // PROJECT HEALTH FOR MULTIPLE IDS
        // ============================================================

        private async Task<List<ProjectHealthDto>>
            GetProjectHealthForIds(
                List<int> projectIds)
        {
            if (projectIds == null ||
                projectIds.Count == 0)
            {
                return new List<ProjectHealthDto>();
            }


            /*
             * Do one call instead of calling
             * GetProjectHealth() once per project.
             */

            var query =
                _db.Project
                    .Where(p =>
                        projectIds.Contains(
                            p.Project_id))
                    .Select(p =>
                        p.Project_id);


            var ids =
                await query.ToListAsync();


            if (!ids.Any())
            {
                return new List<ProjectHealthDto>();
            }


            return await GetProjectHealthForIdList(ids);
        }


        private async Task<List<ProjectHealthDto>>
            GetProjectHealthForIdList(
                List<int> projectIds)
        {
            var query =
                _db.Project
                    .Where(p =>
                        projectIds.Contains(
                            p.Project_id))
                    .Include(p =>
                        p.ProjectRegisters)
                    .Include(p =>
                        p.AttendanceRegisters)
                    .AsQueryable();


            var projects =
                await query
                    .OrderByDescending(
                        p => p.Date)
                    .ToListAsync();


            if (!projects.Any())
            {
                return new List<ProjectHealthDto>();
            }


            var actualIds =
                projects
                    .Select(p => p.Project_id)
                    .ToList();


            var attendance =
                await _db.Attendance_Entry
                    .Where(a =>
                        actualIds.Contains(
                            a.Attendance_Register.Project_Id))
                    .Select(a => new
                    {
                        Project_Id =
                            a.Attendance_Register.Project_Id,

                        a.User_Id,

                        a.Hours_Logged
                    })
                    .ToListAsync();


            var summaries =
                await _db.Project_Summary
                    .Where(s =>
                        actualIds.Contains(
                            s.Project_Id))
                    .Select(s =>
                        s.Project_Id)
                    .Distinct()
                    .ToListAsync();


            var summaryIds =
                summaries.ToHashSet();


            var today =
                DateOnly.FromDateTime(
                    DateTime.Today);


            var result =
                new List<ProjectHealthDto>();


            foreach (var project in projects)
            {
                var registered =
                    project.ProjectRegisters
                        .Count(r =>
                            r.Is_Committed);


                var projectAttendance =
                    attendance
                        .Where(a =>
                            a.Project_Id ==
                            project.Project_id)
                        .ToList();


                var attended =
                    projectAttendance
                        .Select(a =>
                            a.User_Id)
                        .Distinct()
                        .Count();


                var hours =
                    projectAttendance
                        .Sum(a =>
                            a.Hours_Logged ?? 0);


                var hasSummary =
                    summaryIds.Contains(
                        project.Project_id);


                var isCompleted =
                    project.Status ==
                    ProjectStatus.Completed;


                var isCancelled =
                    project.Status ==
                    ProjectStatus.Cancelled;


                var isFuture =
                    project.Date > today;


                var isToday =
                    project.Date == today;


                var isPast =
                    project.Date < today;


                var hasStarted =
                    isToday ||
                    isPast;


                double attendanceRate =
                    0;


                if (hasStarted &&
                    !isCancelled &&
                    registered > 0)
                {
                    attendanceRate =
                        (double)attended /
                        registered *
                        100;
                }


                var issues =
                    new List<ProjectIssue>();


                var limitations =
                    new List<string>();


                // ----------------------------------------------------
                // COMPLETED
                // ----------------------------------------------------

                if (isCompleted)
                {
                    if (!hasSummary)
                    {
                        issues.Add(
                            new ProjectIssue
                            {
                                Priority = 2,

                                Severity =
                                    "Attention",

                                Problem =
                                    "Project summary is missing.",

                                Recommendation =
                                    "Submit the project summary for this completed project."
                            });
                    }


                    if (project.Num_Volunteers > 0)
                    {
                        var fillRate =
                            (double)registered /
                            project.Num_Volunteers;


                        if (fillRate < 0.8)
                        {
                            limitations.Add(
                                $"Volunteer participation reached only {Math.Round(fillRate * 100, 1)}% of the target."
                            );
                        }
                    }


                    if (registered > 0 &&
                        attendanceRate < 80)
                    {
                        limitations.Add(
                            $"Attendance was {Math.Round(attendanceRate, 1)}% of committed volunteers."
                        );
                    }
                }


                // ----------------------------------------------------
                // CANCELLED
                // ----------------------------------------------------

                else if (isCancelled)
                {
                    attendanceRate = 0;
                }


                // ----------------------------------------------------
                // FUTURE
                // ----------------------------------------------------

                else if (isFuture)
                {
                    var daysUntil =
                        project.Date.DayNumber -
                        today.DayNumber;


                    if (project.Num_Volunteers > 0 &&
                        registered <
                            project.Num_Volunteers &&
                        daysUntil <= 3)
                    {
                        var fillRate =
                            (double)registered /
                            project.Num_Volunteers;


                        if (fillRate < 0.5)
                        {
                            issues.Add(
                                new ProjectIssue
                                {
                                    Priority = 2,

                                    Severity =
                                        "High",

                                    Problem =
                                        "Volunteer registration is below 50% of the target and the project is approaching.",

                                    Recommendation =
                                        "Increase volunteer recruitment before the project date."
                                });
                        }
                        else if (fillRate < 0.8)
                        {
                            issues.Add(
                                new ProjectIssue
                                {
                                    Priority = 3,

                                    Severity =
                                        "Attention",

                                    Problem =
                                        "Volunteer registration is below 80% of the target.",

                                    Recommendation =
                                        "Encourage more volunteers to register before the project date."
                                });
                        }
                    }
                }


                // ----------------------------------------------------
                // ACTIVE / PAST
                // ----------------------------------------------------

                else
                {
                    if (isPast)
                    {
                        issues.Add(
                            new ProjectIssue
                            {
                                Priority = 1,

                                Severity =
                                    "Critical",

                                Problem =
                                    "Project date has passed without completion.",

                                Recommendation =
                                    "Review the project status and update the project record."
                            });
                    }


                    if (project.Num_Volunteers > 0 &&
                        registered <
                            project.Num_Volunteers)
                    {
                        var fillRate =
                            (double)registered /
                            project.Num_Volunteers;


                        if (fillRate < 0.5)
                        {
                            issues.Add(
                                new ProjectIssue
                                {
                                    Priority = 2,

                                    Severity =
                                        "High",

                                    Problem =
                                        "Volunteer registration is below 50% of the target.",

                                    Recommendation =
                                        "Increase volunteer recruitment or review the project capacity."
                                });
                        }
                        else if (fillRate < 0.8)
                        {
                            issues.Add(
                                new ProjectIssue
                                {
                                    Priority = 3,

                                    Severity =
                                        "Attention",

                                    Problem =
                                        "Volunteer registration is below 80% of the target.",

                                    Recommendation =
                                        "Encourage more volunteers to participate."
                                });
                        }
                    }


                    if (hasStarted &&
                        registered > 0)
                    {
                        if (attendanceRate < 50)
                        {
                            issues.Add(
                                new ProjectIssue
                                {
                                    Priority = 2,

                                    Severity =
                                        "High",

                                    Problem =
                                        "Attendance is below 50% of committed volunteers.",

                                    Recommendation =
                                        "Investigate the attendance drop and follow up with registered volunteers."
                                });
                        }
                        else if (attendanceRate < 80)
                        {
                            issues.Add(
                                new ProjectIssue
                                {
                                    Priority = 3,

                                    Severity =
                                        "Attention",

                                    Problem =
                                        "Attendance is below 80% of committed volunteers.",

                                    Recommendation =
                                        "Follow up with volunteers who registered but did not attend."
                                });
                        }
                    }


                    if (isPast &&
                        !hasSummary)
                    {
                        issues.Add(
                            new ProjectIssue
                            {
                                Priority = 2,

                                Severity =
                                    "High",

                                Problem =
                                    "Project summary is missing.",

                                Recommendation =
                                    "Submit the project summary for the completed project."
                            });
                    }
                }


                var orderedIssues =
                    issues
                        .OrderBy(i =>
                            i.Priority)
                        .ThenBy(i =>
                            i.Problem)
                        .ToList();


                var score =
                    100;


                foreach (var issue in orderedIssues)
                {
                    score -= issue.Priority switch
                    {
                        1 => 35,
                        2 => 20,
                        3 => 10,
                        4 => 5,
                        _ => 0
                    };
                }


                score =
                    Math.Max(
                        0,
                        Math.Min(
                            100,
                            score));


                string healthStatus;


                if (isCancelled)
                {
                    healthStatus =
                        "Cancelled";
                }
                else if (score < 60)
                {
                    healthStatus =
                        "Critical";
                }
                else if (score < 80)
                {
                    healthStatus =
                        "Attention";
                }
                else
                {
                    healthStatus =
                        "On Track";
                }


                var primaryIssue =
                    orderedIssues.FirstOrDefault();


                var summaryOverdue =
                    !isCancelled &&
                    !hasSummary &&
                    (isPast || isCompleted);


                result.Add(
                    new ProjectHealthDto
                    {
                        Project_Id =
                            project.Project_id,

                        Project_Name =
                            project.Title,

                        Status =
                            project.Status.ToString(),

                        Health_Score =
                            score,

                        Health_Status =
                            healthStatus,

                        Registered_Volunteers =
                            registered,

                        Expected_Volunteers =
                            project.Num_Volunteers,

                        Attended_Volunteers =
                            attended,

                        Attendance_Rate =
                            Math.Round(
                                attendanceRate,
                                2),

                        Hours_Completed =
                            Math.Round(
                                hours,
                                2),

                        Hours_Target =
                            project.Hours,

                        Summary_Generated =
                            hasSummary,

                        Report_Submitted =
                            hasSummary,

                        Problems =
                            orderedIssues
                                .Select(i =>
                                    i.Problem)
                                .Distinct()
                                .ToList(),

                        Recommendations =
                            orderedIssues
                                .Select(i =>
                                    i.Recommendation)
                                .Distinct()
                                .ToList(),

                        Primary_Problem =
                            primaryIssue?.Problem,

                        Primary_Recommendation =
                            primaryIssue?.Recommendation,

                        Primary_Priority =
                            primaryIssue?.Priority,

                        Limitations =
                            limitations
                                .Distinct()
                                .ToList(),

                        Attendance_Applicable =
                            hasStarted &&
                            !isCancelled,

                        Summary_Overdue =
                            summaryOverdue
                    });
            }


            return result;
        }


        // ============================================================
        // AUTOMATIC PROJECT CHECKS
        // ============================================================

        public async Task ProcessAutomaticProjectChecks(
            CancellationToken cancellationToken)
        {
            var health =
                await GetProjectHealth();


            // ========================================================
            // PROJECT RISK NOTIFICATIONS
            // ========================================================

            foreach (
                var project in health
                    .Where(p =>
                        p.Primary_Priority.HasValue &&
                        p.Primary_Priority.Value <= 2))
            {
                cancellationToken
                    .ThrowIfCancellationRequested();


                var dbProject =
                    await _db.Project
                        .FirstOrDefaultAsync(
                            p =>
                                p.Project_id ==
                                project.Project_Id,
                            cancellationToken);


                if (dbProject == null)
                    continue;


                var title =
                    project.Health_Status ==
                        "Critical"
                        ? "Project Requires Immediate Attention"
                        : "Project Requires Attention";


                var message =
                    project.Primary_Problem
                    ??
                    "The project requires attention.";


                var notificationMessage =
                    project.Project_Name +
                    ": " +
                    message;


                var recentNotification =
                    await _db.Notification
                        .AnyAsync(
                            n =>
                                n.User_Id ==
                                    dbProject.User_Id &&
                                n.Title ==
                                    title &&
                                n.Message ==
                                    notificationMessage &&
                                n.Created_At >=
                                    DateTime.Now
                                        .AddHours(-24),
                            cancellationToken);


                if (!recentNotification)
                {
                    await _notificationService.NotifyUser(
                        dbProject.User_Id,
                        title,
                        notificationMessage,
                        NotificationType.ProjectRisk);
                }


                // ----------------------------------------------------
                // SUMMARY ESCALATION
                // ----------------------------------------------------

                if (project.Summary_Overdue)
                {
                    var daysOverdue =
                        DateOnly
                            .FromDateTime(
                                DateTime.Today)
                            .DayNumber -
                        dbProject.Date.DayNumber;


                    if (daysOverdue >= 7)
                    {
                        var escalationMessage =
                            dbProject.Title +
                            " has an outstanding project summary more than 7 days after the project date.";


                        var escalationSent =
                            await _db.Notification
                                .AnyAsync(
                                    n =>
                                        n.Title ==
                                            "Project Summary Escalation" &&
                                        n.Message ==
                                            escalationMessage &&
                                        n.Created_At >=
                                            DateTime.Now
                                                .AddHours(-24),
                                    cancellationToken);


                        if (!escalationSent)
                        {
                            await _notificationService.NotifyCampus(
                                dbProject.Campus_Id,
                                "Project Summary Escalation",
                                escalationMessage,
                                NotificationType.ProjectEscalation,
                                dbProject.User_Id);
                        }
                    }
                }
            }


            // ========================================================
            // SDG VOLUNTEER RECRUITMENT
            // ONE DAY BEFORE PROJECT
            // ========================================================

            await SendSdgVolunteerRecruitmentNotifications(
                cancellationToken);
        }


        // ============================================================
        // SDG VOLUNTEER RECRUITMENT
        // ============================================================

        private async Task
            SendSdgVolunteerRecruitmentNotifications(
                CancellationToken cancellationToken)
        {
            var tomorrow =
                DateOnly.FromDateTime(
                    DateTime.Today.AddDays(1));


            var projects =
                await _db.Project
                    .Include(p =>
                        p.ProjectRegisters)
                    .Where(p =>
                        p.Date ==
                            tomorrow &&
                        p.Status ==
                            ProjectStatus.Active &&
                        p.Num_Volunteers > 0)
                    .ToListAsync(
                        cancellationToken);


            foreach (var project in projects)
            {
                cancellationToken
                    .ThrowIfCancellationRequested();


                var registered =
                    project.ProjectRegisters
                        .Count(r =>
                            r.Is_Committed);


                var remaining =
                    project.Num_Volunteers -
                    registered;


                if (remaining <= 0)
                    continue;


                // ----------------------------------------------------
                // PROJECT SDGs
                // ----------------------------------------------------

                var sdgIds =
                    await _db.Project_Sdg
                        .Where(ps =>
                            ps.Project_Id ==
                            project.Project_id)
                        .Select(ps =>
                            ps.Sdg_Id)
                        .Distinct()
                        .ToListAsync(
                            cancellationToken);


                if (!sdgIds.Any())
                    continue;


                // ----------------------------------------------------
                // ALREADY REGISTERED USERS
                // ----------------------------------------------------

                var registeredUserIds =
                    project.ProjectRegisters
                        .Where(r =>
                            r.Is_Committed)
                        .Select(r =>
                            r.User_Id)
                        .ToHashSet();


                // ----------------------------------------------------
                // VOLUNTEERS INTERESTED IN PROJECT SDGs
                // ----------------------------------------------------

                var matchingVolunteerIds =
                    await _db.Volunteer_Sdg
                        .Where(vs =>
                            sdgIds.Contains(
                                vs.Sdg_Id))
                        .Select(vs =>
                            vs.Volunteer_Id)
                        .Distinct()
                        .ToListAsync(
                            cancellationToken);


                if (!matchingVolunteerIds.Any())
                    continue;


                // ----------------------------------------------------
                // ACTIVE VOLUNTEERS
                // ----------------------------------------------------

                var targetVolunteerIds =
                    await _db.User
                        .Where(u =>
                            matchingVolunteerIds.Contains(
                                u.User_Id) &&
                            u.Role ==
                                UserRoleType.volunteer &&
                            u.Is_Active &&
                            !u.Is_Suspended &&
                            !registeredUserIds.Contains(
                                u.User_Id))
                        .Select(u =>
                            u.User_Id)
                        .ToListAsync(
                            cancellationToken);


                if (!targetVolunteerIds.Any())
                    continue;


                // ----------------------------------------------------
                // NOTIFICATION
                // ----------------------------------------------------

                var title =
                    "Volunteers Needed for Tomorrow";


                var message =
                    $"{project.Title} still needs " +
                    $"{remaining} volunteer(s). " +
                    "Your SDG interests match this project.";


                foreach (var volunteerId
                         in targetVolunteerIds)
                {
                    cancellationToken
                        .ThrowIfCancellationRequested();


                    var alreadySent =
                        await _db.Notification
                            .AnyAsync(
                                n =>
                                    n.User_Id ==
                                        volunteerId &&
                                    n.Title ==
                                        title &&
                                    n.Created_At >=
                                        DateTime.Now
                                            .AddHours(-24),
                                cancellationToken);


                    if (alreadySent)
                        continue;


                    await _notificationService.NotifyUser(
                        volunteerId,
                        title,
                        message,
                        NotificationType.ProjectNotification);
                }
            }
        }


        // ============================================================
        // PRIORITY HELPER
        // ============================================================

        private static int
            GetPriorityFromSeverity(
                string? severity)
        {
            return severity switch
            {
                "Critical" => 1,

                "High" => 2,

                "Attention" => 3,

                "On Track" => 4,

                _ => 99
            };
        }


        // ============================================================
        // INTERNAL ISSUE MODEL
        // ============================================================

        private class ProjectIssue
        {
            public int Priority { get; set; }

            public string Severity { get; set; }
                = string.Empty;

            public string Problem { get; set; }
                = string.Empty;

            public string Recommendation { get; set; }
                = string.Empty;
        }


        // ============================================================
        // ATTENDANCE HELPER
        // ============================================================

        private class AttendanceEntryForIntelligence
        {
            public int ProjectId { get; set; }

            public int UserId { get; set; }
        }
    }
}