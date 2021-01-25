using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    public class BotMoveStateCache
    {
        public FocusBarModel ActiveFocusBarForMove { get; set; }
        public Dictionary<FocusType, int> TradeTokensAvailable { get; set; } = new Dictionary<FocusType, int>();
        public Dictionary<int, TradeCaravanMoveState> TradeCaravansAvailable { get; set; } = new Dictionary<int, TradeCaravanMoveState>();
        public List<string> ControlledNaturalWonders { get; set; }

        public FocusType SmallestTradeTokenPileType { get; set; }

        public int BaseCityControlTokensToBePlaced { get; set; }
        public int BaseTerritoryControlTokensToBePlaced { get; set; }

        public int CityControlTokensPlacedThisTurn { get; set; }
        public int TerritroyControlTokensPlacedThisTurn { get; set; }

        public int NaturalWonderTokensControlledThisTurn { get; set; }
        public int NaturalResourceTokensControlledThisTurn { get; set; }

        public int CultureTokensUsedThisTurn { get; set; }

        public int BaseTechnologyIncrease { get; set; }
        public int TechnologyLevelIncrease { get; set; }
        public int StartingTechnologyLevel { get; set; }

        public int BaseCaravanMoves { get; set; }
        public int SupportedCaravanCount { get; set; }
        public int CurrentCaravanIdToMove { get; set; }
        public bool CanMoveOnWater { get; set; }
    }

    public class TradeCaravanMoveState
    {
        public int EconomyTokensUsedThisTurn { get; set; }
        public int CaravanSpacesMoved { get; set; }
        public string CaravanRivalCityColorDestination { get; set; }
        public CaravanDestinationType CaravanDestinationType { get; set; }
        public CityStateModel CaravanCityStateDestination { get; set; }
        public FocusType SmallestTradeTokenPileType { get; set; }
    }
}
