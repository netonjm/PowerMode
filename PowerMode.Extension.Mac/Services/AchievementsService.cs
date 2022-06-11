namespace PowerMode.Extension.Mac.Services
{
    using System;

    using Microsoft.VisualStudio.Shell;

    using PowerMode.Extension.Mac.Settings;

    public class AchievementsService
    {
        private static AchievementsSettings achievementsSettingsCache = null;


        public static AchievementsSettings GetAchievements()
        {
            if (achievementsSettingsCache == null)
            {
                achievementsSettingsCache = SettingsRepository.GetAchievements(CurrentContext.ServiveProvider);
            }

            return achievementsSettingsCache;
        }

        public static void SaveToStorage(AchievementsSettings settings)
        {
            if (settings == null) { throw new ArgumentNullException("settings"); }

            SettingsRepository.SaveToStorage(settings, CurrentContext.ServiveProvider);
            achievementsSettingsCache.CloneFrom(settings);
        }
    }
}
