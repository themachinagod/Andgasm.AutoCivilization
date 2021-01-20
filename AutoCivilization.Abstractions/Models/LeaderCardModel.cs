namespace AutoCivilization.Abstractions
{
    public enum CaravanDestinationType
    {
        OnRoute,
        CityState,
        RivalCity
    }

    public class LeaderCardModel
    {
        public string Name { get; }
        public string Nation { get; }

        public LeaderCardModel(string name, string nation)
        {
            Name = name;
            Nation = nation;
        }
    }
}
