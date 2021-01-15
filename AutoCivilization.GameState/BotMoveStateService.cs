using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    public class BotMoveStateService : IBotMoveStateService
    {
        public int BaseCityControlTokensToBePlaced { get; set; }
        public int BaseTerritoryControlTokensToBePlaced { get; set; }
        public int CityControlTokensPlaced { get; set; }
        public int TerritroyControlTokensPlaced { get; set; }
        public int NaturalWonderTokensControlled { get; set; }
        public int NaturalResourceTokensControlled { get; set; }
        public int CultureTokensAvailable { get; set; }
    }
}
