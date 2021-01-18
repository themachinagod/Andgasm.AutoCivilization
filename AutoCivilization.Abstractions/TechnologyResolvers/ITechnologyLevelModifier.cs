using AutoCivilization.Abstractions.Models;
using System.Collections.Generic;

namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface ITechnologyLevelModifier
    {
        bool EncounteredBreakthrough { get; set; }
        List<BreakthroughModel> BreakthroughsEncountered { get; set; }

        void IncrementTechnologyLevel(int techPoints);
    }
}
