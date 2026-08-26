namespace CommunityEngagementAPI.Helpers
{
    public class OtpStorage

    {
        public static Dictionary<string, (string otp, DateTime expiry)>
            Otp = new();
    }
}
