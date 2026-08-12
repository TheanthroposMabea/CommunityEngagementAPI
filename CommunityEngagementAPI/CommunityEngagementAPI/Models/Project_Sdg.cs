using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class Project_Sdg
    {

        
      
        public int Project_Id { get; set; }
        public int Sdg_Id { get; set; }



        [ForeignKey(nameof(Project_Id))]
        public Project Project { get; set; }

        [ForeignKey(nameof(Sdg_Id))]
        public Sdg Sdg { get; set; }
        
    }
}
