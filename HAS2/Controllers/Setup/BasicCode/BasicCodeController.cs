using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAS2.Models.Common;
using HAS2.Core.Helpers;

namespace HAS2.Controllers.Setup.BasicCode
{
    public class BasicCodeController : BaseController
    {
        

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.ServerAndClient, Duration = 300, VaryByParam = "*")]
        public JsonResult List()
        {
            List<HosObjectLibrary.Setup.UserTable> userTables=null;
            List<object> data = new List<object>();

            string err = string.Empty;
            try
            {
                userTables = HosDataLib.Setup.DUserTable.selectUserTables(SessionDataHelper.CurrentByromUser, "en");
                foreach (var t in userTables) { data.Add(new { ID = t.TableId, Name = t.TableName }); }
            }
            catch (Exception e) { }

            return Json(new JsonResultBase(data, true, err, null));
        }

        [HttpPost]
        public JsonResult Get(int id)
        {
            HosObjectLibrary.Setup.UserTable userTable = null;            

            string err = string.Empty;
            try
            {
                userTable = HosDataLib.Setup.DUserTable.selectUserTables(SessionDataHelper.CurrentByromUser, "en").Where(o => o.TableId == id).SingleOrDefault();
                
            }
            catch (Exception e) { }

            return Json(new JsonResultBase(userTable, true, err, null));
        }

        [HttpPost]
        public JsonResult Save(HosObjectLibrary.Setup.UserTable basiccodetype)
        {
            string err = string.Empty;
            try
            {
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    foreach (var basicCode in basiccodetype.Values)
                    {
                        AdjustBusinessEntityForSave(basicCode);
                        if (basicCode.HasChanges == HosObjectLibrary.ActionStatus.Insert) { HosDataLib.Setup.DTableValue.insertTableValue(SessionDataHelper.CurrentByromUser, basicCode); }
                        else if (basicCode.HasChanges == HosObjectLibrary.ActionStatus.Modified) { HosDataLib.Setup.DTableValue.updateTableValue(SessionDataHelper.CurrentByromUser, basicCode); }
                        else if (basicCode.HasChanges == HosObjectLibrary.ActionStatus.Deleted) { HosDataLib.Setup.DTableValue.deleteTableValue(SessionDataHelper.CurrentByromUser, basicCode); }
                    }
                    scope.Complete();
                }                
                return Get(basiccodetype.TableId);
            }
            catch (Exception e) {
                return Json(new JsonResultBase(basiccodetype, true, err, null));
            }            
        }

    }
}