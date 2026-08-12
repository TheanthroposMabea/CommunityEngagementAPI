namespace CommunityEngagementAPI.Models.DTOs
{
    public class NGOApplicationDto
    {
        public string Reg_Number { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ClosestCampus { get; set; }
        public string Address { get; set; }
        public string Rep_Name { get; set; }
        public string Rep_Position { get; set; }
        public string Rep_Phone { get; set; }
        public string Key_Focus { get; set; }
        public string Collaboration_Request { get; set; }
        public string Description { get; set; }
        public string Benefits { get; set; }
        public string Website { get; set; }

        public ICollection<IFormFile> Files { get; set; } = new List<IFormFile>();
        public int[]? Sdg { get; set; }

    }
}
