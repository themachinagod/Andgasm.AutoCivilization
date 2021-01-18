using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.TechnologyResolvers
{
    public class TechnologyLevelModifier : ITechnologyLevelModifier
    {
        private readonly IBotGameStateService _botGameStateService;
        private readonly ITechnologyBreakthroughResolver _technologyBreakthroughResolver;
        private readonly IFocusBarTechnologyUpgradeResolver _focusBarTechnologyUpgradeResolver;

        public bool EncounteredBreakthrough { get; set; }
        public FocusCardModel ReplacedFocusCard { get; set; }
        public FocusCardModel UpgradedFocusCard { get; set; }

        public TechnologyLevelModifier(IBotGameStateService botGameStateService,
                                       ITechnologyBreakthroughResolver technologyBreakthroughResolver,
                                       IFocusBarTechnologyUpgradeResolver focusBarTechnologyUpgradeResolver)
        {
            _botGameStateService = botGameStateService;
            _focusBarTechnologyUpgradeResolver = focusBarTechnologyUpgradeResolver;
            _technologyBreakthroughResolver = technologyBreakthroughResolver;
        }

        public void IncrementTechnologyLevel(int techPoints)
        {
            // TODO: need to handle max tech level...
            var breakThroughResponse = _technologyBreakthroughResolver.ResolveTechnologyBreakthrough(techPoints);
            if (breakThroughResponse.HasBreakThrough)
            {
                EncounteredBreakthrough = true;
                var techUpgradeResponse = _focusBarTechnologyUpgradeResolver.UpgradeFocusBarsLowestTechLevel(breakThroughResponse.FocusType, breakThroughResponse.FocusLevel);
                ReplacedFocusCard = techUpgradeResponse.OldTech;
                UpgradedFocusCard = techUpgradeResponse.NewTech;
            }
            _botGameStateService.TechnologyLevel += techPoints;
            _botGameStateService.ScienceTradeTokens = _botGameStateService.ScienceTradeTokens;
        }
    }
}
