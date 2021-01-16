namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface ITechnologyBreakthroughResolver
    {
        (bool HasBreakThrough, FocusType FocusType, FocusLevel FocusLevel) ResolveTechnologyBreakthrough(int techLevelIncrement);
    }

}
