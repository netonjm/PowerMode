using CoreGraphics;
using AppKit;
using Foundation;

namespace PowerMode
{
    public enum SettingsPropperties
    {
        PowerModeIndex,
        IsEnabled,
        CounterEnabled,
        ModeTimeout,
        ShakeEnabled,
    }

    static class Settings
    {
        static string GetPropertyName(SettingsPropperties propperty)
        {
            return "PowerMode" + propperty.ToString();
        }

        public static bool GetBool(SettingsPropperties propperty)
        => NSUserDefaults.StandardUserDefaults.BoolForKey(GetPropertyName(propperty));

        public static void SetBool(SettingsPropperties propperty, bool value)
         => NSUserDefaults.StandardUserDefaults.SetBool(value, GetPropertyName(propperty));

        public static string GetString(SettingsPropperties propperty)
          => (string)NSUserDefaults.StandardUserDefaults.StringForKey(GetPropertyName(propperty));

        public static void SetString(SettingsPropperties propperty, string value)
         => NSUserDefaults.StandardUserDefaults.SetString(value, GetPropertyName(propperty));

        public static int GetInt(SettingsPropperties propperty)
            => (int)NSUserDefaults.StandardUserDefaults.IntForKey(GetPropertyName(propperty));

        public static void SetInt(SettingsPropperties property, int variable)
            => NSUserDefaults.StandardUserDefaults.SetInt(variable, GetPropertyName(property));
    }
}
