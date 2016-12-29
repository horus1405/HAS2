using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAS2.Models.Common;
using HAS2.Core.Helpers;
using HosObjectLibrary.Setup;

namespace HAS2.Controllers.Setup.Event
{
    public class EventController : BaseController
    {

        [HttpPost]
        public JsonResult Get()
        {
            return GenericGet<HosObjectLibrary.Setup.HospitalityEvent>(SessionDataHelper.CurrentByromUser, HosDataLib.Setup.DEvent.getEvent);
        }

        public JsonResult Save(HospitalityEvent e)
        {            
            bool result = true;
            string err = string.Empty;

            try
            {
                HosDataLib.Setup.DEvent.updateEvent(SessionDataHelper.CurrentByromUser, e);
                e = HosDataLib.Setup.DEvent.getEvent(SessionDataHelper.CurrentByromUser);
            }
            catch(Exception ex)
            {
                result = false;
                err = ex.Message;
            }

            return Json(new JsonResultBase<HosObjectLibrary.Setup.HospitalityEvent>(e, result, err));
        }
        
    }
}