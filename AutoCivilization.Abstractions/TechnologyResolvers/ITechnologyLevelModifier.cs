namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface ITechnologyLevelModifier
    {
        void IncrementTechnologyLevel(int techPoints);
    }
}
