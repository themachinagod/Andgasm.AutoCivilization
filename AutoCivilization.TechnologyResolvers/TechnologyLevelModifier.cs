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
            if (breakThroughResponse != null) // TODO: do we have better than null check here to define if no breakthrough??
            {
                foreach (var breakthroughLevel in breakThroughResponse)
                {
                    var techUpgradeResponse = _focusBarTechnologyUpgradeResolver.RegenerateFocusBarLowestTechnologyLevelUpgrade(activeFocusBar, breakthroughLevel);
                    activeFocusBar = techUpgradeResponse.UpgradedFocusBar;
                    encounteredBreakthroughs.Add(new BreakthroughModel(techUpgradeResponse.OldTechnology, techUpgradeResponse.NewTechnology));
                }
            }
            var newTechLevelPoints = currentTechLevel + techLevelIncrement;
            return new TechnologyUpgradeResponse(newTechLevelPoints, activeFocusBar, encounteredBreakthroughs);
        }

        public FocusBarUpgradeResponse ResolveFreeTechnologyUpdate(FocusBarModel activeFocusBar)
        {            
            return _focusBarTechnologyUpgradeResolver.RegenerateFocusBarLowestTechnologyLevelUpgrade(activeFocusBar);  
        }
    }
}
