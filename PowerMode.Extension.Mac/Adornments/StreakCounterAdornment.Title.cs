namespace PowerMode.Extension.Mac.Adornments
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using AppKit;

    public partial class StreakCounterAdornment
    {
        private const string COMBO_TITLE = "combo";

        private NSImage GetTitleImage(bool inDarkMode)
        {
            return new NSImage();
            //var bitmap = new Bitmap(ADORNMENT_WIDTH, ADORNMENT_TITLE_HEIGHT);
            //bitmap.MakeTransparent();

            //using (var graphics = Graphics.FromImage(bitmap))
            //{
            //    graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //    var font = new Font("Tahoma", 8);
            //    var size = graphics.MeasureString(COMBO_TITLE, font);
            //    graphics.DrawString(COMBO_TITLE, font, inDarkMode ? Brushes.White : Brushes.Black, new RectangleF(ADORNMENT_WIDTH - size.Width, 0, size.Width, ADORNMENT_TITLE_HEIGHT));

            //    graphics.Flush();
            //    return bitmap;
            //}
        }
    }
}
