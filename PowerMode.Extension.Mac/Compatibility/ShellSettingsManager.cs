using System;
using Microsoft.VisualStudio.Settings;

namespace Microsoft.VisualStudio.Shell.Settings
{
    internal enum SettingsScope
    {
        UserSettings
    }

    internal class ShellSettingsManager
    {
        private IServiceProvider serviceProvider;

        public ShellSettingsManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        internal WritableSettingsStore GetWritableSettingsStore(SettingsScope userSettings)
        {
            return new WritableSettingsStore();
        }
    }
}