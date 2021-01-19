using AutoCivilization.Abstractions.Models;
using System.Collections.Generic;

namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface IFocusBarTechnologyUpgradeResolver
    {
        FocusBarUpgradeResponse RegenerateFocusBarForLowestTechnologyLevelUpgrade(FocusBarModel activeFocusBar, FocusLevel levelBarrierHit);
    }

    public class FocusBarUpgradeResponse
    {
        public FocusBarModel UpgradedFocusBar { get; }
        public FocusCardModel OldTechnology { get; }
        public FocusCardModel NewTechnology { get; }

        public FocusBarUpgradeResponse(FocusBarModel focusBarModel, FocusCardModel oldTech, FocusCardModel newTech)
        {
            UpgradedFocusBar = focusBarModel;
            OldTechnology = oldTech;
            NewTechnology = newTech;
        }
    }
}
