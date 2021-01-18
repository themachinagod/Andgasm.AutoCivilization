using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.Models;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;

namespace AutoCivilization.TechnologyResolvers
{
    public class TechnologyLevelModifier : ITechnologyLevelModifier
    {
        private readonly IBotGameStateService _botGameStateService;
        private readonly ITechnologyBreakthroughResolver _technologyBreakthroughResolver;
        private readonly IFocusBarTechnologyUpgradeResolver _focusBarTechnologyUpgradeResolver;

        public bool EncounteredBreakthrough { get; set; }
        public List<BreakthroughModel> BreakthroughsEncountered { get; set; } = new List<BreakthroughModel>();

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
            var breakThroughResponse = _technologyBreakthroughResolver.ResolveTechnologyBreakthrough(techPoints);
            if (breakThroughResponse != null)
            {
                foreach (var breakthroughLevel in breakThroughResponse)
                {
                    EncounteredBreakthrough = true;
                    var techUpgradeResponse = _focusBarTechnologyUpgradeResolver.UpgradeFocusBarsLowestTechLevel(breakthroughLevel);
                    BreakthroughsEncountered.Add(new BreakthroughModel() { ReplacedFocusCard = techUpgradeResponse.OldTech, UpgradedFocusCard = techUpgradeResponse.NewTech });
                }
            }
            _botGameStateService.TechnologyLevel += techPoints;
            _botGameStateService.ScienceTradeTokens = _botGameStateService.ScienceTradeTokens;
        }
    }
}
