using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    // DBr: currently this guy is mutated during at the end of each move
    //      this essentially provides a cache for the life time of the game it is initialised in
    //      ideally we would find a better way than mutation due to the inherant risks
    //      for now we are living with the fact that the game state should only be mutated at the following points;
    //      FocusCardMoveResolvers : UpdateGameStateForMove() : update the game state from the move state

    public class BotGameStateCache
    {
        public BotGameStateCache(FocusBarModel focusBarModel, LeaderCardModel leaderCardModel)
        {
            ActiveFocusBar = focusBarModel;
            ChosenLeaderCard = leaderCardModel;

            TradeTokens = new Dictionary<FocusType, int>();
            TradeTokens.Add(FocusType.Culture, 0);
            TradeTokens.Add(FocusType.Economy, 0);
            TradeTokens.Add(FocusType.Science, 0);
            TradeTokens.Add(FocusType.Industry, 0);
            TradeTokens.Add(FocusType.Military, 0);

            VisitedCityStates = new List<CityStateModel>();
            VisitedPlayerColors = new List<string>();
        }

        public int GameId { get; set; } = 1001;
        public int CurrentRoundNumber { get; set; }

        public Dictionary<FocusType, int> TradeTokens { get; set; }
        public FocusBarModel ActiveFocusBar { get; set; }
        public LeaderCardModel ChosenLeaderCard { get; set; }
        public List<CityStateModel> VisitedCityStates { get; set; }
        public List<string> VisitedPlayerColors { get; set; }

        public int ControlledSpaces { get; set; }
        public int ControlledResources { get; set; }
        public int ControlledWonders { get; set; }

        public int TechnologyLevel { get; set; }
        public int SupportedCaravanCount { get; set; }
        public int CaravansOnRouteCount { get; set; }
    }
}
