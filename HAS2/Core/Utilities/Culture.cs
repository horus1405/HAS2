using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace HAS2.Core.Utilities
{
    public static class CultureManager
    {
        public const string DEFAULT_LANGUAGE = "en";
        public const string LANGUAGE_STORAGE_NAME = "language";

        public static string SessionLanguage
        {
            get { return HttpContext.Current.Session[LANGUAGE_STORAGE_NAME] as string; }
            set { HttpContext.Current.Session[LANGUAGE_STORAGE_NAME] = value; }
        }

        public static string CurrentLanguage { get { return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName; } }
        public static string CurrentLanguageLower { get { return CurrentLanguage.ToLower(); } }


        public static string[] SupportedLanguages
        {
            get { return new[] { "en-US", "fr" }; }
        }

        private static string CookieName
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}Language", typeof(CultureManager).Namespace); }
        }


        public static void SetDemandedCultureAsDefault()
        {
            var culture = GetCurrentSelectedCulture();
            //get the cookie in order to use it later
            var ck = HttpContext.Current.Request.Cookies.Get(CookieName);

            if (string.IsNullOrWhiteSpace(culture))
            {
                //check cookie
                if (ck != null && !string.IsNullOrEmpty(ck[LANGUAGE_STORAGE_NAME]))
                {
                    culture = ck[LANGUAGE_STORAGE_NAME];
                }
                if (string.IsNullOrEmpty(culture))
                {
                    culture = DEFAULT_LANGUAGE;
                }
            }

            //setting thread
            try
            {
                //check supported culture
                if (!SupportedLanguages.Contains(culture))
                {
                    culture = DEFAULT_LANGUAGE;
                }

                SetThreadCulture(culture);

            }
            catch (Exception)
            {
                //if it fails maybe the culture string is not correct (the user may have entered some weird string)
                //so we set it to default: english
                //This won't fail:
                culture = DEFAULT_LANGUAGE;
                SetThreadCulture(culture);
            }

            //setting cookie
            if (ck == null)
            {
                ck = new HttpCookie(CookieName) { Expires = DateTime.Now.AddYears(1) };
            }
            ck[LANGUAGE_STORAGE_NAME] = culture;
            try
            {
                HttpContext.Current.Response.Cookies.Add(ck);
            }
            catch (Exception ex)
            {
                //this can fail because of the http headers.
                //swallow and go on
                Debug.WriteLine(ex);
            }

            //setting session
            SessionLanguage = culture;

        }

        private static void SetThreadCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
                culture = DEFAULT_LANGUAGE;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }

        /// <summary>
        /// Gets the currently selected culture.
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentSelectedCulture()
        {
            string demandedLanguage = null;

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
            if (routeData != null)
            {
                demandedLanguage = routeData.Values[LANGUAGE_STORAGE_NAME] as string;
            }

            if (!string.IsNullOrWhiteSpace(demandedLanguage))
            {
                return demandedLanguage;
            }

            return !string.IsNullOrWhiteSpace(SessionLanguage)
                ? SessionLanguage
                : demandedLanguage;
        }

    }
}