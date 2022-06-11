using System;
using Foundation;

namespace Microsoft.VisualStudio.Settings
{
    internal class WritableSettingsStore : SettingsStore
    {
        internal void CreateCollection(string cOLLECTION_PATH)
        {
            //throw new NotImplementedException();
        }

        internal void SetBoolean(string collectionPath, string property, bool value)
        {
            defaults.SetBool(value, GetIdentifier(collectionPath, property));
        }

        internal void SetInt32(string collectionPath, string property, int value)
        {
            defaults.SetInt(value, GetIdentifier(collectionPath, property));
        }

        internal void SetString(string collectionPath, string property, string value)
        {
            defaults.SetString(value, GetIdentifier(collectionPath, property));
        }
    }
}