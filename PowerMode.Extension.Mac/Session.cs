using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using PowerMode.Cocoa.Views;

namespace PowerMode
{
    class Session
    {
        List<DocumentPowerSession> documentLayers = new List<DocumentPowerSession>();

        internal LevelManager LevelManager { get; }

        public Session()
        {
            LevelManager = new LevelManager();

            isEnabled = Settings.GetBool(SettingsPropperties.IsEnabled);
            shakeEnabled = Settings.GetBool(SettingsPropperties.ShakeEnabled);
            counterEnabled = Settings.GetBool(SettingsPropperties.CounterEnabled);
            powerModeIndex = Settings.GetInt(SettingsPropperties.PowerModeIndex);
        }

        public void Reload()
        {
            foreach (var session in documentLayers)
            {
                session.GameView.SetData(PowerMode);
            }
        }

        public void Unattach(ICocoaTextView view)
        {
            var documentLayer = documentLayers.FirstOrDefault(s => s == view);
            if (documentLayer != null)
            {
                documentLayer.Unregister();
                documentLayers.Remove(documentLayer);
            }
        }

        public void Attach(ICocoaTextView view, ITextDocumentFactoryService textDocumentFactory)
        {
            var documentLayer = documentLayers.FirstOrDefault(s => s == view);
            if (documentLayer == null)
            {
                documentLayer = new DocumentPowerSession(view, textDocumentFactory);
                documentLayers.Add(documentLayer);
            }
        }

        static bool shakeEnabled = true;
        public bool ShakeEnabled
        {
            get => shakeEnabled;
            set
            {
                if (shakeEnabled == value)
                    return;
                shakeEnabled = value;
                foreach (var session in documentLayers)
                {
                    session.GameView.Shake = shakeEnabled;
                }
            }
        }

        bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value)
                    return;
                isEnabled = value;
                foreach (var session in documentLayers)
                {
                    session.GameView.Hidden = !isEnabled;
                    session.UnsuscribeEvents();
                    if (value)
                    {
                        session.SuscribeEvents();
                    }
                }
            }
        }

        bool counterEnabled;
        public bool IsCounterEnabled
        {
            get => counterEnabled;
            set
            {
                if (counterEnabled == value)
                    return;
                counterEnabled = value;
            }
        }

        public int ExplosionAmount { get; set; } = 2;
        public int ExplosionDelay { get; set; } = 50;
        public int MaxShakeAmount { get; set; } = 5;

        int powerModeIndex = 0;
        public int PowerModeIndex
        {
            get => powerModeIndex;
            set
            {
                if (powerModeIndex == value)
                    return;
                powerModeIndex = value;
            }
        }

        public PowerModeItem PowerMode => Configurations.Data[PowerModeIndex];

        static Session session;
        public static Session Current => session ??= new Session();
    }
}

