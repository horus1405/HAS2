using System.Web.Mvc;

namespace HAS2.Core.Utilities
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// From string to MvcHtmlString.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static MvcHtmlString ToMvcHtmlString(this string str)
        {
            return new MvcHtmlString(str);
        }

        /// <summary>
        /// Transforms the spaces into underscores.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string UnderscorizeSpaces(this string str)
        {
            return Utils.UnderscorizeSpaces(str);
        }

        /// <summary>
        /// Adds a hash querystring to a URL (string).
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="hashChain">The hash chain.</param>
        /// <returns></returns>
        public static string AddHashToUrl(this string url, string hashChain)
        {
            var newUrl = url;
            if (!newUrl.EndsWith("/"))
            {
                newUrl += "/";
            }
            return string.Format("{0}#{1}", newUrl, hashChain);
        }

        /// <summary>
        /// Cdnizes the specified string by replacing @@CDNURL@@ in the string with the CDN URL.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string Cdnize(this string str)
        {
            return str.Replace("@@CDNURL@@", Utils.CDNUrl());
        }
    }
}