using System;

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

    public class FocusCardModel
    {
        public FocusType Type { get; set; }
        public FocusLevel Level { get; set; }
        public string Name { get; set; }
    }
}
