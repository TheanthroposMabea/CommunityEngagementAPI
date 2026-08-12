namespace CommunityEngagementAPI.Models.DTOs
{
    public class PartnershipDto
    {
       
        public int DaysToRenewal { get; set; }
        public bool Is_Active { get; set; }
        public string Notes { get; set; }
        public string CBO_Name { get; set; }
        public string CBO_Email { get; set; } 
        public string CBO_Address { get; set; }     
        public string CBO_Key_Focus { get; set; }
        public string Collaboration_Request { get; set; }
        public string Description { get; set; }
        public string Benefits { get; set; }
    }
}
