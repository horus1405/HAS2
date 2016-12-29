using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using HAS2.Core.Filters;
using HAS2.Core.Localization;
using HAS2.Core.Utilities;
using HAS2.Resources;
using System.Linq;
using System.Collections.Generic;

namespace HAS2.Controllers
{
    [SSLFilter(SSLFilter.SSLPolicy.UseRequest)]
    public class JavascriptController : BaseController
    {
        protected ILocalizationManager LocalizationManager { get; set; }
        protected IJsTranslationsScriptManager JsTranslationsScriptManager { get; set; }
        protected IJsConfigurationScriptManager JsConfigurationScriptManager { get; set; }


        public JavascriptController(ILocalizationManager localizationManager, IJsTranslationsScriptManager jsTranslationsScriptManager, IJsConfigurationScriptManager jsConfigurationScriptManager)
        {
            LocalizationManager = localizationManager;
            JsTranslationsScriptManager = jsTranslationsScriptManager;
            JsConfigurationScriptManager = jsConfigurationScriptManager;
        }

        [OutputCache(Location = OutputCacheLocation.ServerAndClient, Duration = 31536000, VaryByParam = "*")]
        [AllowAnonymous]
        public JavaScriptResult GetTranslations()
        {
            var data = LocalizationManager.GetAll(ScriptMessages.ResourceManager);
            data = data.Concat(LocalizationManager.GetAll(ErrorMessages.ResourceManager));

            var defaultCulture = new CultureInfo("en");
            var sc = LocalizationManager.GetAll(ScriptMessages.ResourceManager, defaultCulture);
            var em = LocalizationManager.GetAll(ErrorMessages.ResourceManager, defaultCulture);
            var tl = sc.Concat(em);
            JsTranslationsScriptManager.DefaultLanguageEntries = tl;


            var script = JsTranslationsScriptManager.GetScript(data);
            return JavaScript(script);
        }

        [OutputCache(Location = OutputCacheLocation.ServerAndClient, Duration = 31536000, VaryByParam = "*")]
        [AllowAnonymous]
        public JavaScriptResult GetData()
        {
            const string mainVar = "Data";
            var builder = new StringBuilder("var Data = Data || {};");

            //AddDataCodeFromConfig(builder, "CODE_Round_", "Data.RoundCodes");
            //AddDataCodeFromConfig(builder, "CODE_Category_", "Data.CategoryCodes");
            //AddDataCodeFromConfig(builder, "CODE_Series_", "Data.SeriesCodes");
            //AddDataCodeFromConfig(builder, "CODE_Stadium_", "Data.StadiumCodes");
            //AddDataCodeFromConfig(builder, "js_", "Data.config");


            var baseAddress = Utils.IsSSL() ? Configuration.SiteUrlSSL : Configuration.SiteUrl;
            builder.Append(JsObjectSerializer.GetScript(baseAddress, mainVar, "baseAddress"));

            return JavaScript(builder.ToString());
        }

        private void AddDataCodeFromConfig(StringBuilder builder, string prefix, string varName)
        {
            JsConfigurationScriptManager.Prefix = prefix;
            var codes = WebConfigAccess.FilterAppSettings(JsConfigurationScriptManager.Prefix);
            JsConfigurationScriptManager.VarIsMainVar = false;
            JsConfigurationScriptManager.VariableName = varName;
            builder.Append(JsConfigurationScriptManager.GetScript(codes));
        }
        

    }
}
