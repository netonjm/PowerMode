namespace PowerMode
{
    public interface ICounterProvider
    {
        int Count { get; }
        int CurrentLevel { get; }
        void Reset();
        void Step();
        string GetCurrentDescription();
    }
}

