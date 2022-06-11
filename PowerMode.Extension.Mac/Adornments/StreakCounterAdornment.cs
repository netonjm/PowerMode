namespace PowerMode.Extension.Mac.Adornments
{
    using System.Windows;
    using System.Windows.Media;
    using AppKit;
    using Microsoft.VisualStudio.Text.Editor;
    using PowerMode.Extension.Mac.Services;

    class CustomView : NSView
    {
        public CustomView()
        {
            WantsLayer = true;
            Layer.BackgroundColor = NSColor.Green.CGColor;

            WidthAnchor.ConstraintEqualTo(50).Active = true;
            HeightAnchor.ConstraintEqualTo(50).Active = true;
        }
    }

    public partial class StreakCounterAdornment : IAdornment
    {
        private const int ADORNMENT_WIDTH = 100;
        private const int ADORNMENT_TITLE_HEIGHT = 15;
        private const int ADORNMENT_STREAK_COUNTER_HEIGHT = 65;
        private const int ADORNMENT_EXCLAMATION_HEIGHT = 30;
        private const int TopMargin = 30;
        private const int RightMargin = 30;
        private CustomView titleImage;
        private CustomView streakCounterImage;
        private Exclamation exclamationImage;
        private int newMaxComboStreak = 0;
        private bool newMaxComboStreakReached = false;


        public void Cleanup(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view)
        {
            if (titleImage != null)
            {
                adornmentLayer.RemoveAdornment(titleImage);
                titleImage = null;
            }
            if (streakCounterImage != null)
            {
                adornmentLayer.RemoveAdornment(streakCounterImage);
                streakCounterImage = null;
            }
            if (exclamationImage != null)
            {
                adornmentLayer.RemoveAdornment(exclamationImage);
                exclamationImage = null;
            }
            newMaxComboStreakReached = false;
        }

        public void OnSizeChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount, bool backgroundColorChanged = false)
        {
            if (titleImage == null || backgroundColorChanged)
            {
                titleImage = new CustomView();
                //titleImage.Image = GetTitleImage(IsDarkMode(view.Background));
            }

            //adornmentLayer.

            //adornmentLayer.RefreshImage(titleImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH, view.ViewportTop + TopMargin);

            if (streakCounterImage == null) { streakCounterImage = new CustomView(); }
            //streakCounterImage.Image = GetStreakCounterImage(streakCount).Item1;
            //adornmentLayer.RefreshImage(streakCounterImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH, view.ViewportTop + TopMargin + ADORNMENT_TITLE_HEIGHT);

            //if (exclamationImage != null) { adornmentLayer.RemoveAdornment(exclamationImage); }
        }

        public void OnTextBufferChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount)
        {
            if (titleImage == null)
            {
                titleImage = new CustomView();
                //adornmentLayer.AddAdornment(AdornmentPositioningBehavior.TextRelative, null, "tag", titleImage, (s, d) => { });

                //titleImage.Image = GetTitleImage(IsDarkMode(view.Background));
                //adornmentLayer.RefreshImage(titleImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH, view.ViewportTop + TopMargin);
            }

            var comboNumberImageTuple = GetStreakCounterImage(streakCount);
            if (streakCounterImage == null) {
                streakCounterImage = new CustomView();

            }

            //streakCounterImage.UpdateSource(comboNumberImageTuple.Item1);

            //adornmentLayer.RefreshImage(streakCounterImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH, view.ViewportTop + TopMargin + ADORNMENT_TITLE_HEIGHT);

            //ScaleTransform trans = new ScaleTransform();
            //streakCounterImage.RenderTransformOrigin = new Point((ADORNMENT_WIDTH - comboNumberImageTuple.Item2.Width / 2) / ADORNMENT_WIDTH, (comboNumberImageTuple.Item2.Height / 2) / comboNumberImageTuple.Item2.Height);
            //streakCounterImage.RenderTransform = trans;

            //trans.BeginAnimation(ScaleTransform.ScaleXProperty, GetStreakCounterImageSizeAnimation(streakCount));
            //trans.BeginAnimation(ScaleTransform.ScaleYProperty, GetStreakCounterImageSizeAnimation(streakCount));


            if (ComboService.ShowExclamation(streakCount))
            {
                ShowExclamation(adornmentLayer, view, GetExclamationImage(streakCount), comboNumberImageTuple.Item2.Height);
            }

            var achievevments = AchievementsService.GetAchievements();
            if (streakCount > 0 && achievevments.MaxComboStreak < streakCount && !newMaxComboStreakReached)
            {
                ShowExclamation(adornmentLayer, view, GetNewMaxExclamationImage(streakCount), comboNumberImageTuple.Item2.Height);
                newMaxComboStreakReached = true;
            }

            if (streakCount == 0 && newMaxComboStreak > 0)
            {
                achievevments.MaxComboStreak = newMaxComboStreak;
                AchievementsService.SaveToStorage(achievevments);
                newMaxComboStreakReached = false;
            }
            newMaxComboStreak = streakCount;
        }

        class Exclamation : NSImageView
        {
            public Exclamation()
            {
                TranslatesAutoresizingMaskIntoConstraints = true;
                WantsLayer = true;
                Layer.BackgroundColor = NSColor.Green.CGColor;
                
            }
        }

        private void ShowExclamation(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, NSImage exclamationBitmap, float streakCounterHeight)
        {
            if (exclamationImage == null) { exclamationImage = new Exclamation(); }
            exclamationImage.Image = exclamationBitmap;

            //exclamationImage.BeginAnimation(Canvas.TopProperty, null);
            //exclamationImage.BeginAnimation(UIElement.OpacityProperty, null);
            //exclamationImage.Visibility = Visibility.Visible;

            double exclamationImageTop = view.ViewportTop + TopMargin + ADORNMENT_TITLE_HEIGHT + streakCounterHeight + 5;

            //adornmentLayer.RefreshImage(exclamationImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH, exclamationImageTop);

            //exclamationImage.BeginAnimation(Canvas.TopProperty, GetExclamationTopAnimation(exclamationImageTop));
            //var opacityAnimation = GetExclamationOpacityAnimation();
            //opacityAnimation.Completed += (sender, e) => exclamationImage.Visibility = Visibility.Hidden;
            //exclamationImage.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }

        private bool IsDarkMode(CoreGraphics.CGColor background)
        {
            //if (background is SolidColorBrush)
            //{
            //    var c = ((SolidColorBrush)background).Color.ToDrawingColor();
            //    var i = ((int)c.R + (int)c.G + (int)c.B) / 3;
            //    return i <= 128;
            //}

            return true;
        }
    }
}
