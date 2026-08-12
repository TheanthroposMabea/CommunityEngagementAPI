namespace CommunityEngagementAPI.Models.DTOs
{
    public class NgoRenewalDto
    {
        public int PartnershipId { get; set; }

        public int ApplicationId { get; set; }

        public string NgoName { get; set; }

        public DateTime RenewalDate { get; set; }

        public int DaysToRenewal { get; set; }
    }
}
