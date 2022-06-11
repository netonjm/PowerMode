using System;
using System.Collections.Generic;
using System.Drawing;

namespace PowerMode.Utils
{
    public class RandomUtils
    {
        public static Random Random { get; } = new Random(DateTime.Now.Millisecond);
        public static string NextString(IList<string> stringList)
        {
            return stringList[Random.Next(0, stringList.Count)];
        }

        public static int GetRandomIndex(int to)
        {
            var index = Random.Next(0, to);
            return index;
        }

        public static int NextSignal()
        {
            return Random.Next(0, 2) == 1 ? 1 : -1;
        }

        public static Color NextColor()
        {
            var bytes = new byte[3];
            Random.NextBytes(bytes);

            return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
        }

        public static int NextSignSwap()
        {
            throw new NotImplementedException();
        }
    }
}
