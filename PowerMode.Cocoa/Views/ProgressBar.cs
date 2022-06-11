using AppKit;
using CoreGraphics;

namespace PowerMode.Cocoa.Views
{
    public class ProgressBar : NSView
    {
        float progress = 100;
        NSView progressLayer;

        NSColor shadowColor = NSColor.Blue;
        public NSColor ShadowColor
        {
            get => shadowColor;
            set
            {
                if (shadowColor == value)
                    return;
                shadowColor = value;
                progressLayer.Shadow.ShadowColor = shadowColor;
            }
        }

        public float Progress
        {
            get => progress;
            set
            {
                if (progress == value)
                    return;
                progress = value;
                Refresh();
            }
        }

        public const int DefaultHeight = 10;

        public ProgressBar()
        {
            WantsLayer = true;

            progressLayer = new NSBox()
            {
                FillColor = NSColor.White,
                BoxType = NSBoxType.NSBoxCustom,
            };
            AddSubview(progressLayer);

            progressLayer.AddShadow(shadowColor, 5);
        }

        public override void SetFrameSize(CGSize newSize)
        {
            base.SetFrameSize(newSize);
            Refresh();
        }

        void Refresh()
        {
            var frame = Frame;
            var width = frame.Width * (Progress / 100);
            var progressRect = new CGRect(frame.Width - width, 0, width, frame.Height);
            progressLayer.Frame = progressRect;
        }
    }
}

