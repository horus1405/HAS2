using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using HAS2.Core.Utilities;

namespace HAS2.Core.Helpers
{
    public static class UrlHelpers
    {
        /// <summary>
        /// Determines whether the specified URL is SSL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsSSL(this UrlHelper url)
        {
            return Utils.IsSSL();
        }

        /// <summary>
        /// Determines whether [is debugging enabled].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsDebuggingEnabled(this UrlHelper url)
        {
            return url.RequestContext.HttpContext.IsDebuggingEnabled;
        }

        /// <summary>
        /// Returns the CDN url defined in the web.config.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string CDNUrl(this UrlHelper url)
        {
            return Utils.CDNUrl();
        }

        /// <summary>
        /// Returns the CDN url defined in the web.config.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static Uri CDNUri(this UrlHelper url)
        {
            return new Uri(url.CDNUrl());
        }

        /// <summary>
        /// Returns the current application's path.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string ApplicationPath(this UrlHelper url)
        {
            return url.Content("~/");
        }

        /// <summary>
        /// Returns the path of a resource using the CDN url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="path">The resource path.</param>
        /// <returns></returns>
        public static string CDNResource(this UrlHelper url, string path)
        {
            return CDNResource(url, path, false);
        }

        /// <summary>
        /// Returns the path of a resource using the CDN url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="path">The resource path.</param>
        /// <param name="localized">if set to <c>true</c> the resource path is [localized].</param>
        /// <returns></returns>
        public static string CDNResource(this UrlHelper url, string path, bool localized)
        {
            try
            {
                if (!localized)
                {
                    return new Uri(url.CDNUri(), path).ToString();
                }
                if (path.LastIndexOf('.') > 0)
                {
                    var fileName = path.Substring(0, path.LastIndexOf('.'));
                    var ext = path.Substring(path.LastIndexOf('.') + 1);
                    return new Uri(url.CDNUri(), String.Format("{0}_{1}.{2}", fileName, Utils.TwoLetterISOLanguageName().ToLower(), ext)).ToString();
                }
                return new Uri(url.CDNUri(), String.Format(path, Utils.TwoLetterISOLanguageName().ToLower(), "{0}_{1}")).ToString();
            }
            catch (Exception ex){
                Utils.Log(ex);
            }
            return new Uri(url.CDNUri(), path).ToString();
            
        }

        /// <summary>
        /// Generates a script tag using a js created on the server.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="jsAction">The js action that will define which method generates the js.</param>
        /// <returns></returns>
        public static MvcHtmlString ServerScriptLink(this UrlHelper url, string jsAction)
        {
            var s = @"<script src=""{0}"" type=""text/javascript""></script>";
            var u = url.Action(jsAction, "Javascript",  new { l = Utils.TwoLetterISOLanguageName().ToLower(), v = Configuration.JSCacheVersion });
            s = string.Format(CultureInfo.InvariantCulture, s, u);
            return new MvcHtmlString(s);
        }

        /// <summary>
        /// Creates a script link for a specific resource.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="path">The path.</param>
        /// <param name="useMinimizationConvention">if set to <c>true</c> [use minimization convention].</param>
        /// <returns></returns>
        public static MvcHtmlString ScriptLink(this UrlHelper url, string path, bool useMinimizationConvention = true)
        {
            const string SCRIPT_TEMPLATE = @"<script src=""{0}"" type=""text/javascript""></script>";
            if (useMinimizationConvention)
            {
                path += url.IsDebuggingEnabled() ? ".js" : ".min.js";
            }

            var u = url.ContentVersion(path);
            var s = string.Format(CultureInfo.InvariantCulture, SCRIPT_TEMPLATE, u);
            return new MvcHtmlString(s);
        }


        /// <summary>
        /// Creates a script link for some specific resources.
        /// If a bundle is provide it will be used when in release mode.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="scripts">The scripts.</param>
        /// <param name="minimizedBundle">The minimized bundle.</param>
        /// <returns></returns>
        public static MvcHtmlString ScriptLink(this UrlHelper url, string[] scripts, string minimizedBundle = null)
        {
            var sb = new StringBuilder();
            AppendScript(sb, url, minimizedBundle == null || url.IsDebuggingEnabled() ? scripts : new[] { minimizedBundle });
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Returns a file path with the language and the version added as querystring.
        /// Very useful for js and css.
        /// It also uses CDNResource function so it will create the proper url for CDN use right away.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string ContentVersion(this UrlHelper url, string path)
        {
            var finalPath = string.Format("{0}?l={1}&v={2}", path, Utils.TwoLetterISOLanguageName().ToLower(), Configuration.JSCacheVersion);
            var u = url.CDNResource(finalPath);
            return u;
        }


        ///<summary>
        ///Appends the script tag to a stringbuilder. Used by ControlScriptLink.
        ///</summary>
        ///<param name="sb">The stringbuilder.</param>
        ///<param name="url">The url helper.</param>
        ///<param name="scripts">The script you want to append.</param>
        private static void AppendScript(StringBuilder sb, UrlHelper url, IEnumerable<string> scripts)
        {
            const string SCRIPT_TEMPLATE = @"<script src=""{0}"" type=""text/javascript""></script>";
            foreach (var script in scripts)
            {
                var u = string.Format(CultureInfo.InvariantCulture, url.ContentVersion("{0}"), script);
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, SCRIPT_TEMPLATE, u));
            }
        }

        /// <summary>
        /// Gets the controller name of the current view.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string ControllerName(this UrlHelper url)
        {
            return url.RequestContext.RouteData.Values["controller"].ToString();
        }
    }
}