using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    // DBr: currently this guy is mutated during the execution of a move
    //      this data object will exist for the duration of move resolver 
    //      use of this object should be limited as below
    //      - initialise the instance object in the PrimeMoveState() method of a FocusCardMoveResolver
    //      - when resolving ProcessActionResponse() update the state instance properties
    //      doing the above keeps changes to the move state easy to track however we are not preventing the mutation of this object

    public class BotMoveState
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
