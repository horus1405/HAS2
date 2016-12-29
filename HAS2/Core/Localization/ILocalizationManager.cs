using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace HAS2.Core.Localization
{
    public interface ILocalizationManager
    {
        IEnumerable<KeyValuePair<string, string>> FindItem(string key, ResourceManager manager);

        IEnumerable<KeyValuePair<string, string>> FindItemsByPrefix(string prefix, ResourceManager manager);

        IEnumerable<KeyValuePair<string, string>> GetAll(ResourceManager manager, CultureInfo culture = null);

        IEnumerable<KeyValuePair<string, string>> GetFromXml(Type type);
    }
}