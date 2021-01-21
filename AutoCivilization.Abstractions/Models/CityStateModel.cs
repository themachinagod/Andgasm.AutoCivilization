namespace AutoCivilization.Abstractions
{
    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised at construction and should NEVER change its internal state

    public class CityStateModel
    {
        public int Id { get; set; }
        public FocusType Type { get; }
        public string Name { get; set; }

        public CityStateModel(int id, FocusType type, string name)
        {
            Id = id;
            Type = type;
            Name = name;
        }
    }
}
