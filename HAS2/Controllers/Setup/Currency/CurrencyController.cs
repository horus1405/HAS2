using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAS2.Models.Common;
using HAS2.Core.Helpers;

namespace HAS2.Controllers.Setup.Currency
{
    public class CurrencyController : BaseController
    {
        [HttpPost]
        public JsonResult List()
        {
            List<HosObjectLibrary.Setup.Currency> _currencies = null;
            

            string err = string.Empty;
            try
            {
                _currencies = HosDataLib.Setup.DEvent.selectCurrencies(SessionDataHelper.CurrentByromUser);
            }
            catch (Exception e) { }

            return Json(new JsonResultBase(_currencies, true, err, null));
        }

        [HttpPost]
        public JsonResult Get(int id)
        {
            List<HosObjectLibrary.Setup.Currency> _currencies = null;
            HosObjectLibrary.Setup.Currency _currency = null;


            string err = string.Empty;
            try
            {
                _currencies = HosDataLib.Setup.DEvent.selectCurrencies(SessionDataHelper.CurrentByromUser);
                if (_currencies != null)
                {
                    _currency = _currencies.Where(o => o.ID == id).Single();
                }

            }
            catch (Exception e) { }

            return Json(new JsonResultBase(_currencies, true, err, null));
        }
    }
}