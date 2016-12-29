using System;
using System.Web.Mvc;
using HAS2.Core.Utilities;

namespace HAS2.Core.Filters
{
    public class SSLFilter : RequireHttpsAttribute
    {
        public enum SSLPolicy
        {
            SSLRequired = 1,
            SSLNonRequired = 2,
            UseRequest = 3
        }

        protected SSLPolicy SSLBehavior { get; set; }

        protected bool AcceptComingRequest { get; set; }

        public SSLFilter(SSLPolicy ssl)
        {
            SSLBehavior = ssl;
            if (Configuration.SSLBehaviorOverride > 0 && Configuration.SSLBehaviorOverride < 4)
            {
                SSLBehavior = (SSLPolicy) Configuration.SSLBehaviorOverride;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            
            var req = filterContext.HttpContext.Request;
            if ((req.Url == null) 
                || (SSLBehavior == SSLPolicy.SSLRequired && req.IsSecureConnection) 
                || SSLBehavior == SSLPolicy.SSLNonRequired && !req.IsSecureConnection
                || SSLBehavior == SSLPolicy.UseRequest) return;

            //if we're here we want to force ssl or non ssl so...
            var isSSLRequired = SSLBehavior == SSLPolicy.SSLRequired;
            var ub = new UriBuilder(req.Url.AbsoluteUri) { Scheme = isSSLRequired ? Uri.UriSchemeHttps : Uri.UriSchemeHttp };
            var port = isSSLRequired ? Configuration.SSLPort : Configuration.NormalPort;
            //it will return 0 if the config parameter is not set so we assume default port.
            if (port == 0) port = isSSLRequired ? 443 : 80;
            ub.Port = port;
            filterContext.Result = new RedirectResult(ub.Uri.AbsoluteUri);
        }

       
    }
}