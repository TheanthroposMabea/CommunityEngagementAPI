namespace CommunityEngagementAPI.Models.DTOs
{
    public class SdgStatisticDto
    {
        public int Sdg_Id { get; set; }

        public string Sdg_Name { get; set; }

        // Number of unique volunteers linked to this SDG
        public int VolunteerCount { get; set; }

        // Percentage of all volunteers participating in SDGs
        public double VolunteerPercentage { get; set; }

        // Hours attributed to projects associated with this SDG
        public double Hours { get; set; }

        // Percentage of all SDG-attributed hours
        public double HoursPercentage { get; set; }
    }
}
