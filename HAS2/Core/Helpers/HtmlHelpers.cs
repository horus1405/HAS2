using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Services.Description;
using HAS2.Core.Localization;
using HAS2.Core.Utilities;
using System.Collections.Generic;
using System.Resources;
using HAS2.Resources;

namespace HAS2.Core.Helpers
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Determines whether [is debugging enabled].
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static bool IsDebuggingEnabled(this HtmlHelper html)
        {
            return html.ViewContext.HttpContext.IsDebuggingEnabled;
        }

        /// <summary>
        /// Returns the current HTML scheme.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string CurrentHtmlScheme(this HtmlHelper html)
        {
            return string.Format("{0}://", Utils.IsSSL() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
        }


        /// <summary>
        /// Draws a Section title.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="title">The title of the section.</param>
        /// <returns></returns>
        public static MvcHtmlString SectionTitle(this HtmlHelper html, string title)
        {
            return SectionTitle(html, title, string.Empty);
        }

        /// <summary>
        /// Draws a Section title.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="title">The title of the section.</param>
        /// <param name="subtitle">The subtitle of the section</param>
        /// <returns></returns>
        public static MvcHtmlString SectionTitle(this HtmlHelper html, string title, string subtitle)
        {
            var vd = new ViewDataDictionary { { "Title", title }, { "Subtitle", subtitle } };
            return html.Partial("Controls/_sectionTitle", vd);
        }

        /// <summary>
        /// Draws a Mores link (text or graphic).
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="url">The URL of the link.</param>
        /// <param name="isMoreGraphic">if set to <c>true</c> the more link will be an image.</param>
        /// <returns></returns>
        public static MvcHtmlString More(this HtmlHelper html, string url, bool isMoreGraphic)
        {
            var vd = new ViewDataDictionary
            {
                { "Url", url },
                { "IsMoreGraphic", isMoreGraphic }
            };
            return html.Partial("Controls/_moreSection", vd);
        }

        /// <summary>
        /// Draws a venue info gallery box
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="venueName">Name of the venue.</param>
        /// <param name="venueInfo">The venue information.</param>
        /// <param name="numberOfImages">The number of images.</param>
        /// <returns></returns>
        public static MvcHtmlString VenueInfo(this HtmlHelper html, string venueName, string venueInfo, int numberOfImages)
        {
            //var vd = new ViewDataDictionary
            //{
            //    { "Title", venueName }, 
            //    { "VenueInfo", venueInfo.ToMvcHtmlString() },
            //    { "NumberOfImages", numberOfImages }
            //};
            //return html.Partial("_venueInfo", vd);
            return VenueInfo(html, venueName, venueInfo, numberOfImages, venueName);
        }

        /// <summary>
        /// Draws a venue info gallery box
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="venueName">Name of the venue.</param>
        /// <param name="venueInfo">The venue information.</param>
        /// <param name="numberOfImages">
        ///     The number of images. 
        /// </param>
        /// <param name="imageName"> Name of the image (using venue name depends on language).</param>
        /// ///
        /// <returns></returns>
        public static MvcHtmlString VenueInfo(this HtmlHelper html, string venueName, string venueInfo, int numberOfImages, string imageName)
        {
            var vd = new ViewDataDictionary
            {
                { "Title", venueName }, 
                { "VenueInfo", venueInfo.ToMvcHtmlString() },
                { "NumberOfImages", numberOfImages }, 
                { "ImageName", imageName }
            };
            return html.Partial("_venueInfo", vd);
        }

        /// <summary>
        /// Draws a venues map.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="linksToSamePage">if set to <c>true</c> [links to same page].</param>
        /// <returns></returns>
        public static MvcHtmlString VenueInfoMap(this HtmlHelper html, bool linksToSamePage)
        {
            var vd = new ViewDataDictionary
            {
                { "LinksToSamePage", linksToSamePage }
            };
            return html.Partial("Controls/_venueInformationMap", vd);
        }

        /// <summary>
        /// Draws a fifa banner.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static MvcHtmlString BannerRow(this HtmlHelper html)
        {
            return html.Partial("Ads/_bannerRow");
        }

        /// <summary>
        /// Draws a Series box.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="title">The title of the box.</param>
        /// <param name="text">The text of the box. Description.</param>
        /// <param name="imgName">Name of the image.</param>
        /// <param name="isImageTranslated">if set to <c>true</c> the image will be localized (_en, _es, ...).</param>
        /// <param name="availabilityAction">Action name of the availability button</param>
        /// <param name="css">Any css classes you want to add to the container.</param>
        /// <returns></returns>
        public static MvcHtmlString SeriesBox(this HtmlHelper html, string title, string text, string imgName, bool isImageTranslated, string availabilityAction, string css = "")
        {
            var vd = new ViewDataDictionary
            {
                { "Title", title },
                { "Text", text },
                { "ImageName", imgName },
                {"IsImageTranslated", isImageTranslated},
                {"AvailabilityAction", availabilityAction},
                {"Css", css}
            };
            return html.Partial("_seriesBox", vd);
        }

        /// <summary>
        /// Tiers the box.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text of the box. Description.</param>
        /// <param name="li1">The first list.</param>
        /// <param name="li2">The second list.</param>
        /// <param name="terms">The terms.</param>
        /// <param name="imgName">Name of the image.</param>
        /// <param name="isImageTranslated">if set to <c>true</c> the image will be localized (_en, _es, ...).</param>
        /// <param name="css">Any css classes you want to add to the container.</param>
        /// <returns></returns>
        public static MvcHtmlString TierBox(this HtmlHelper html,
            string title, string text,
            string li1, string li2,
            string terms, string imgName,
            bool isImageTranslated, string css = "")
        {
            var vd = new ViewDataDictionary
            {
                { "Title", title },
                { "Text", text },
                { "Li1", li1 },
                { "Li2", li2 },
                { "Terms", terms },
                { "ImageName", imgName },
                {"IsImageTranslated", isImageTranslated},
                {"Css", css}
            };
            return html.Partial("_tierBox", vd);
        }

        /// <summary>
        /// Gets a culturized link.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="lang">The language.</param>
        /// <returns></returns>
        public static MvcHtmlString CulturizedLink(this HtmlHelper html, string linkText, string lang)
        {
            var action = html.ViewContext.RouteData.Values["action"].ToString();
            return html.ActionLink(linkText, action, new { language = lang });
        }        

        /// <summary>
        /// Gets the page title.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="titleTag">It will look for Title as default.</param>
        /// <returns></returns>
        public static string GetPageTitle(this HtmlHelper html, string titleTag = "Title")
        {
            var res = html.GetPageResource(titleTag);
            if (string.IsNullOrWhiteSpace(res))
            {
                res = html.ViewData["title"].ToString();
            }
            return string.Format("{0} - {1}", Strings.ApplicationName, res);
        }

        /// <summary>
        /// Gets a resource.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="resourceFilePath">The resource file path.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        public static string GetResource(HtmlHelper html, string resourceFilePath, string resourceKey)
        {
            return LocalizationManager.GetResource(resourceFilePath, resourceKey);
        }

        /// <summary>
        /// Gets a specific page related resource.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        public static string GetPageResource(this HtmlHelper html, string resourceKey)
        {
            //we're going to get the action and controller values
            //be careful: if child action we're going to get parent level values.
            string controller, action;
            if (html.ViewContext.IsChildAction)
            {
                controller = html.ViewContext.ParentActionViewContext.RouteData.Values["controller"].ToString();
                action = html.ViewContext.ParentActionViewContext.RouteData.Values["action"].ToString();
            }
            else
            {
                controller = html.ViewContext.RouteData.Values["controller"].ToString();
                action = html.ViewContext.RouteData.Values["action"].ToString();
            }

            var pageResourceKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", controller, action,
                resourceKey);
            var res = GetResource(html, "Resources.PageResources", pageResourceKey);
            return res;
        }


        /// <summary>
        /// Creates a loader
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="css">The CSS.</param>
        /// <param name="attrs">The attrs.</param>
        /// <returns></returns>
        public static MvcHtmlString Loader(this HtmlHelper html, string css = null, string attrs = null)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);
            var imgPath = url.CDNResource("Content/images/common/loaders/fb.gif");
            var s = string.Format(@"<img src=""{0}"" alt=""{1}"" class=""{2}"" {3} />",
                imgPath, Strings.Loading, css ?? "", attrs ?? "");
            return new MvcHtmlString(s);
        }        
        
    }
}