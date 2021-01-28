using AutoCivilization.Abstractions.Models;
using System.Collections.Generic;

namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface IFocusBarTechnologyUpgradeResolver
    {
        FocusBarUpgradeResponse RegenerateFocusBarLowestTechnologyLevelUpgrade(FocusBarModel activeFocusBar, FocusLevel levelBarrierHit);
        FocusBarUpgradeResponse RegenerateFocusBarLowestTechnologyLevelUpgrade(FocusBarModel activeFocusBar);
        FocusBarUpgradeResponse RegenerateFocusBarSpecificTechnologyLevelUpgrade(FocusBarModel activeFocusBar, FocusType focusType);
    }

    public class FocusBarUpgradeResponse
    {
        public FocusBarModel UpgradedFocusBar { get; }
        public FocusCardModel OldTechnology { get; }
        public FocusCardModel NewTechnology { get; }
        public bool HasUpgraded { get; }

        public FocusBarUpgradeResponse(bool hasUpgraded, FocusBarModel focusBarModel, FocusCardModel oldTech, FocusCardModel newTech)
        {
            HasUpgraded = hasUpgraded;
            UpgradedFocusBar = focusBarModel;
            OldTechnology = oldTech;
            NewTechnology = newTech;
        }
    }
}
