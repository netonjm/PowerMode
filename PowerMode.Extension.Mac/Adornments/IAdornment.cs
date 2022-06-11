using System;
using Microsoft.VisualStudio.Text.Editor;

namespace PowerMode.Extension.Mac
{
    public interface IAdornment
    {
        void OnSizeChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount, bool backgroundColorChanged = false);

        void OnTextBufferChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount);

        void Cleanup(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view);
    }
}

