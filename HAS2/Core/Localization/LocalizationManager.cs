using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Xml;
using HAS2.Resources;

namespace HAS2.Core.Localization
{
    /// <summary>
    /// Localization Manager
    /// </summary>
    public class LocalizationManager : ILocalizationManager
    {
        #region [Private methods]

        private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// Gets the resource set.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        private ResourceSet GetResourceSet(ResourceManager resourceManager, CultureInfo culture = null)
        {
            if (resourceManager == null)
            {
                throw new ArgumentException("resource");
            }

            ResourceSet resourceSet = resourceManager.GetResourceSet(culture ?? Thread.CurrentThread.CurrentCulture, true, true);

            if (resourceSet == null || !resourceSet.Cast<DictionaryEntry>().Any())
            {
                resourceSet = resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            }

            return resourceSet;
        }

        /// <summary>
        /// Gets the item from resource.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="resourceSet">The resource set.</param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, string>> GetItemFromResource(string key, ResourceSet resourceSet)
        {
            return (from entry in resourceSet.Cast<DictionaryEntry>()
                    where entry.Key.ToString() == key
                    select new KeyValuePair<string, string>(entry.Key.ToString(), entry.Value.ToString())).ToList();
        }


        /// <summary>
        /// Gets the items from resource.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="resourceSet">The resource set.</param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, string>> GetItemsFromResource(string prefix,
            ResourceSet resourceSet)
        {
            return (from entry in resourceSet.Cast<DictionaryEntry>()
                    where entry.Key.ToString().StartsWith(prefix)
                    select new KeyValuePair<string, string>(entry.Key.ToString(), entry.Value.ToString())).ToList();
        }


        /// <summary>
        /// Gets all items from resource.
        /// </summary>
        /// <param name="resourceSet">The resource set.</param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, string>> GetAllItemsFromResource(ResourceSet resourceSet)
        {
            return (from entry in resourceSet.Cast<DictionaryEntry>()
                    select new KeyValuePair<string, string>(entry.Key.ToString(), entry.Value.ToString())).ToList();
        }

        /// <summary>
        /// Loads the XML.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Filled Xml document</returns>
        private XmlDocument LoadXml(string name)
        {
            using (
                var resourceStream =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Concat(AssemblyName, ".", name)))
            {
                if (resourceStream == null) return null;
                
                using (var resourceStreamReader = new StreamReader(resourceStream))
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(resourceStreamReader.ReadToEnd());
                    return doc;
                }
            }
        }

        #endregion

        #region [Interface]

        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="manager">The manager.</param>
        /// <returns>Return current translation by given key</returns>
        public string Translate(string key, ResourceManager manager)
        {
            if ((string.IsNullOrWhiteSpace(key)))
            {
                throw new ArgumentNullException("key");
            }

            if ((manager == null))
            {
                throw new ArgumentNullException("manager");
            }

            var translation = key;

            var pair = FindItem(key, Strings.ResourceManager);


            if ((pair != null && pair.Any()))
            {
                translation = pair.ToList()[0].Value;
            }

            return translation;
        }

        /// <summary>
        /// Finds the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> FindItem(string key, ResourceManager manager)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            return GetItemFromResource(key, GetResourceSet(manager));
        }

        /// <summary>
        /// Finds the items by prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> FindItemsByPrefix(string prefix, ResourceManager manager)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentException("prefix");
            }

            return GetItemsFromResource(prefix, GetResourceSet(manager));
        }


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> GetAll(ResourceManager resource, CultureInfo culture = null)
        {
            return GetAllItemsFromResource(GetResourceSet(resource, culture));
        }

        /// <summary>
        /// Gets the XML.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Dictionary with key value pairs from xml</returns>
        public IEnumerable<KeyValuePair<string, string>> GetFromXml(Type type)
        {
            var name = string.Concat(type.Name, ".xml");

            XmlDocument doc = LoadXml(name);

            var collection = new Dictionary<string, string>();


            var xmlNodeList = doc.SelectNodes("/class/const");
            if (xmlNodeList != null)
                foreach (XmlNode node in xmlNodeList)
                {
                    if (node.Attributes != null)
                    {
                        collection.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
                    }
                }

            return collection;
        }

        #endregion

        #region [Static methods]

        /// <summary>
        /// Gets the get resource manager.
        /// </summary>
        public static ResourceManager GetResourceManager(string baseName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            return new ResourceManager(string.Format("{0}.{1}", GetAssemblyName(), baseName), currentAssembly);
        }

        /// <summary>
        /// Gets a specific resource.
        /// </summary>
        /// <param name="resource">The resource base.</param><returns></returns>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="ignoreCase"></param>
        public static string GetResource(string resource, string resourceKey, bool ignoreCase = true)
        {
            var rm = GetResourceManager(resource);
            rm.IgnoreCase = ignoreCase;
            return rm.GetString(resourceKey);
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns></returns>
        public static object GetErrorMessage(int errorCode)
        {
            string resourceKey = "EnumDataIUD" + (errorCode * -1);
            return GetResource("ErrorMessages.ErrorMessages", resourceKey);
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyName()
        {
            var ns = Assembly.GetExecutingAssembly().GetTypes().First().Namespace;
            return ns != null ? ns.Replace(".My", "") : null;
        }

        #endregion
    }
}