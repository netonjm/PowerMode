using Microsoft.VisualStudio.Text.Editor;

namespace PowerMode
{
    public interface IPowerModeSession
    {
        bool ShakeEnabled { get; set; }
        bool IsEnabled { get; set; }

        bool IsLevelVisible { get; set; }

        int ExplosionAmount { get; set; }
        int ExplosionDelay { get; set; }
        int MaxShakeAmount { get; set; }
        int PowerModeIndex { get; set; }

        void RefreshGameView();
        void Configure(ICocoaTextView view);
    }
}

