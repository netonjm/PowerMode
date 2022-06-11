using System;


namespace PowerMode.Extension.Mac.Settings
{
    class ServiveProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    static class CurrentContext
    {
        public static ServiveProvider ServiveProvider { get; set; } = new ServiveProvider();
    }

    public static partial class SettingsRepository
    {
        private static readonly string GENERAL_SETTINGS_CATELOG = "General";

        public static GeneralSettings GetGeneralSettings()
        {
            GeneralSettings generalSettingsCache = null;
            var store = GetSettingsStore(CurrentContext.ServiveProvider);
            generalSettingsCache = new GeneralSettings();
            generalSettingsCache.IsEnablePowerMode = GetBoolOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnablePowerMode)).GetValueOrDefault(generalSettingsCache.IsEnablePowerMode);
            generalSettingsCache.IsEnableParticles = GetBoolOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableParticles)).GetValueOrDefault(generalSettingsCache.IsEnableParticles);
            generalSettingsCache.IsEnableScreenShake = GetBoolOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableScreenShake)).GetValueOrDefault(generalSettingsCache.IsEnableScreenShake);
            generalSettingsCache.IsEnableComboMode = GetBoolOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableComboMode)).GetValueOrDefault(generalSettingsCache.IsEnableComboMode);
            generalSettingsCache.IsEnableAudio = GetBoolOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableAudio)).GetValueOrDefault(generalSettingsCache.IsEnableAudio);
            var types = GetStringOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.ExcludedFileTypesString));
            if (types != null)
                generalSettingsCache.ExcludedFileTypesString = GetStringOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.ExcludedFileTypesString));

            return generalSettingsCache;
        }

        public static void SaveToStorage(GeneralSettings settings, IServiceProvider serviceProvider)
        {
            if (settings == null) { throw new ArgumentNullException("settings"); }

            var store = GetSettingsStore(serviceProvider);
            SetOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnablePowerMode), settings.IsEnablePowerMode);
            SetOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableParticles), settings.IsEnableParticles);
            SetOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableScreenShake), settings.IsEnableScreenShake);
            SetOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableComboMode), settings.IsEnableComboMode);
            SetOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.IsEnableAudio), settings.IsEnableAudio);
            SetOption(store, GENERAL_SETTINGS_CATELOG, nameof(GeneralSettings.ExcludedFileTypesString), settings.ExcludedFileTypesString);
        }
    }
}
