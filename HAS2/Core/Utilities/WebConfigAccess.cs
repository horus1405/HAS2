using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;

namespace HAS2.Core.Utilities
{
    /// <summary>
    /// Class to manage web.config access.
    /// </summary>
    public class WebConfigAccess
    {

        /// <summary>
        /// Gets a string from web.config. 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return GetString(key, string.Empty);
        }

        /// <summary>
        /// Gets a string from web.config. 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultvalue">Default value if key does not exist</param>
        /// <returns></returns>
        public static string GetString(string key, string defaultvalue)
        {
            var res = defaultvalue;
            if (ConfigurationManager.AppSettings[key] != null)
            {
                res = ConfigurationManager.AppSettings[key].ToString(CultureInfo.InvariantCulture);
            }
            return res;
        }

        /// <summary>
        /// Gets the culturized string.
        /// It looks for a key in web.config and also for key_DE,key_FR being DE/FR the current culture in capitals.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetCulturizedString(string key)
        {
            var current = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            var res = GetString(key);
            if (current != "en")
            {
                var resCult = GetString(key + "_" + current.ToUpper());
                if (!String.IsNullOrEmpty(resCult))
                    res = resCult;
            }
            return res;
        }

        /// <summary>
        /// Gets a boolean from web.config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool GetBoolean(string key)
        {
            var b = GetString(key);
            return b == "1";
        }

        /// <summary>
        /// Gets a boolean from web.config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultvalue">The default value</param>
        /// <returns></returns>
        public static bool GetBoolean(string key, bool defaultvalue)
        {
            var b = GetString(key);
            return (b.Length==0 ? defaultvalue :  b == "1");
        }

        /// <summary>
        /// Gets a integer from web.config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static int GetInteger(string key)
        {
            return GetInteger(key, 0);
        }

        /// <summary>
        /// Gets a integer from web.config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultvalue">The default value.</param>
        /// <returns></returns>
        public static int GetInteger(string key, int defaultvalue)
        {
            int ret;
            var res = GetString(key, defaultvalue.ToString(CultureInfo.InvariantCulture));
            return Int32.TryParse(res, out ret) ? ret : 0;
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static decimal GetDecimal(string key)
        {
            decimal ret;
            var res = GetString(key);
            return Decimal.TryParse(res, out ret) ? ret : 0m;
        }

        /// <summary>
        /// Gets the complete path from relative path.
        /// </summary>
        /// <param name="key">The webconfig key.</param>
        /// <returns></returns>
        public static string GetPath(string key)
        {
            return HttpContext.Current.Server.MapPath(GetString(key));
        }

        /// <summary>
        /// Filters the app settings.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> FilterAppSettings(string searchCriteria)
        {
            var entries = new List<KeyValuePair<string, string>>();
            if (ConfigurationManager.AppSettings.Count > 0)
            {
                var keys = ConfigurationManager.AppSettings.AllKeys;
                foreach (var key in keys)
                {
                    if (key.ToLower().IndexOf(searchCriteria.ToLower(), StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        entries.Add(new KeyValuePair<string, string>(key, GetString(key)));
                    }
                }
            }
            return entries;
        }
    }
}