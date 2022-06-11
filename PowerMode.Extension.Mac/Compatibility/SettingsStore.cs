
using System;
using Foundation;

namespace Microsoft.VisualStudio.Settings
{
    internal class SettingsStore
    {
        protected NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

        protected string GetIdentifier(string collectionPath, string identifier)
        {
            return string.Format("{0}|{1}", collectionPath, identifier);
        }

        internal bool CollectionExists(string collectionPath)
        {
            return true;
        }

        internal bool PropertyExists(string collectionPath, string property)
        {
            return defaults.DataForKey(GetIdentifier(collectionPath, property)) != null;
        }

        internal bool? GetBoolean(string collectionPath, string property)
        {
            return defaults.BoolForKey(GetIdentifier(collectionPath, property));
        }

        internal int? GetInt32(string collectionPath, string property)
        {
            return (int)defaults.IntForKey(GetIdentifier(collectionPath, property));
        }

        internal string GetString(string collectionPath, string property)
        {
            return defaults.StringForKey(GetIdentifier(collectionPath, property));
        }
    }
}