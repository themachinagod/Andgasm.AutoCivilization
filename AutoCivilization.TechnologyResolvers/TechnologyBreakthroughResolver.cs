using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.TechnologyResolvers
{
    public class TechnologyBreakthroughResolver : ITechnologyBreakthroughResolver
    {
        private readonly IBotGameStateService _botGameStateService;

        public TechnologyBreakthroughResolver(IBotGameStateService botGameStateService)
        {
            _botGameStateService = botGameStateService;
        }

        public (bool HasBreakThrough, FocusType FocusType, FocusLevel FocusLevel) ResolveTechnologyBreakthrough(int techLevelIncrement)
        {
            throw new System.NotImplementedException();
        }
    }
}
