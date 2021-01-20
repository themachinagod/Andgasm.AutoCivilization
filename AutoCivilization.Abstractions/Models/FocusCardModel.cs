namespace AutoCivilization.Abstractions
{
    public enum FocusType
    { 
        Culture,
        Science,
        Military,
        Industry,
        Economy
    }

    public enum FocusLevel
    {
        Lvl1,
        Lvl2,
        Lvl3,
        Lvl4
    }

    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised at construction and should NEVER change its internal state

    public class FocusCardModel
    {
        public FocusType Type { get; }
        public FocusLevel Level { get; }
        public string Name { get; }

        public FocusCardModel(FocusType type, FocusLevel level, string name)
        {
            Type = type;
            Level = level;
            Name = name;
        }
    }
}
