namespace HAS2.Core.Utilities
{
    public class Configuration
    {
        public static string CDNUrl { get { return WebConfigAccess.GetString("CDNUrl"); }}
        public static string CDNUrlSsl { get { return WebConfigAccess.GetString("CDNUrlSSL"); }}
        public static string SiteUrl { get { return WebConfigAccess.GetString("SiteUrl"); }}
        public static string SiteUrlSSL { get { return WebConfigAccess.GetString("SiteUrlSsl"); } }
        public static bool LanguagesActive { get { return WebConfigAccess.GetBoolean("LanguagesActive"); }}
        public static bool GoogleAnalyticsActive { get { return WebConfigAccess.GetBoolean("GoogleAnalyticsActive"); }}
        public static string GoogleAnalyticsDomainName { get { return WebConfigAccess.GetString("GoogleAnalyticsDomainName"); }}
        public static string GoogleAnalyticsAccount { get { return WebConfigAccess.GetString("GoogleAnalyticsAccount"); } }
        public static bool FifaAnalyticsActive { get { return WebConfigAccess.GetBoolean("FifaAnalyticsActive"); } }
        public static string FifaAnalyticsDomainName { get { return WebConfigAccess.GetString("FifaAnalyticsDomainName"); } }
        public static string FifaAnalyticsJsUrl { get { return WebConfigAccess.GetString("FifaAnalyticsJsUrl"); } }
        public static string FifaAnalyticsAccount { get { return WebConfigAccess.GetString("FifaAnalyticsAccount"); } }
        public static string JSCacheVersion { get { return WebConfigAccess.GetString("JSCacheVersion"); } }
        public static string ContactTo { get { return WebConfigAccess.GetString("Contact_to"); } }
        public static string ContactFrom { get { return WebConfigAccess.GetString("Contact_from"); } }
        public static string MailBCC { get { return WebConfigAccess.GetString("mail_bcc"); } }
        public static string ContactSubject { get { return WebConfigAccess.GetString("Contact_subject"); } }
        public static int SSLBehaviorOverride { get { return WebConfigAccess.GetInteger("SSLBehaviorOverride"); } }
        public static int SSLPort { get { return WebConfigAccess.GetInteger("SSLPort"); } }
        public static int NormalPort { get { return WebConfigAccess.GetInteger("NormalPort"); } }

        public static bool RunningPerformanceTests { get { return WebConfigAccess.GetBoolean("RunningPerformanceTests", false); } }        

        /* Cache Management */
        public static int DefaultCacheDuration { get { return WebConfigAccess.GetInteger("DefaultCacheDuration", 1); } }
        public static int GetCacheDuration(string cacheKey) { return WebConfigAccess.GetInteger(cacheKey, DefaultCacheDuration); }

        /* Manteinance Message */
        public static bool ManteinanceMessage { get { return WebConfigAccess.GetBoolean("ManteinanceMessage", false); } }

        
    }
}