using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;

namespace AutoCivilization.TechnologyResolvers
{
    public class TechnologyBreakthroughResolver : ITechnologyBreakthroughResolver
    {
        private Dictionary<int, FocusLevel> LevelBarrierMarkers = new Dictionary<int, FocusLevel>();

        public TechnologyBreakthroughResolver()
        {
            LevelBarrierMarkers.Add(3, FocusLevel.Lvl2);
            LevelBarrierMarkers.Add(6, FocusLevel.Lvl2);
            LevelBarrierMarkers.Add(10, FocusLevel.Lvl3);
            LevelBarrierMarkers.Add(14, FocusLevel.Lvl3);
            LevelBarrierMarkers.Add(19, FocusLevel.Lvl4);
            LevelBarrierMarkers.Add(25, FocusLevel.Lvl4);
            LevelBarrierMarkers.Add(31, FocusLevel.Lvl4);
            LevelBarrierMarkers.Add(37, FocusLevel.Lvl4);
            LevelBarrierMarkers.Add(44, FocusLevel.Lvl4);
        }

        public IReadOnlyCollection<FocusLevel> ResolveTechnologyBreakthrough(int currentTechLevel, int techLevelIncrement)
        {
            var upgradeLevelsHit = new List<FocusLevel>();
            var newTechLevel = currentTechLevel + techLevelIncrement;
            for (int i = currentTechLevel + 1; i <= newTechLevel; i++)
            {
                if (LevelBarrierMarkers.ContainsKey(i))
                {
                    upgradeLevelsHit.Add(LevelBarrierMarkers[i]);
                }
            }
            return upgradeLevelsHit;
        }
    }
}
