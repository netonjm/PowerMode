﻿namespace PowerMode.Extension.Mac.Adornments
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Media.Animation;
    using AppKit;
    using PowerMode.Extension.Mac.Services;

    public partial class StreakCounterAdornment
    {
        private readonly static double EXCLAMATION_START_ALPHA = 0.9;

        private NSImage GetExclamationImage(int streakCount)
        {
            //var font = new Font("Tahoma", ComboService.GetPowerLevelExclamationFontSize(streakCount));
            //var color = ComboService.GetPowerLevelColor(streakCount);

            return new NSImage();

            //var bitmap = new Bitmap(ADORNMENT_WIDTH, ADORNMENT_EXCLAMATION_HEIGHT);
            //bitmap.MakeTransparent();

            //using (var graphics = Graphics.FromImage(bitmap))
            //{
            //    graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //    var exclamation = ComboService.GetExclamation();
            //    var size = graphics.MeasureString(exclamation, font);
            //    graphics.DrawString(exclamation, font, new SolidBrush(color), new RectangleF(ADORNMENT_WIDTH - size.Width, 0, size.Width, ADORNMENT_EXCLAMATION_HEIGHT));

            //    graphics.Flush();
            //    return bitmap;
            //}
        }

        private NSImage GetNewMaxExclamationImage(int streakCount)
        {
            return new NSImage();
            //var font = new Font("Tahoma", ComboService.GetPowerLevelExclamationFontSize(streakCount));
            //var color = ComboService.GetPowerLevelColor(streakCount);

            //var bitmap = new Bitmap(ADORNMENT_WIDTH, ADORNMENT_EXCLAMATION_HEIGHT);
            //bitmap.MakeTransparent();

            //using (var graphics = Graphics.FromImage(bitmap))
            //{
            //    graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //    var exclamation = "New MAX !!!";
            //    var size = graphics.MeasureString(exclamation, font);
            //    graphics.DrawString(exclamation, font, new SolidBrush(color), new RectangleF(ADORNMENT_WIDTH - size.Width, 0, size.Width, ADORNMENT_EXCLAMATION_HEIGHT));

            //    graphics.Flush();
            //    return bitmap;
            //}
        }


        //private DoubleAnimation GetExclamationTopAnimation(double top)
        //{
        //    var iterations = EXCLAMATION_START_ALPHA / Constants.ALPHA_REMOVE_AMOUNT;
        //    var timeSpan = TimeSpan.FromMilliseconds(Constants.FRAME_DELAY_MILLISECOND * iterations);

        //    return new DoubleAnimation()
        //    {
        //        From = top,
        //        To = top + 10,
        //        Duration = timeSpan
        //    };
        //}

        //private DoubleAnimation GetExclamationOpacityAnimation()
        //{
        //    var iterations = EXCLAMATION_START_ALPHA / Constants.ALPHA_REMOVE_AMOUNT;
        //    var timeSpan = TimeSpan.FromMilliseconds(Constants.FRAME_DELAY_MILLISECOND * iterations);

        //    return new DoubleAnimation()
        //    {
        //        From = EXCLAMATION_START_ALPHA,
        //        To = 0,
        //        Duration = timeSpan
        //    };
        //}
    }
}
