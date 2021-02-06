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

    public enum AttackTargetType
    {
        Barbarian,
        RivalCity,
        RivalCapitalCity,
        CityState,
        RivalControlToken
    }

    public class BotMoveState
    {
        public FocusBarModel ActiveFocusBarForMove { get; set; }
        public WonderCardDecksModel ActiveWonderCardDecks { get; set; }
        public Dictionary<FocusType, int> TradeTokensAvailable { get; set; } = new Dictionary<FocusType, int>();
        public Dictionary<int, TradeCaravanMoveState> TradeCaravansAvailable { get; set; } = new Dictionary<int, TradeCaravanMoveState>();
        public Dictionary<int, AttackTargetMoveState> AttacksAvailable { get; set; } = new Dictionary<int, AttackTargetMoveState>();
        public List<string> ControlledNaturalWonders { get; set; } = new List<string>();
        public List<WonderCardModel> BotPurchasedWonders { get; set; } = new List<WonderCardModel>();
        public List<WonderCardModel> AllPurchasedWonders { get; set; } = new List<WonderCardModel>();
        public List<CityStateModel> CityStatesDiplomacyCardsHeld { get; set; } = new List<CityStateModel>();
        public List<CityStateModel> ConqueredCityStateTokensHeld { get; set; } = new List<CityStateModel>();

        public FocusType SmallestTradeTokenPileType { get; set; }

        public int FriendlyCityCount { get; set; }
        public int FriendlyCitiesAddedThisTurn { get; set; }
        public bool HasPurchasedWonderThisTurn { get; set; }
        public bool HasPurchasedCityThisTurn { get; set; }

        public int BaseCityControlTokensToBePlaced { get; set; }
        public int BaseTerritoryControlTokensToBePlaced { get; set; }

        public int CityControlTokensPlacedThisTurn { get; set; }
        public int TerritroyControlTokensPlacedThisTurn { get; set; }
        public bool HasStolenNaturalWonder { get; set; }

        public int BaseProductionPoints { get; set; }
        public int NaturalResourcesToSpend { get; set; }
        public int NaturalResourcesSpentThisTurn { get; set; }
        public int NaturalWonderTokensControlledThisTurn { get; set; }
        public int NaturalResourceTokensControlledThisTurn { get; set; }

        public int CultureTokensUsedThisTurn { get; set; }

        public int BaseTechnologyIncrease { get; set; }
        public int StartingTechnologyLevel { get; set; }

        public int BaseCaravanMoves { get; set; }
        public int SupportedCaravanCount { get; set; }
        public int CurrentCaravanIdToMove { get; set; }
        public bool CanMoveOnWater { get; set; }
        public int IndustryTokensUsedThisTurn { get; set; }
        public WonderCardModel WonderPurchasedThisTurn { get; set; }
        public int ComputedProductionCapacityForTurn { get; set; }
        public int BaseCityDistance { get; set; }

        public int CurrentAttackMoveId { get; set; }
        public int BaseAttackRange { get; set; }
        public int BaseAttackPower { get; set; }
        public int BaseMaxTargetPower { get; set; }
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

    public class AttackTargetMoveState
    {
        public bool IsTargetWithinRange { get; set; }
        public AttackTargetType AttackTargetType { get; set; }
        public int AttackTargetPowerWithoutTradeTokens { get; set; }
        public int ComputedBotAttackPowerForTurn { get; set; }
        public int TargetSpentMilitaryTradeTokensThisTurn { get; set; }
        public int BotSpentMilitaryTradeTokensThisTurn { get; set; }
        public CityStateModel TargetCityState { get; set; }
        public bool BotIsWinning { get; set; }
        public int DiceRollAttackPower { get; set; }
        public bool HasStolenNaturalWonder { get; set; }
        public WonderCardModel ConqueredWonder { get; set; }
    }
}
