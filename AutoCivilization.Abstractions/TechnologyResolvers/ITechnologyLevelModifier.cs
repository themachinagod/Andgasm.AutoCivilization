using AutoCivilization.Abstractions.Models;
using System.Collections.Generic;

namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface ITechnologyUpgradeResolver
    {
        TechnologyUpgradeResponse ResolveTechnologyLevelUpdates(int currentTechLevel, int techLevelIncrement, FocusBarModel activeFocusBar);
        FocusBarUpgradeResponse ResolveFreeTechnologyUpdate(FocusBarModel activeFocusBar);
    }

    public class TechnologyUpgradeResponse
    {
        public int NewTechnologyLevelPoints { get; }
        public FocusBarModel UpgradedFocusBar { get; }
        public IReadOnlyCollection<BreakthroughModel> EncounteredBreakthroughs { get; }

        public TechnologyUpgradeResponse(int newTechnologyLevel, FocusBarModel focusBarModel, IReadOnlyCollection<BreakthroughModel> encounteredBreakthroughs)
        {
            NewTechnologyLevelPoints = newTechnologyLevel;
            UpgradedFocusBar = focusBarModel;
            EncounteredBreakthroughs = encounteredBreakthroughs;
        }
    }
}
