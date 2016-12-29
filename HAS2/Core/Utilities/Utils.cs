using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace HAS2.Core.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// Shorthand for current thread culture in 2 letters
        /// </summary>
        /// <returns></returns>
        public static string TwoLetterISOLanguageName()
        {
            return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }

        /// <summary>
        /// Logs the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Log(Exception ex)
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            Debug.WriteLine(ex);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message)
        {
            Log(new Exception(message));
        }

        /// <summary>
        /// Transforms the spaces into underscores.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string UnderscorizeSpaces(string str)
        {
            return str.Replace(" ", "_");
        }

        /// <summary>
        /// Determines whether the current httpcontext is SSL.
        /// </summary>
        /// <returns></returns>
        public static bool IsSSL()
        {
            return HttpContext.Current.Request.IsSecureConnection;
        }

        public static string CDNUrl()
        {
            var u = IsSSL() ? Configuration.CDNUrlSsl : Configuration.CDNUrl;
            if (!u.EndsWith("/"))
            {
                u += "/";
            }
            return u;
        }


        /// <summary>
        /// Gets the constants of a type.
        /// </summary>
        /// <param name="type">The type.</param><returns></returns>
        public static List<FieldInfo> GetConstants(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var constants = fields.Where(x => x.IsLiteral && !x.IsInitOnly);
            return constants.ToList();
        }

        /// <summary>
        /// Gets the value of the innerProperty inside the object.
        /// It supports nested property names o.inner.moreInner
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="innerPropertyName">Name of the inner property.</param>
        /// <returns></returns>
        public static object GetValue(object o, string innerPropertyName)
        {
            //check for composed property... o.inner.moreInner....
            //we'll look for any "." and take the first one and pass the rest in a recursive way.
            var props = innerPropertyName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var firstProp = props.First();

            var value = o.GetType().GetProperty(firstProp).GetValue(o, null);
            if (props.Length > 1)
            {
                var rest = innerPropertyName.Replace(firstProp + ".", "");
                value = GetValue(value, rest);
            }
            return value;
        }        


    }
}