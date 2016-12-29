using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAS2.Core.Localization
{
    public abstract class JsResourceScriptBase : IJsResourceScriptManager
    {
        private JsNamespaceCollection _namespaces;
        private bool _varIsMainVar = true;

        public bool VarIsMainVar
        {
            get { return _varIsMainVar; } 
            set { _varIsMainVar = value; }
        }

        protected JsNamespaceCollection NameSpaces
        {
            get { return _namespaces ?? (_namespaces = new JsNamespaceCollection()); }
        }

        public abstract string VariableName { get; set; }

        public string GetScript(IEnumerable<KeyValuePair<string, string>> dictionary, JsNamespaceCollection namespaces = null)
        {
            var builder = new StringBuilder();
            if (dictionary == null) return string.Empty;

            var keyValuePairs = dictionary as IList<KeyValuePair<string, string>> ?? dictionary.ToList();
            if (keyValuePairs.Any())
            {
                _namespaces = namespaces;
                CreateHeader(builder);
                var currentIndex = 0;
                foreach (var entry in keyValuePairs)
                {
                    ApplyBeginNamespace(builder, currentIndex);
                    CreateItem(builder, entry);
                    ApplyEndNamespace(builder, currentIndex);
                    currentIndex++;
                }
            }

            AddItemsAfterCreateItems(builder);
            CreateFooter(builder);

            return builder.ToString();
        }



        #region [helpers]

        protected void CreateHeader(StringBuilder builder)
        {
            var format = VarIsMainVar ? @"var {0} = {{ " : @"{0} = {{ ";
            builder.AppendFormat(format, VariableName);
        }

        protected abstract void CreateItem(StringBuilder builder, KeyValuePair<string, string> entry);

        protected virtual void AddItemsAfterCreateItems(StringBuilder builder) { }

        protected void ApplyBeginNamespace(StringBuilder builder, int currentIndex)
        {
            if (!NameSpaces.Any()) return;
            var ns = NameSpaces.TryBeginNamespace(currentIndex);
            if (!string.IsNullOrEmpty(ns))
            {
                builder.Append(ns).Append(": { ");
            }
        }

        protected void ApplyEndNamespace(StringBuilder builder, int currentIndex)
        {
            if (!NameSpaces.Any()) return;
            var ns = NameSpaces.TryEndNamespace(currentIndex);
            if (string.IsNullOrEmpty(ns)) return;
            builder.Remove(builder.Length - 2, 2);
            builder.Append(" }, ");
        }

        protected void CreateFooter(StringBuilder builder)
        {
            if (builder.Length > 2) { 
                builder.Remove(builder.Length - 2, 2);
                builder.Append(" };");
            }
            
        }


        protected string SanitizeString(string str)
        {
            return str.Replace("'", "\'").Replace(Environment.NewLine, "");
        }

        #endregion
    }
}