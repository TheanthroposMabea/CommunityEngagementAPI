namespace CommunityEngagementAPI.Helpers
{
    public class TempStorage

    {
        public static Dictionary<string, (string otp, DateTime expiry)>
            Otp = new();

        public static Dictionary<string, (string token, DateTime expiry, int Application_Id)>
           DocumentUpload = new();

        //public static Dictionary<int, (int slots,int DaysLeft)>
        //    Slot = new();
    }



}
