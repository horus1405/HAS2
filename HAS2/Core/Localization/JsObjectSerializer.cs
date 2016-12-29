using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.VisualBasic;

namespace HAS2.Core.Localization
{
    public class JsObjectSerializer
    {

        private static JavaScriptSerializer _serializer;

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// 
        public static JavaScriptSerializer Serializer
        {
            get { return _serializer ?? (_serializer = new JavaScriptSerializer()); }
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="objectToserialize">The object toserialize.</param>
        /// <param name="mainVarName">Name of the main variable.</param>
        /// <param name="secondaryVarName">This variable will depend on the mainVarName.</param>
        /// <returns></returns>
        public static string GetScript<TType>(TType objectToserialize, string mainVarName, string secondaryVarName = null)
        {
            if (string.IsNullOrWhiteSpace(mainVarName))
            {
                throw new ArgumentNullException("mainVarName");
            }
            if (objectToserialize.Equals(null))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(secondaryVarName))
            {
                return string.Format("var {0}={1}; ", mainVarName, Serializer.Serialize(objectToserialize));
            }
            return string.Format("{0}.{1}={2};", mainVarName, secondaryVarName, Serializer.Serialize(objectToserialize));

        }

        /// <summary>
        /// Gets the script of a static type.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="mainVarName">Name of the main var.</param>
        /// <param name="secondaryVarName">Name of the secondary var.</param><returns></returns>
        public static string GetScript<TType>(string mainVarName, string secondaryVarName = null)
        {
            var data = Utilities.Utils.GetConstants(typeof(TType));
            var res = BuildDataFromList(data, mainVarName, secondaryVarName);
            return res;
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="localizationManager">The localization manager.</param>
        /// <param name="mainVarName">Name of the main var.</param>
        /// <param name="secondaryVarName">Name of the secondary var.</param>
        /// <returns>Returns script</returns>
        public static string GetScript<TType>(ILocalizationManager localizationManager, string mainVarName, string secondaryVarName = null)
        {
            //ANOTHER USAGE if you have xmls
            var data = localizationManager.GetFromXml(typeof(TType));
            var res = BuildDataFromDictionary(data, mainVarName, secondaryVarName);
            return res;
        }

        /// <summary>
        /// Builds the data from list.
        /// </summary>
        /// <param name="l">The list of constants.</param>
        /// <param name="mainVarName">Name of the main var.</param>
        /// <param name="secondaryVarName">Name of the secondary var.</param><returns></returns>
        private static string BuildDataFromList(List<System.Reflection.FieldInfo> l, string mainVarName, string secondaryVarName)
        {
            var script = new StringBuilder();
            if (l == null || l.Count <= 0) return script.ToString();

            //starting declaration.
            if (string.IsNullOrWhiteSpace(secondaryVarName))
            {
                script.AppendFormat("var {0}= {{ ", mainVarName);
            }
            else
            {
                script.AppendFormat("{0}.{1}= {{ ", mainVarName, secondaryVarName);
            }
            //iterating through properties
            foreach (var fLoopVariable in l)
            {
                var f = fLoopVariable;
                //getting info
                var name = f.Name;
                var value = f.GetRawConstantValue();
                var format = " '{0}': '{1}', ";
                //checking if it's a number
                if (Information.IsNumeric(value))
                {
                    format = " '{0}': {1}, ";
                }
                script.AppendFormat(format, name, value);
            }
            //removing last comma
            script.Remove(script.Length - 2, 2);
            script.Append(" }; ");
            //returning
            return script.ToString();
        }

        /// <summary>
        /// Serializes from a KeyValuePair enumerable containing property name and value.
        /// </summary>
        /// <param name="dic">The Enumerable of KeyValuePair</param>
        /// <param name="mainVarName">Name of the main var</param>
        /// <param name="secondaryVarName">Name of the secondary var</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string BuildDataFromDictionary(IEnumerable<KeyValuePair<string, string>> dic, string mainVarName, string secondaryVarName)
        {
            var script = new StringBuilder();
            if (dic != null && dic.Any())
            {
                //starting declaration.
                if (string.IsNullOrWhiteSpace(secondaryVarName))
                {
                    script.AppendFormat("var {0}= {{ ", mainVarName);
                }
                else
                {
                    script.AppendFormat("{0}.{1}= {{ ", mainVarName, secondaryVarName);
                }
                //iterating through properties
                foreach (var entryLoopVariable in dic)
                {
                    var entry = entryLoopVariable;
                    if (Information.IsNumeric(entry.Value))
                    {
                        script.AppendFormat(" '{0}': {1}, ", entry.Key, entry.Value.Replace("'", "\\'"));
                    }
                    else
                    {
                        script.AppendFormat(" '{0}': '{1}', ", entry.Key, entry.Value.Replace("'", "\\'"));
                    }
                }
                //removing last comma
                script.Remove(script.Length - 2, 2);
                script.Append(" }; ");
            }
            return script.ToString();
        }


    }

}