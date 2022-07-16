using Microsoft.VisualStudio.Text.Editor;

namespace PowerMode
{
    public interface IDocumentPowerSession
    {
        bool ShakeEnabled { get; set; }
        bool IsEnabled { get; set; }
        bool IsCounterEnabled { get; set; }

        int ExplosionAmount { get; set; }
        int ExplosionDelay { get; set; }
        int MaxShakeAmount { get; set; }
        int PowerModeIndex { get; set; }

        void RefreshGameView();
        void SetTextView(ICocoaTextView view);
    }
}

