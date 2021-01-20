using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    // DBr: currently this guy is mutated during the execution of a move
    //      this essentially provides a cache for the life time of the scope it lives in
    //      this scope is expected to be the life time of a move (so will be utilised by 0-n action steps asscociated with that move
    //      ideally we would find a better way than mutation due to the risks
    //      for now we are living with the fact that the move state should only be mutated at the following points;
    //      FocusCardMoveResolvers : PrimeMoveState() : initialise the move state from current game state
    //      StepActions : ProcessActionResponse() : process user supplied data to move state and any knockon effects

    public interface IBotMoveStateCache
    {
        FocusBarModel ActiveFocusBarForMove { get; set; }
        Dictionary<FocusType, int> TradeTokensAvailable { get; set; }

        //int CultureTokensAvailable { get; set; }
        int CultureTokensUsedThisTurn { get; set; }

        //int ScienceTokensAvailable { get; set; }

        int BaseCityControlTokensToBePlaced { get; set; }
        int BaseTerritoryControlTokensToBePlaced { get; set; }

        int CityControlTokensPlaced { get; set; }
        int TerritroyControlTokensPlaced { get; set; }

        int NaturalWonderTokensControlled { get; set; }
        int NaturalResourceTokensControlled { get; set; }

        int StartingTechnologyLevel { get; set; }
        int BaseTechnologyIncrease { get; set; }
        int TechnologyLevelIncrease { get; set; }

        FocusType SmallestTradeTokenPileType { get; set; }

        int BaseCaravanMoves { get; set; }
        CaravanDestinationType CaravanDestinationType { get; set; }
        string CaravanCityStateDestination { get; set; }
        string CaravanRivalCityColorDestination { get; set; }
    }
}
