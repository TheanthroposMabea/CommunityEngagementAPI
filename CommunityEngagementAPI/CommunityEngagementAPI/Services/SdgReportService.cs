using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CommunityEngagementAPI.Services
{

    public interface ISdgReportService
    {
        Task<SdgReportDto> GetSdgReport(int? campusId);
    }

    public class SdgReportService: ISdgReportService
    {
        private readonly CEAppContext _db;

        public SdgReportService(CEAppContext db)
        {
            _db = db;
        }



        public async Task<SdgReportDto> GetSdgReport(int? campusId)
        {
            // --------------------------------------------------
            // 1. GET SDGs
            // --------------------------------------------------

            var sdgs = await _db.Sdg
                .AsNoTracking()
                .ToListAsync();


            // --------------------------------------------------
            // 2. GET VOLUNTEERS
            //
            // Volunteer_Id is also the User_Id according to
            // your Volunteer model.
            // --------------------------------------------------

            var volunteersQuery = _db.Volunteer
                .AsNoTracking()
                .AsQueryable();


            // --------------------------------------------------
            // 3. FILTER VOLUNTEERS BY CAMPUS
            //
            // Volunteer -> User -> Campus
            // --------------------------------------------------

            if (campusId.HasValue)
            {
                volunteersQuery = volunteersQuery
                    .Where(v =>
                        v.User.Campus_Id == campusId.Value);
            }


            var volunteers = await volunteersQuery
                .Select(v => new
                {
                    VolunteerId = v.Volunteer_Id,
                    UserId = v.User.User_Id
                })
                .ToListAsync();


            var volunteerIds = volunteers
                .Select(v => v.VolunteerId)
                .ToHashSet();


            // --------------------------------------------------
            // 4. VOLUNTEER -> SDG PARTICIPATION
            // --------------------------------------------------

            var volunteerSdgs = await _db.Volunteer_Sdg
                .AsNoTracking()
                .Where(vs =>
                    volunteerIds.Contains(vs.Volunteer_Id))
                .ToListAsync();


            // --------------------------------------------------
            // 5. TOTAL UNIQUE VOLUNTEERS PARTICIPATING
            // --------------------------------------------------

            var totalVolunteersParticipating =
                volunteerSdgs
                    .Select(vs => vs.Volunteer_Id)
                    .Distinct()
                    .Count();


            // --------------------------------------------------
            // 6. TOTAL SDG PARTICIPATIONS
            //
            // Example:
            //
            // Volunteer A -> SDG 4
            // Volunteer A -> SDG 3
            //
            // = 2 participations
            // --------------------------------------------------

            var totalSdgParticipations =
                volunteerSdgs.Count;


            // --------------------------------------------------
            // 7. GET PROJECT-SDG RELATIONSHIPS
            // --------------------------------------------------

            var projectSdgs = await _db.Project_Sdg
                .AsNoTracking()
                .ToListAsync();


            // --------------------------------------------------
            // 8. GET PROJECT IDs THAT ARE ASSOCIATED WITH
            // THE SDGS
            // --------------------------------------------------

            var projectIds = projectSdgs
                .Select(ps => ps.Project_Id)
                .Distinct()
                .ToList();


            // --------------------------------------------------
            // 9. GET ATTENDANCE REGISTERS FOR THOSE PROJECTS
            // --------------------------------------------------

            var attendanceRegisters = await _db.Attendance_Register
                .AsNoTracking()
                .Where(ar =>
                    projectIds.Contains(ar.Project_Id))
                .Select(ar => new
                {
                    ar.Attendance_Id,
                    ar.Project_Id
                })
                .ToListAsync();


            var attendanceIds = attendanceRegisters
                .Select(ar => ar.Attendance_Id)
                .ToList();


            // --------------------------------------------------
            // 10. GET ATTENDANCE ENTRIES
            // --------------------------------------------------

            var attendanceEntries = await _db.Attendance_Entry
                .AsNoTracking()
                .Where(a =>
                    attendanceIds.Contains(a.Attendance_Id))
                .Select(a => new
                {
                    a.Attendance_Id,
                    a.Hours_Logged
                })
                .ToListAsync();


            // --------------------------------------------------
            // 11. ONLY USE ATTENDANCE FROM THE SELECTED CAMPUS
            //
            // We need to identify which attendance belongs to
            // volunteers from this campus.
            //
            // Attendance Entry -> User
            // --------------------------------------------------

            var campusAttendanceEntries = await _db.Attendance_Entry
                .AsNoTracking()
                .Where(a =>
                    volunteerIds.Contains(a.User_Id))
                .Select(a => new
                {
                    a.Attendance_Id,
                    a.User_Id,
                    a.Hours_Logged
                })
                .ToListAsync();


            // --------------------------------------------------
            // 12. MAP ATTENDANCE REGISTER -> PROJECT
            // --------------------------------------------------

            var attendanceProjectLookup =
                attendanceRegisters
                    .ToDictionary(
                        x => x.Attendance_Id,
                        x => x.Project_Id);


            // --------------------------------------------------
            // 13. CALCULATE HOURS PER SDG
            // --------------------------------------------------

            var hoursBySdg = new Dictionary<int, double>();

            foreach (var entry in campusAttendanceEntries)
            {
                if (!entry.Hours_Logged.HasValue)
                    continue;

                if (!attendanceProjectLookup.TryGetValue(
                        entry.Attendance_Id,
                        out var projectId))
                {
                    continue;
                }

                // Find every SDG associated with this project
                var sdgsForProject = projectSdgs
                    .Where(ps =>
                        ps.Project_Id == projectId)
                    .Select(ps => ps.Sdg_Id)
                    .Distinct();

                foreach (var sdgId in sdgsForProject)
                {
                    if (!hoursBySdg.ContainsKey(sdgId))
                    {
                        hoursBySdg[sdgId] = 0;
                    }

                    hoursBySdg[sdgId] +=
                        entry.Hours_Logged.Value;
                }
            }


            // --------------------------------------------------
            // 14. TOTAL SDG HOURS
            // --------------------------------------------------

            var totalSdgHours =
                hoursBySdg.Values.Sum();


            // --------------------------------------------------
            // 15. BUILD SDG STATISTICS
            // --------------------------------------------------

            var sdgStatistics = new List<SdgStatisticDto>();

            foreach (var sdg in sdgs)
            {
                // Volunteers participating in this SDG
                var volunteerCount =
                    volunteerSdgs
                        .Where(vs =>
                            vs.Sdg_Id == sdg.Sdg_Id)
                        .Select(vs =>
                            vs.Volunteer_Id)
                        .Distinct()
                        .Count();


                var volunteerPercentage =
                    totalVolunteersParticipating == 0
                        ? 0
                        : (double)volunteerCount /
                          totalVolunteersParticipating *
                          100;


                var hours =
                    hoursBySdg.ContainsKey(sdg.Sdg_Id)
                        ? hoursBySdg[sdg.Sdg_Id]
                        : 0;


                var hoursPercentage =
                    totalSdgHours == 0
                        ? 0
                        : hours /
                          totalSdgHours *
                          100;


                sdgStatistics.Add(
                    new SdgStatisticDto
                    {
                        Sdg_Id = sdg.Sdg_Id,

                        Sdg_Name = sdg.Name,

                        VolunteerCount = volunteerCount,

                        VolunteerPercentage =
                            Math.Round(
                                volunteerPercentage,
                                2),

                        Hours =
                            Math.Round(
                                hours,
                                2),

                        HoursPercentage =
                            Math.Round(
                                hoursPercentage,
                                2)
                    });
            }


            // --------------------------------------------------
            // 16. RETURN REPORT
            // --------------------------------------------------

            return new SdgReportDto
            {
                TotalSdgs = sdgs.Count,

                TotalVolunteersParticipating =
                    totalVolunteersParticipating,

                TotalSdgParticipations =
                    totalSdgParticipations,

                TotalSdgHours =
                    Math.Round(
                        totalSdgHours,
                        2),

                SdgStatistics =
                    sdgStatistics
                        .OrderByDescending(
                            x => x.VolunteerCount)
                        .ToList()
            };
        }
    }
}
