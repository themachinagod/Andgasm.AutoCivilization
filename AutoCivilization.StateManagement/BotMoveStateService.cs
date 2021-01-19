using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    public class BotMoveStateCache : IBotMoveStateCache
    {
        public FocusBarModel ActiveFocusBarForMove { get; set; }

        public int BaseCityControlTokensToBePlaced { get; set; }
        public int BaseTerritoryControlTokensToBePlaced { get; set; }
        
        public int CityControlTokensPlaced { get; set; }
        public int TerritroyControlTokensPlaced { get; set; }

        public int NaturalWonderTokensControlled { get; set; }
        public int NaturalResourceTokensControlled { get; set; }

        public int CultureTokensAvailable { get; set; }
        public int CultureTokensUsedThisTurn { get; set; }

        public int BaseTechnologyIncrease { get; set; }
        public int TechnologyLevelIncrease { get; set; }

        public int ScienceTokensAvailable { get; set; }
        public int ScienceTokensUsedThisTurn { get; set; }
        public int StartingTechnologyLevel { get; set; }
    }
}
