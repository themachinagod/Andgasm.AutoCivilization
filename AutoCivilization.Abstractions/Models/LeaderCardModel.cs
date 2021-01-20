namespace AutoCivilization.Abstractions
{
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
