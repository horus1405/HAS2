using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAS2.Models.Common;
using HAS2.Core.Helpers;
using HosObjectLibrary.Setup;
using HAS2.Core.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HAS2.Controllers
{
    [SSLFilter(SSLFilter.SSLPolicy.SSLNonRequired)]
    public class BaseController : Controller
    {

        // GET: BasicCode
        public virtual ActionResult Index()
        {
            return View(this.GetViewPath());
        }

        protected const string DEFAULT_VIEW_NAME = "/index";

        protected string GetViewPath()
        {
            return GetViewPath(null);
        }

        protected string GetViewPath(string viewname)
        {
            string viewpath = "~/Views";
            string assemblyname = this.GetType().FullName;
            string[] assemblynameParts = assemblyname.Split('.');
            if (assemblynameParts.Length > 2)
                for (int i = 2; i < assemblynameParts.Length-1; i++)
                    viewpath += "/" + assemblynameParts[i];
            if (string.IsNullOrEmpty(viewname)) { viewname = DEFAULT_VIEW_NAME; }
            viewpath += viewname + ".cshtml";
            return viewpath;
        }

        public JsonResult GenericGet<T>(Byrom.Framework.Objects.systemBaseUser _user, Func<Byrom.Framework.Objects.systemBaseUser, T> getter)
        {
            T obj = default(T); ;
            bool result = true;
            string err = string.Empty;
            try
            {
                obj = getter(_user);
            }
            catch (Exception e) { HAS2.Core.Utilities.Utils.Log(e); err = e.Message; }

            return Json(new JsonResultBase<T>(obj, result, err));
        }

        public JsonResult GenericGet<T>(Byrom.Framework.Objects.systemBaseUser _user, int id, Func<Byrom.Framework.Objects.systemBaseUser, int, T> getter)
        {
            T obj = default(T);
            bool result = true;
            string err = string.Empty;
            try
            {
                obj = getter(_user, id);
            }
            catch (Exception e) { HAS2.Core.Utilities.Utils.Log(e); err = e.Message; }

            return Json(new JsonResultBase<T>(obj, result, err));
        }

        public JsonResult GenericGet<T>(Byrom.Framework.Objects.systemBaseUser _user, T o, Func<Byrom.Framework.Objects.systemBaseUser, T, T> getter)
        {
            T obj = default(T);
            bool result = true;
            string err = string.Empty;
            try
            {
                obj = getter(_user, o);
            }
            catch (Exception e) { HAS2.Core.Utilities.Utils.Log(e); err = e.Message; }

            return Json(new JsonResultBase<T>(obj, result, err));
        }

        public JsonResult Json(JsonResultBase result)
        {
            return new HasJsonResult() { Data = result };
        }

        protected void AdjustBusinessEntityForSave(HosObjectLibrary.Management o){
            o.LogLastAction = o.GenerateLogLastAction();
            o.LogLastUserId = SessionDataHelper.CurrentByromUser.GlobalID;
            o.LogCreatedBy = SessionDataHelper.CurrentByromUser.GlobalID;
            o.LogHostName = Environment.MachineName;
        }
    }
}

public class HasJsonResult : JsonResult
{
  public object Data { get; set; }
 
  public HasJsonResult()
  {
  }

  public override void ExecuteResult(ControllerContext context)
  {
      HttpResponseBase response = context.HttpContext.Response;
      response.ContentType = "application/json";
      if (ContentEncoding != null)
          response.ContentEncoding = ContentEncoding;
      if (Data != null)
      {
          JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting.Indented };
          JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
          serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
          //serializerSettings.DateFormatString = "yyyy-MM-dd";          

          serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

          JsonSerializer serializer = JsonSerializer.Create(serializerSettings);
          serializer.Serialize(writer, Data);
          writer.Flush();
      }
  }

}