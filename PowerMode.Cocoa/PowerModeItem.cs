using CoreGraphics;

namespace PowerMode
{
    public class GifDataPowerModeItem : PowerModeItem
    {
        public string[] GifData { get; set; }
    }

    public class GifUrlPowerModeItem : PowerModeItem
    {
        public NSUrl[] Urls { get; set; }
    }
   
    public enum ExplosionOrder
    {
        Random,
        Sequential
    }

    public abstract class PowerModeItem
    {
        public GifMode GifMode { get; set; }
        public ExplosionOrder ExplosionOrder { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// In miliseconds
        /// </summary>
        public float ExplosionDuration { get; set; }
        public float ExplosionSize { get; set; }
        public float MaxExplosions { get; set; }

        public int ShakeIntensity { get; set; }

        public CGPoint Offset { get; set; }
    }
}

