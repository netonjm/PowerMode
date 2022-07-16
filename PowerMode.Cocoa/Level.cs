namespace PowerMode
{
    public class Level
    {
        public event EventHandler Changed;

        static string[] descriptions = new string[]
        {
            "Beginner",
            "Good",
            "Expert",
            "Master",
            "🔥🔥God🔥🔥"
        };

        public int CurrentLevel { get; private set; } = 0;

        int count = 0;
        public int Count => count;

        int step = 0;

        public string GetCurrentDescription()
        {
            return descriptions[CurrentLevel];
        }

        int deltaInit = 40;

        int GetMaxPointsForLevel(int level)
        {
            int init = deltaInit;
            for (int i = 0; i < level; i++)
            {
                init = (init * 2) + (init / 2);
            }
            return init;
        }

        void Recalculate()
        {
            var maxpoints = GetMaxPointsForLevel(CurrentLevel);
            if (step > maxpoints)
            {
                CurrentLevel++;
                step = 0;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Step()
        {
            count++;
            step++;
            Recalculate();
        }

        public void Reset()
        {
            count = 0;
            CurrentLevel = 0;
            step = 0;
        }
    }
}

