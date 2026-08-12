using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CommunityEngagementAPI.Services
{
    public interface INgoReportService
    {
        Task<NgoReportDto> GetNgoReport(int? campusId);
    }

    public class NgoReportService: INgoReportService
    {

        private readonly CEAppContext _db;

        public NgoReportService(CEAppContext db)
        {
            _db = db;
        }

        public async Task<NgoReportDto> GetNgoReport(int? campusId)
        {
            var today = DateTime.Today;

            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var startOfYear = new DateTime(today.Year,1, 1);


            // Filter the campus
          
            string? campusName = null;

            if (campusId.HasValue)
            {
                campusName = await _db.Campus.Where(c => c.Campus_Id == campusId.Value).Select(c => c.Name).FirstOrDefaultAsync();

                if (campusName == null)
                {
                    throw new KeyNotFoundException($"Campus with ID {campusId.Value} was not found.");
                }
            }


            // NGO Applications

            var applicationsQuery = _db.NGO_Application.AsNoTracking().AsQueryable();

            // Campus flter

            if (campusName != null)
            {
                applicationsQuery = applicationsQuery.Where(n => n.Close_Campus == campusName);
            }

            var applications = await applicationsQuery.ToListAsync();


            // Application IDS
          
            var applicationIds = applications
                .Select(n => n.Application_Id)
                .ToHashSet();


 
            // BASIC APPLICATION COUNTS
            // ==================================================

            var totalApplications =
                applications.Count;

            var applicationsThisMonth =
                applications.Count(n =>
                    n.Date_Submitted >= startOfMonth);

            var applicationsThisYear =
                applications.Count(n =>
                    n.Date_Submitted >= startOfYear);


            // ==================================================
            // STATUS COUNTS
            // ==================================================

            var pendingApplications =
                applications.Count(n =>
                    n.Status == Status.PendingReview);

            var missingInformationApplications =
                applications.Count(n =>
                    n.Status == Status.MissingInformation);

            var approvedApplications =
                applications.Count(n =>
                    n.Status == Status.Approved);

            var declinedApplications =
                applications.Count(n =>
                    n.Status == Status.Declined);

            var reviewedApplications =
                applications.Count(n =>
                    n.Status == Status.Reviewed);


            // ==================================================
            // PARTNERSHIPS
            // ==================================================

            var partnerships = await _db.NGO_Partnership
                .AsNoTracking()
                .Where(p =>
                    applicationIds.Contains(
                        p.Application_Id))
                .ToListAsync();


            var totalPartnerships =
                partnerships.Count;

            var activePartnerships =
                partnerships.Count(p =>
                    p.Is_Active);

            var inactivePartnerships =
                partnerships.Count(p =>
                    !p.Is_Active);

            var partnershipsStartedThisYear =
                partnerships.Count(p =>
                    p.Start_Date >= startOfYear);


            // ==================================================
            // RENEWALS
            // ==================================================

            var partnershipsDueForRenewal =
                partnerships.Count(p =>
                    p.Renewal_Date.Date >= today &&
                    p.Renewal_Date.Date <= today.AddDays(30));


            // ==================================================
            // APPROVAL RATE
            // ==================================================

            var approvalRate =
                totalApplications == 0
                    ? 0
                    : (double)approvedApplications /
                      totalApplications *
                      100;


            // ==================================================
            // PARTNERSHIP RATE
            //
            // Number of applications that have a partnership
            // ==================================================

            var partnershipApplicationIds =
                partnerships
                    .Select(p => p.Application_Id)
                    .Distinct()
                    .ToHashSet();


            var partnershipRate =
                totalApplications == 0
                    ? 0
                    : (double)partnershipApplicationIds.Count /
                      totalApplications *
                      100;


            // ==================================================
            // NGO -> SDG
            // ==================================================

            var ngoSdgs = await _db.NGO_Sdg
                .AsNoTracking()
                .Where(ns =>
                    applicationIds.Contains(ns.Ngo_Id))
                .ToListAsync();


            // ==================================================
            // SDG INFORMATION
            // ==================================================

            var sdgs = await _db.Sdg
                .AsNoTracking()
                .ToListAsync();


            // ==================================================
            // NGOs WITH / WITHOUT SDGS
            // ==================================================

            var ngosWithSdgs =
                ngoSdgs
                    .Select(ns => ns.Ngo_Id)
                    .Distinct()
                    .Count();

            var ngosWithoutSdgs =
                totalApplications -
                ngosWithSdgs;


            // ==================================================
            // TOTAL NGO-SDG PARTICIPATIONS
            // ==================================================

            var totalNgoSdgParticipations =
                ngoSdgs.Count;


            // ==================================================
            // SDG STATISTICS
            // ==================================================

            var sdgStatistics = new List<NgoSdgStatisticDto>();

            foreach (var sdg in sdgs)
            {
                var ngoCount =
                    ngoSdgs
                        .Where(ns =>
                            ns.Sdg_Id == sdg.Sdg_Id)
                        .Select(ns =>
                            ns.Ngo_Id)
                        .Distinct()
                        .Count();


                var percentage =
                    ngosWithSdgs == 0
                        ? 0
                        : (double)ngoCount /
                          ngosWithSdgs *
                          100;


                sdgStatistics.Add(
                    new NgoSdgStatisticDto
                    {
                        Sdg_Id = sdg.Sdg_Id,

                        Sdg_Name = sdg.Name,

                        NgoCount = ngoCount,

                        Percentage =
                            Math.Round(
                                percentage,
                                2)
                    });
            }


            // ==================================================
            // STATUS STATISTICS
            // ==================================================

            var statusStatistics =
                Enum.GetValues<Status>()
                    .Select(status =>
                    {
                        var count =
                            applications.Count(n =>
                                n.Status == status);

                        var percentage =
                            totalApplications == 0
                                ? 0
                                : (double)count /
                                  totalApplications *
                                  100;

                        return new NgoStatusStatisticDto
                        {
                            Status = status.ToString(),

                            Count = count,

                            Percentage =
                                Math.Round(
                                    percentage,
                                    2)
                        };
                    })
                    .ToList();


            // ==================================================
            // CAMPUS STATISTICS
            //
            // For ALL CAMPUSES only.
            // ==================================================

            var campusStatistics =
                await BuildCampusStatistics(
                    applications,
                    partnerships);


            // ==================================================
            // UPCOMING RENEWALS
            // ==================================================

            var upcomingRenewals =
                (from partnership in partnerships
                 join application in applications
                     on partnership.Application_Id
                     equals application.Application_Id
                 where partnership.Renewal_Date.Date >= today
                       && partnership.Renewal_Date.Date
                           <= today.AddDays(30)
                 orderby partnership.Renewal_Date
                 select new NgoRenewalDto
                 {
                     PartnershipId =
                         partnership.Partnership_Id,

                     ApplicationId =
                         application.Application_Id,

                     NgoName =
                         application.Name,

                     RenewalDate =
                         partnership.Renewal_Date,

                     DaysToRenewal =
                         (partnership.Renewal_Date.Date -
                          today).Days
                 })
                .ToList();


            // ==================================================
            // RETURN
            // ==================================================

            return new NgoReportDto
            {
                TotalApplications =
                    totalApplications,

                ApplicationsThisMonth =
                    applicationsThisMonth,

                ApplicationsThisYear =
                    applicationsThisYear,

                PendingApplications =
                    pendingApplications,

                MissingInformationApplications =
                    missingInformationApplications,

                ApprovedApplications =
                    approvedApplications,

                DeclinedApplications =
                    declinedApplications,

                ReviewedApplications =
                    reviewedApplications,

                TotalPartnerships =
                    totalPartnerships,

                ActivePartnerships =
                    activePartnerships,

                InactivePartnerships =
                    inactivePartnerships,

                PartnershipsStartedThisYear =
                    partnershipsStartedThisYear,

                PartnershipsDueForRenewal =
                    partnershipsDueForRenewal,

                ApprovalRate =
                    Math.Round(
                        approvalRate,
                        2),

                PartnershipRate =
                    Math.Round(
                        partnershipRate,
                        2),

                NgosWithSdgs =
                    ngosWithSdgs,

                NgosWithoutSdgs =
                    ngosWithoutSdgs,

                TotalNgoSdgParticipations =
                    totalNgoSdgParticipations,

                SdgStatistics =
                    sdgStatistics
                        .OrderByDescending(
                            x => x.NgoCount)
                        .ToList(),

                CampusStatistics =
                    campusStatistics,

                StatusStatistics =
                    statusStatistics,

                UpcomingRenewals =
                    upcomingRenewals
            };
        }


        // ======================================================
        // CAMPUS COMPARISON
        // ======================================================

        private async Task<List<NgoCampusStatisticDto>>
            BuildCampusStatistics(
                List<NGO_Application> applications,
                List<NGO_Partnership> partnerships)
        {
            var result =
                new List<NgoCampusStatisticDto>();


            var campuses = await _db.Campus
                .AsNoTracking()
                .ToListAsync();


            foreach (var campus in campuses)
            {
                var campusApplications =
                    applications
                        .Where(n =>
                            n.Close_Campus ==
                            campus.Name)
                        .ToList();


                var campusApplicationIds =
                    campusApplications
                        .Select(n =>
                            n.Application_Id)
                        .ToHashSet();


                var campusPartnerships =
                    partnerships
                        .Where(p =>
                            campusApplicationIds.Contains(
                                p.Application_Id))
                        .ToList();


                result.Add(
                    new NgoCampusStatisticDto
                    {
                        Campus =
                            campus.Name,

                        ApplicationCount =
                            campusApplications.Count,

                        ApprovedCount =
                            campusApplications.Count(n =>
                                n.Status ==
                                Status.Approved),

                        PartnershipCount =
                            campusPartnerships.Count,

                        ActivePartnershipCount =
                            campusPartnerships.Count(p =>
                                p.Is_Active)
                    });
            }


            return result
                .OrderByDescending(
                    x => x.ApplicationCount)
                .ToList();
        }


    }
}
