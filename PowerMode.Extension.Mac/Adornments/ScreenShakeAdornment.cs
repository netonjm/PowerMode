namespace PowerMode.Extension.Mac.Adornments
{
    using System;
    using Task = System.Threading.Tasks.Task;

    using Microsoft.VisualStudio.Text.Editor;

    using PowerMode.Extension.Mac.Services;
    using PowerMode.Extension.Mac.Utils;

    public class ScreenShakeAdornment : IAdornment
    {
        private readonly static int SHAKE_TIMEOUT_MILLISECONDS = 75;
        private readonly static int SHAKE_THROTTLED_MILLISECONDS = 50;
        private DateTime lastShakeTime = DateTime.Now;


        public void Cleanup(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view)
        {
            lastShakeTime = DateTime.Now;
        }

        public void OnSizeChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount, bool backgroundColorChanged = false)
        {
            lastShakeTime = DateTime.Now;
        }

        public void OnTextBufferChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount)
        {
            if (lastShakeTime.AddMilliseconds(SHAKE_THROTTLED_MILLISECONDS) > DateTime.Now) { return; }

            Shake(view).ConfigureAwait(false);
        }

        public async Task Shake(ICocoaTextView view)
        {
            var settings = SettingsService.GetScreenShakeSettings();

            int leftAmount = GetShakeIntensity(settings.MinIntensity, settings.MaxIntensity),
                topAmount = GetShakeIntensity(settings.MinIntensity, settings.MaxIntensity);

            lastShakeTime = DateTime.Now;
            view.ViewScroller.ScrollViewportHorizontallyByPixels(leftAmount);
            view.ViewScroller.ScrollViewportVerticallyByPixels(topAmount);
            //  TODO: Should have a better way to shake the screen

            await Task.Delay(SHAKE_TIMEOUT_MILLISECONDS);
            view.ViewScroller.ScrollViewportHorizontallyByPixels(-leftAmount);
            view.ViewScroller.ScrollViewportVerticallyByPixels(-topAmount);
        }


        private int GetShakeIntensity(int min, int max)
        {
            var direction = RandomUtils.NextSignal();
            return RandomUtils.Random.Next(min, max) * direction;
        }
    }
}
