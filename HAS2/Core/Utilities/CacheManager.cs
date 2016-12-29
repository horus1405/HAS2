
using System;
using System.Web;
using System.Web.Caching;

namespace HAS2.Core.Utilities
{
   
    public class CacheManagement
    {        

        public static void AddtoCache(string key, object value, DateTime validUntil)
        {
            HttpContext.Current.Cache.Add(key, value, null, validUntil, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        public static object GetFromCache(string key, DateTime? validUntil = null)
        {
            try
            {
                return HttpContext.Current.Cache.Get(key);
            }
            catch (Exception ex)
            {
                Utils.Log(ex);
                return null;
            }
        }
    }
}