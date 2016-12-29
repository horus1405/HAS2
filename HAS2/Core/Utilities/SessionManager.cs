using System;
using System.Collections.Generic;
using System.Web;

namespace HAS2.Core.Utilities
{
    public static class SessionManager
    {

        /// <summary>
        /// Clears all the session items.
        /// </summary>
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// Clears the session manager; only session items managed by the session manager.
        /// </summary>
        public static void ClearSessionManager()
        {
            //HttpContext.Current.Session[PENDING_CLIENT_CART_ITEMS] = null;
            //HttpContext.Current.Session[FIRST_CARTITEM_TIME] = null;
        }

    }
}