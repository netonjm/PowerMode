namespace PowerMode.Extension.Mac.Adornments
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows;
    using System.Windows.Media.Animation;
    using AppKit;
    using Microsoft.VisualStudio.Text.Editor;
    using PowerMode.Extension.Mac.Services;
    using PowerMode.Extension.Mac.Settings;
    using PowerMode.Extension.Mac.Utils;

    public class ParticlesAdornment : IAdornment
    {
        private readonly static double PARTICLES_START_ALPHA = 1.0;
        private readonly double iterations;
        private readonly TimeSpan timeSpan;
        private readonly List<NSImageView> particlesList;
        private ParticlesSettings settings;

        public ParticlesAdornment()
        {
            iterations = PARTICLES_START_ALPHA / Constants.ALPHA_REMOVE_AMOUNT;
            timeSpan = TimeSpan.FromMilliseconds(Constants.FRAME_DELAY_MILLISECOND * iterations);

            particlesList = new List<NSImageView>();
        }

        public void Cleanup(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view)
        {
            particlesList.ForEach(image => { adornmentLayer.RemoveAdornment(image); });
            particlesList.Clear();
        }

        public void OnSizeChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount, bool backgroundColorChanged = false)
        {
            particlesList.ForEach(image => { adornmentLayer.RemoveAdornment(image); });
            particlesList.Clear();
        }

        public void OnTextBufferChanged(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, int streakCount)
        {
            settings = SettingsService.GetParticlesSettings();
            var isPartyMode = settings.IsEnabledPartyMode && settings.PartyModeThreshold <= streakCount;
            isPartyMode = true;
            var spawnedSize = isPartyMode
                                ? settings.PartyModeSpawnedParticles
                                : RandomUtils.Random.Next(settings.MinSpawnedParticles, settings.MaxSpawnedParticles);
            spawnedSize = Math.Min(spawnedSize, settings.MaxParticlesCount - particlesList.Count);

            for (int i = 0; i < spawnedSize; i++)
            {
                NewParticleImage(adornmentLayer, view, isPartyMode);
            }
        }


        private void NewParticleImage(IXPlatAdornmentLayer adornmentLayer, ICocoaTextView view, bool isPartyMode)
        {
            //try
            //{
            //    var particles = new Image();
            //    particles.UpdateSource(GetParticleImage());
            //    adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, particles, null);
            //    particlesList.Add(particles);

            //    try
            //    {
            //        var top = isPartyMode
            //            ? RandomUtils.Random.Next((int)view.ViewportTop, (int)view.ViewportBottom)
            //            : view.Caret.Top;
            //        var left = isPartyMode
            //            ? RandomUtils.Random.Next((int)view.ViewportLeft, (int)view.ViewportRight)
            //            : view.Caret.Left;
            //        particles.BeginAnimation(Canvas.TopProperty, GetParticleTopAnimation(top));
            //        particles.BeginAnimation(Canvas.LeftProperty, GetParticleLeftAnimation(left));
            //        var opacityAnimation = GetParticleOpacityAnimation();
            //        opacityAnimation.Completed += (sender, e) =>
            //        {
            //            particles.Visibility = Visibility.Hidden;
            //            adornmentLayer.RemoveAdornment(particles);
            //            particlesList.Remove(particles);
            //        };
            //        particles.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            //    }
            //    catch
            //    {
            //        adornmentLayer.RemoveAdornment(particles);
            //        particlesList.Remove(particles);
            //    }
            //}
            //catch
            //{
            //}

            try
            {
                var particles = new NSImageView() { Frame = new CoreGraphics.CGRect(view.Caret.Left, view.Caret.Top,5,5) };
                particles.WantsLayer = true;
                particles.Layer.BackgroundColor = NSColor.Green.CGColor;
                //particles.Image = null;
                //particles.UpdateSource(GetParticleImage());
                adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, particles, null);
                particlesList.Add(particles);

                try
                {
                    var top = isPartyMode
                        ? RandomUtils.Random.Next((int)view.ViewportTop, (int)view.ViewportBottom)
                        : view.Caret.Top;
                    var left = isPartyMode
                        ? RandomUtils.Random.Next((int)view.ViewportLeft, (int)view.ViewportRight)
                        : view.Caret.Left;

                    NSAnimationContext.RunAnimation((context) =>
                    {
                        context.Duration = 1;
                        context.AllowsImplicitAnimation = true;
                        
                        var frame = particles.Frame;
                        frame.Y = (float)top;
                        frame.X = (float)left;
                        ((NSView)particles.Animator).Frame = frame;
                    },() =>
                    {
                        particles.Hidden = true;
                        adornmentLayer.RemoveAdornment(particles);
                        particlesList.Remove(particles);
                    });

                    //particles.BeginAnimation(Canvas.TopProperty, GetParticleTopAnimation(top));
                    //particles.BeginAnimation(Canvas.LeftProperty, GetParticleLeftAnimation(left));

                    //var opacityAnimation = GetParticleOpacityAnimation();
                    //opacityAnimation.Completed += (sender, e) =>
                    //{
                    //    particles.Visibility = Visibility.Hidden;
                    //    adornmentLayer.RemoveAdornment(particles);
                    //    particlesList.Remove(particles);
                    //};
                    //particles.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
                }
                catch
                {
                    adornmentLayer.RemoveAdornment(particles);
                    particlesList.Remove(particles);
                }
            }
            catch
            {
            }


        }

        //private Bitmap GetParticleImage()
        //{
        //    var color = settings.ParticlesColorType == ParticlesColorType.Random
        //        ? RandomUtils.NextColor()
        //        : settings.FixedColor;
        //    var size = RandomUtils.Random.Next(settings.MinParticlesSize, settings.MaxParticlesSize) * 2;

        //    var bitmap = new Bitmap(size, size);
        //    bitmap.MakeTransparent();

        //    using (var graphics = Graphics.FromImage(bitmap))
        //    using (var brush = new SolidBrush(color))
        //    {
        //        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        graphics.FillEllipse(brush, 0, 0, size - 1, size - 1);

        //        graphics.Flush();
        //        return bitmap;
        //    }
        //}

        //private DoubleAnimation GetParticleTopAnimation(double top)
        //{
        //    return new DoubleAnimation()
        //    {
        //        EasingFunction = new BackEase { Amplitude = RandomUtils.Random.NextDouble() * 3 + 0.5, EasingMode = EasingMode.EaseIn },
        //        From = top,
        //        To = top + RandomUtils.Random.Next(1, 30),
        //        Duration = timeSpan
        //    };
        //}
        //private DoubleAnimation GetParticleLeftAnimation(double left)
        //{
        //    var leftDelta = RandomUtils.Random.NextDouble() * 40 * RandomUtils.NextSignal();

        //    return new DoubleAnimation()
        //    {
        //        From = left,
        //        To = left - leftDelta,
        //        Duration = timeSpan
        //    };
        //}

        //private DoubleAnimation GetParticleOpacityAnimation()
        //{
        //    return new DoubleAnimation()
        //    {
        //        From = PARTICLES_START_ALPHA,
        //        To = 0,
        //        Duration = timeSpan
        //    };
        //}
    }
}
