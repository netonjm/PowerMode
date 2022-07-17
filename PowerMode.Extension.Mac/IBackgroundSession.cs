using Microsoft.VisualStudio.Text.Editor;

namespace PowerMode
{
    public interface IBackgroundSession
    {
        bool IsEnabled { get; set; }
        float Opacity { get; set; }
        float Duration { get; set; }
        string FileName { get; set; }
        void Configure(ICocoaTextView view);
        void RefreshBackgroundView();
        void SetAnimation(string fileName, float duration, float explosionSize);
    }
}

