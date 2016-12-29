using System.Collections.Generic;

namespace HAS2.Core.Localization
{
    public interface IJsTranslationsScriptManager : IJsResourceScriptManager
    {
        IEnumerable<KeyValuePair<string, string>> DefaultLanguageEntries { get; set; }
    }
}
