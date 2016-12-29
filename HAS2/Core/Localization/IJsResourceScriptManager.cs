using System.Collections.Generic;

namespace HAS2.Core.Localization
{
    public interface IJsResourceScriptManager
    {
        string GetScript(IEnumerable<KeyValuePair<string, string>> dictionary, JsNamespaceCollection namespaces = null );
        bool VarIsMainVar { get; set; }
    }
}
