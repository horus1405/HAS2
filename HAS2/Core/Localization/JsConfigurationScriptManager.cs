using System.Web;
using Microsoft.VisualBasic;

namespace HAS2.Core.Localization
{
    public class JsConfigurationScriptManager : JsResourceScriptBase, IJsConfigurationScriptManager
    {
        private string _prefix = "CODE_";
        private string _variableName = "ServerConfigurations";

        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        /// <value>
        /// The name of the variable.
        /// </value>
        public override string VariableName
        {
            get { return _variableName; }
            set { _variableName = value; }
        }

        /// <summary>
        /// Creates the item.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="entry">The entry.</param>
        protected override void CreateItem(System.Text.StringBuilder builder, System.Collections.Generic.KeyValuePair<string, string> entry)
        {
            var format = " '{0}': '{1}', ";
            if ((Information.IsNumeric(entry.Value)))
            {
                format = " '{0}': {1}, ";
            }
            builder.AppendFormat(format, entry.Key.Replace(Prefix, ""), SanitizeString(entry.Value));
        }

        /// <summary>
        /// Adds items after create items.
        /// </summary>
        /// <param name="builder">The builder.</param>

        protected override void AddItemsAfterCreateItems(System.Text.StringBuilder builder)
        {
            var relPath = VirtualPathUtility.ToAbsolute("~");
            relPath = relPath == "/" ? relPath : relPath + "/";
            builder.AppendFormat(" 'relPath':'{0}', ", relPath);
            base.AddItemsAfterCreateItems(builder);

        }

    }
}
