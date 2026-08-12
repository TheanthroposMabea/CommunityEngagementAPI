namespace CommunityEngagementAPI.Models.DTOs
{
    public class SdgReportDto
    {
        public int TotalSdgs { get; set; }

        public int TotalVolunteersParticipating { get; set; }

        public int TotalSdgParticipations { get; set; }

        public double TotalSdgHours { get; set; }

        public List<SdgStatisticDto> SdgStatistics { get; set; } = new();
    }

  
}
