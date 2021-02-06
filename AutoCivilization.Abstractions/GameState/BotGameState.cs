using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Abstractions
{
    // DBr: currently this guy is mutated at the end of each move
    //      this essentially provides a cache for the life time of the game it is initialised in
    //      for now we are living with the fact that the game state should only be mutated at the following points;
    //      FocusCardMoveResolvers : UpdateGameStateForMove() : update the game state from the move state

    public class BotGameState
    {
        public BotGameState(FocusBarModel focusBarModel, LeaderCardModel leaderCardModel, WonderCardDecksModel wonderCardDecksModel)
        {
            GameId = Guid.NewGuid();
            ActiveFocusBar = focusBarModel;
            ChosenLeaderCard = leaderCardModel;
            WonderCardDecks = wonderCardDecksModel;
            SupportedCaravanCount = 1;
            FriendlyCityCount = 1;

            TradeTokens = new Dictionary<FocusType, int>();
            TradeTokens.Add(FocusType.Culture, 0);
            TradeTokens.Add(FocusType.Economy, 0);
            TradeTokens.Add(FocusType.Science, 0);
            TradeTokens.Add(FocusType.Industry, 0);
            TradeTokens.Add(FocusType.Military, 0);

            PurchasedWonders = new List<WonderCardModel>();
            CityStateDiplomacyCardsHeld = new List<CityStateModel>();
            VisitedPlayerColors = new List<string>();
            ControlledNaturalWonders = new List<string>();
            ConqueredCityStateTokensHeld = new List<CityStateModel>();
        }

        public Guid GameId { get; set; }
        public int CurrentRoundNumber { get; set; }

        public LeaderCardModel ChosenLeaderCard { get; set; }
        public FocusBarModel ActiveFocusBar { get; set; }
        public WonderCardDecksModel WonderCardDecks { get; set; }
        public Dictionary<FocusType, int> TradeTokens { get; set; }
        
        public List<CityStateModel> CityStateDiplomacyCardsHeld { get; set; }
        public string CityStateDiplomacyCardsHeldString { get { return CityStateDiplomacyCardsHeld.Count > 0 ? string.Join(", ", CityStateDiplomacyCardsHeld.Select(x => $"{x.Name} ({x.Type})")) : "None"; } }
        public List<CityStateModel> ConqueredCityStateTokensHeld { get; set; }
        public string ConqueredCityStateTokensHeldString { get { return ConqueredCityStateTokensHeld.Count > 0 ? string.Join(", ", ConqueredCityStateTokensHeld.Select(x => $"{x.Name} ({x.Type})")) : "None"; } }
        public List<string> VisitedPlayerColors { get; set; }
        public string VisitedPlayerColorsString { get { return VisitedPlayerColors.Count > 0 ? string.Join(", ", VisitedPlayerColors) : "None"; } } // TODO: this will be insufficient for player diplomacy cards!!!
        public List<WonderCardModel> PurchasedWonders { get; set; }
        public string PurchasedWondersString { get { return PurchasedWonders.Count > 0 ? string.Join(", ", PurchasedWonders.Select(x => $"{x.Name} ({x.Type})")) : "None"; } }
        public List<string> ControlledNaturalWonders { get; set; } // TODO: perhaps we get more by storing the wonder model instead of just string!!
        public string ControlledNaturalWondersString { get { return ControlledNaturalWonders.Count > 0 ? string.Join(", ", ControlledNaturalWonders) : "None"; } }

        public int FriendlyCityCount { get; set; }
        public int ControlledSpaces { get; set; }
        public int ControlledNaturalResources { get; set; }

        public int TechnologyLevel { get; set; }

        public int SupportedCaravanCount { get; set; }
        public int CaravansOnRouteCount { get; set; }
    }
}
