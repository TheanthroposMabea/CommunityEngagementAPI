namespace CommunityEngagementAPI.Models.DTOs
{
    public class NgoReportDto
    {
        // -----------------------------------------
        // APPLICATION SUMMARY
        // -----------------------------------------

        public int TotalApplications { get; set; }

        public int ApplicationsThisMonth { get; set; }

        public int ApplicationsThisYear { get; set; }

        public int PendingApplications { get; set; }

        public int MissingInformationApplications { get; set; }

        public int ApprovedApplications { get; set; }

        public int DeclinedApplications { get; set; }

        public int ReviewedApplications { get; set; }


        // -----------------------------------------
        // PARTNERSHIPS
        // -----------------------------------------

        public int TotalPartnerships { get; set; }

        public int ActivePartnerships { get; set; }

        public int InactivePartnerships { get; set; }

        public int PartnershipsStartedThisYear { get; set; }

        public int PartnershipsDueForRenewal { get; set; }


        // -----------------------------------------
        // RATES
        // -----------------------------------------

        public double ApprovalRate { get; set; }

        public double PartnershipRate { get; set; }


        // -----------------------------------------
        // SDG
        // -----------------------------------------

        public int NgosWithSdgs { get; set; }

        public int NgosWithoutSdgs { get; set; }

        public int TotalNgoSdgParticipations { get; set; }

        public List<NgoSdgStatisticDto> SdgStatistics { get; set; }
            = new();


        // -----------------------------------------
        // CAMPUS
        // -----------------------------------------

        public List<NgoCampusStatisticDto> CampusStatistics { get; set; }
            = new();


        // -----------------------------------------
        // APPLICATION STATUS
        // -----------------------------------------

        public List<NgoStatusStatisticDto> StatusStatistics { get; set; }
            = new();


        // -----------------------------------------
        // RENEWALS
        // -----------------------------------------

        public List<NgoRenewalDto> UpcomingRenewals { get; set; }
            = new();
    }
}
