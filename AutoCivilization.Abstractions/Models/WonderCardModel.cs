namespace AutoCivilization.Abstractions
{
    public enum EraType
    { 
        Ancient,
        Medievil,
        Modern
    }

    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised at construction and should NEVER change its internal state

    public class WonderCardModel
    {
        public int Id { get; set; }
        public FocusType Type { get; }
        public EraType Era { get; }
        public string Name { get; }
        public int Cost { get; }

        public WonderCardModel(int id, FocusType type, EraType era, string name, int cost)
        {
            Id = id;
            Type = type;
            Era = era;
            Name = name;
            Cost = cost;
        }
    }
}
