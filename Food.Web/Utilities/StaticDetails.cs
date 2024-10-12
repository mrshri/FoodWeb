namespace Food.Web.Utilities
{
    public class StaticDetails
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string TokenCookie = "Token";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
