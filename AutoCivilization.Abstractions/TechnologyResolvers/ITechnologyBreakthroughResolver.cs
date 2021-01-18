using System.Collections.Generic;

namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface ITechnologyBreakthroughResolver
    {
        IReadOnlyCollection<FocusLevel> ResolveTechnologyBreakthrough(int techLevelIncrement);
    }

}
