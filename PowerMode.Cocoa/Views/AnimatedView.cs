using AppKit;
using CoreGraphics;
using Foundation;
using PowerMode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerMode.Cocoa.Views
{
    public class AnimatedView : NSView
    {
        const string base64Init = "data:image/gif;base64,";

        List<NSImage> frames = new List<NSImage>();

        PowerModeItem lastItem;

        public float RepeatCount { get; set; } = 1;

        /// <summary>
        /// In seconds
        /// </summary>
        public float Duration { get; set; } = 1;

        public override NSView HitTest(CGPoint aPoint)
        {
            return null;
        }

        readonly string identifier;
        public AnimatedView(string identifier)
        {
            this.identifier = identifier;
            WantsLayer = true;
        }

        public void Process(PowerModeItem powerModeItem)
        {
            if (powerModeItem == null)
                return;

            lastItem = powerModeItem;

            // var data = new NSData("", NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
            if (powerModeItem is GifUrlPowerModeItem url)
            {
                ProcessUrl(url.Urls[0]);
            }
            else if (powerModeItem is GifDataPowerModeItem gifdata)
            {
                var index = RandomUtils.GetRandomIndex(gifdata.GifData.Length - 1);
                ProcessGif(gifdata.GifData[index]);
            }
            else
            {
                throw new Exception();
            }
            Start();
        }

        void ProcessGif(string gifdata)
        {
            if (gifdata.StartsWith(base64Init))
                gifdata = gifdata.Substring(base64Init.Length);
            frames.Clear();
            var data = new NSData(gifdata, NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
            ProcessImageSource(data);
        }

        void ProcessUrl(NSUrl url)
        {
            frames.Clear();
            NSData data = NSData.FromUrl(url);
            ProcessImageSource(data);
        }

        void ProcessImageSource(NSData data)
        {
            var imageSource = ImageIO.CGImageSource.FromData(data);
            var frame = Frame;
            for (int i = 0; i < imageSource.ImageCount; i++)
            {
                var cgImage = imageSource.CreateImage(i, new ImageIO.CGImageOptions());
                var image = new NSImage(cgImage, new CGSize(frame.Width, frame.Height));
                frames.Add(image);
            }
        }

        void Stop()
        {
            if (Layer.AnimationKeys != null && Layer.AnimationKeys.Contains(identifier))
            {
                Layer.RemoveAnimation(identifier);
            }
        }

        public void Start()
        {
            Stop();

            CoreAnimation.CAKeyFrameAnimation animation = CoreAnimation.CAKeyFrameAnimation.FromKeyPath("contents");
            animation.CalculationMode = CoreAnimation.CAKeyFrameAnimation.AnimationDiscrete;
            animation.Duration = Duration;
            animation.RemovedOnCompletion = false;
            animation.RepeatCount = RepeatCount;

            //if (lastItem.GifMode == GifMode.Continue)
            //{
            //    animation.RepeatCount = 300;
            //}
            //else
            //{
            //    animation.RepeatCount = 1;
            //}

            animation.BeginTime = 0;
            animation.Values = frames.ToArray();
            Layer.AddAnimation(animation, identifier);
        }
    }
}

