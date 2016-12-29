using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAS2.Core.Helpers;
using HAS2.Models.Common;

namespace HAS2.Controllers.Sales.Account
{
    public class AccountController : BaseController
    {
        

        [HttpPost]
        public JsonResult List()
        {
            List<HosObjectLibrary.Sales.Account> accounts = null;
            List<object> data = new List<object>();

            string err = string.Empty;
            try
            {
                Byrom.Framework.Objects.systemBaseUser user = SessionDataHelper.CurrentByromUser;
                accounts = HosDataLib.Sales.DAccount.SelectAccounts(ref user, new HosObjectLibrary.Sales.Account());
                foreach (var a in accounts) { data.Add(new { ID = a.ID, Name = a.Name }); }
            }
            catch (Exception e) { }

            return Json(new JsonResultBase(data, true, err, null));
        }

        
        public JsonResult Get(int id)
        {
            HosObjectLibrary.Sales.Account account = null;            

            string err = string.Empty;
            try
            {
                Byrom.Framework.Objects.systemBaseUser user = SessionDataHelper.CurrentByromUser;
                account = HosDataLib.Sales.DAccount.GetAccount(ref user, new HosObjectLibrary.Sales.Account() { ID = id });                
            }
            catch (Exception e) { }

            return Json(new JsonResultBase(account, true, err, null));
        }
    }
}