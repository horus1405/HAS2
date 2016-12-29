using System;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace HAS2.Core.Filters
{
    public class ElmahErrorHandlerFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var e = filterContext.Exception;
            if (!filterContext.ExceptionHandled || RaiseErrorSignal(e) || IsFiltered(filterContext))
            {
                return;
            }

            LogException(e);
        }

        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null) return false;

            var signal = ErrorSignal.FromContext(context);
            if (signal == null) return false;

            signal.Raise(e, context);
            return true;
        }

        /// <summary>
        /// Determines whether the specified context is filtered.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static bool IsFiltered(ExceptionContext context)
        {
            var config = context.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;
            if (config == null) return false;
            var testContext = new ErrorFilterModule.AssertionHelperContext(context.Exception, HttpContext.Current);
            return config.Assertion.Test(testContext);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        private static void LogException(Exception e)
        {
            ErrorSignal.FromCurrentContext().Raise(e);
        }
    }
}