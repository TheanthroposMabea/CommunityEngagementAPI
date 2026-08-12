using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEngagementAPI.Models
{
    public class NGO_Sdg
    {
        public int Ngo_Id { get; set; }
        public int Sdg_Id { get; set; }


        [ForeignKey(nameof(Ngo_Id))]
        public NGO_Application NGO_Application { get; set; }

        [ForeignKey(nameof(Sdg_Id))]
        public Sdg Sdg { get; set; }
    }
}
