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
        }

        public int GameId { get; set; } = 1001;
        public LeaderCardModel ChosenLeaderCard { get; set; }
        public int CurrentRoundNumber {get;set;}

        public FocusBarModel ActiveFocusBar { get; set; }

        public int TechnologyLevel { get; set; }

        public int ControlledSpaces { get; set; }
        public int ControlledResources { get; set; }
        public int ControlledWonders { get; set; }

        public int CultureTradeTokens { get; set; }
        public int MilitaryTradeTokens { get; set; }
        public int EconomyTradeTokens { get; set; }
        public int IndustryTradeTokens { get; set; }
        public int ScienceTradeTokens { get; set; }
    }
}
