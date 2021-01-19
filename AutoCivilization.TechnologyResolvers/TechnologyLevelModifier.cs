using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.Models;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;

namespace AutoCivilization.TechnologyResolvers
{
    public class TechnologyUpgradeResolver : ITechnologyUpgradeResolver
    {
        private readonly ITechnologyBreakthroughResolver _technologyBreakthroughResolver;
        private readonly IFocusBarTechnologyUpgradeResolver _focusBarTechnologyUpgradeResolver;

        public TechnologyUpgradeResolver(ITechnologyBreakthroughResolver technologyBreakthroughResolver,
                                         IFocusBarTechnologyUpgradeResolver focusBarTechnologyUpgradeResolver)
        {
            _focusBarTechnologyUpgradeResolver = focusBarTechnologyUpgradeResolver;
            _technologyBreakthroughResolver = technologyBreakthroughResolver;
        }

        public TechnologyUpgradeResponse ResolveTechnologyLevelUpdates(int currentTechLevel, int techLevelIncrement, FocusBarModel activeFocusBar)
        {
            var encounteredBreakthroughs = new List<BreakthroughModel>();
            var breakThroughResponse = _technologyBreakthroughResolver.ResolveTechnologyBreakthrough(currentTechLevel, techLevelIncrement);
            if (breakThroughResponse != null)
            {
                foreach (var breakthroughLevel in breakThroughResponse)
                {
                    var techUpgradeResponse = _focusBarTechnologyUpgradeResolver.RegenerateFocusBarForLowestTechnologyLevelUpgrade(activeFocusBar, breakthroughLevel);
                    activeFocusBar = techUpgradeResponse.UpgradedFocusBar;
                    encounteredBreakthroughs.Add(new BreakthroughModel() { ReplacedFocusCard = techUpgradeResponse.OldTechnology, UpgradedFocusCard = techUpgradeResponse.NewTechnology });
                }
            }
            var newTechLevelPoints = currentTechLevel + techLevelIncrement;
            return new TechnologyUpgradeResponse(newTechLevelPoints, activeFocusBar, encounteredBreakthroughs);
        }
    }
}
