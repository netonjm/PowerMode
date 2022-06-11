namespace PowerMode.Extension.Mac.Services
{
    using System;

    using Microsoft.VisualStudio.Shell;

    using PowerMode.Extension.Mac.Settings;

    public class SettingsService
    {
        private static GeneralSettings generalSettingsCache = null;
        private static ComboModeSettings comboModeSettingsCache = null;
        private static ParticlesSettings particlesSettingsCache = null;
        private static ScreenShakeSettings screenShakeSettingsCache = null;


        public static GeneralSettings GetGeneralSettings()
        {
            if (generalSettingsCache == null)
            {
                generalSettingsCache = SettingsRepository.GetGeneralSettings();
            }

            return generalSettingsCache;
        }

        public static ComboModeSettings GetComboModeSettings()
        {
            if (comboModeSettingsCache == null)
            {
                comboModeSettingsCache = SettingsRepository.GetComboModeSettings(CurrentContext.ServiveProvider);
            }

            return comboModeSettingsCache;
        }

        public static ParticlesSettings GetParticlesSettings()
        {
            if (particlesSettingsCache == null)
            {
                particlesSettingsCache = SettingsRepository.GetParticlesSettings(CurrentContext.ServiveProvider);
            }

            return particlesSettingsCache;
        }

        public static ScreenShakeSettings GetScreenShakeSettings()
        {
            if (screenShakeSettingsCache == null)
            {
                screenShakeSettingsCache = SettingsRepository.GetScreenShakeSettings(CurrentContext.ServiveProvider);
            }

            return screenShakeSettingsCache;
        }

        public static void SaveToStorage(GeneralSettings settings)
        {
            if (settings == null) { throw new ArgumentNullException("settings"); }

            SettingsRepository.SaveToStorage(settings, CurrentContext.ServiveProvider);
            generalSettingsCache.CloneFrom(settings);
        }

        public static void SaveToStorage(ComboModeSettings settings)
        {
            if (settings == null) { throw new ArgumentNullException("settings"); }

            SettingsRepository.SaveToStorage(settings, CurrentContext.ServiveProvider);

            comboModeSettingsCache.CloneFrom(settings);
        }

        public static void SaveToStorage(ParticlesSettings settings)
        {
            if (settings == null) { throw new ArgumentNullException("settings"); }

            SettingsRepository.SaveToStorage(settings, CurrentContext.ServiveProvider);
            particlesSettingsCache.CloneFrom(settings);
        }

        public static void SaveToStorage(ScreenShakeSettings settings)
        {
            if (settings == null) { throw new ArgumentNullException("settings"); }

            SettingsRepository.SaveToStorage(settings, CurrentContext.ServiveProvider);

            screenShakeSettingsCache.CloneFrom(settings);
        }
    }
}
