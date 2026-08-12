namespace CommunityEngagementAPI.Models.DTOs
{
    public class UpdateProfileDto
    {
        public int User_Id { get; set; }
        //for alumni
        public DateOnly? Year_Graduated { get; set; }
        //for students
       
        public string? Student_Number { get; set; }
        public string? Year_Of_Study { get; set; }
        public string? Department { get; set; }
        public string? Qualification { get; set; }

     
    }
}
