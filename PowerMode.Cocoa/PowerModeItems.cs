using CoreGraphics;

namespace PowerMode
{
    public enum GifMode
    {
        Restart,
        Continue
    }

    public static class Configurations
    {
        public static PowerModeItem[] Data => data;

        static PowerModeItem[] data = new PowerModeItem[]
        {
          new GifDataPowerModeItem()
          {
              Description = "Clippy",
              GifData = new string[] {
                  Resources.Clippy
              },
              Offset = new CGPoint(-37,-66),
              ExplosionSize = 10,
              ExplosionDuration = 1000,
              MaxExplosions=1,
              GifMode = GifMode.Continue
          },

          new GifDataPowerModeItem()
          {
              Description = "Fireworks",
                GifData = new string[] {
                  Resources.Sparkles,
                   Resources.ThreeColorfulFireworks,
                    Resources.ThreeColorfulFireworks2
              },
                Offset = new CGPoint(-37,-66),
              ExplosionSize = 10,
              ExplosionDuration = 1500,
              ExplosionOrder = ExplosionOrder.Random,
              ShakeIntensity = 1,
              MaxExplosions = 3,
              GifMode = GifMode.Restart
          },

          new GifDataPowerModeItem()
          {
              Description = "Flames",
              GifData = new string[]
               {
                   Resources.Flame
               },
              Offset = new CGPoint(-44,-80),
              ExplosionSize = 13,
              ExplosionDuration = 500,
              ExplosionOrder = ExplosionOrder.Random,
              GifMode = GifMode.Continue
          },

          new GifDataPowerModeItem()
          {
              Description = "Magic",
                GifData = new string[] {
                  Resources.Magic
              },
              Offset = new CGPoint(-54,-75),
              ExplosionSize = 20,
              ExplosionDuration = 415,
              ExplosionOrder = ExplosionOrder.Random,
              GifMode = GifMode.Continue
          },

          new GifDataPowerModeItem()
          {
              Description = "Particles",
                GifData = new string[] {
                  Resources.AtomExplosion1,
                  Resources.AtomExplosion2,
                  Resources.AtomExplosion3,
                  Resources.AtomExplosion4,
                  Resources.AtomExplosion5,
                  Resources.AtomExplosion6,
                  Resources.AtomExplosion7,
                  Resources.AtomExplosion8,
                  Resources.AtomExplosion9,
                  Resources.AtomExplosion10,
              },
              Offset = new CGPoint(-36,-46),
              ExplosionSize = 13,
              ExplosionDuration = 400,
              MaxExplosions = 3,
              ExplosionOrder = ExplosionOrder.Random,
              GifMode = GifMode.Continue
          },

          new GifDataPowerModeItem()
          {
              Description = "Simple Rift",
                GifData = new string[] {
                  Resources.VerticalRift
              },
              Offset = new CGPoint(-34,-40),
              ExplosionSize = 14,
              ExplosionDuration = 1500,
              ExplosionOrder = ExplosionOrder.Random,
              GifMode = GifMode.Continue
          },

          new GifDataPowerModeItem()
          {
              Description = "Exploding Rift",
              GifData = new string[]
               {
                   Resources.HorizontalRift,
                   Resources.Space1,
                   Resources.Space2
                },
              Offset = new CGPoint(-34,-40),
              ExplosionSize = 14,
              ExplosionDuration = 500,
              ShakeIntensity = 3,
              ExplosionOrder = ExplosionOrder.Random,
              GifMode = GifMode.Continue
          },
        };
    }
}
