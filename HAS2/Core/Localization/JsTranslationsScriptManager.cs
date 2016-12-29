using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAS2.Core.Localization
{
    public class JsTranslationsScriptManager : JsResourceScriptBase, IJsTranslationsScriptManager
    {
        protected List<KeyValuePair<string, string>> handledEntries; 
        
        public IEnumerable<KeyValuePair<string, string>> DefaultLanguageEntries { get; set; }


        public override string VariableName
        {
            get { return "Translations"; }
            set { }
        }

        public JsTranslationsScriptManager()
        {
            handledEntries= new List<KeyValuePair<string, string>>();
        }

        protected override void CreateItem(StringBuilder builder, KeyValuePair<string,string> entry)
        {
            builder.AppendFormat(@" '{0}': '{1}', ", entry.Key, SanitizeString(entry.Value));
            handledEntries.Add(entry);
        }

        protected override void AddItemsAfterCreateItems(StringBuilder builder)
        {
            base.AddItemsAfterCreateItems(builder);
            if (DefaultLanguageEntries == null || !DefaultLanguageEntries.Any()) return;
            
            foreach (var entry in DefaultLanguageEntries)
            {
                var currKey = entry.Key;
                var entryFound = handledEntries.FirstOrDefault(x => x.Key == currKey);
                if (entryFound.Key == null)
                {
                    CreateItem(builder,entry);
                }
            }
        }
    }
}