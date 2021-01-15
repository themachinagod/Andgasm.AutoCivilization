using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    public class BotGameStateService : IBotGameStateService
    {
        public LeaderCardModel ChosenLeaderCard { get; set; }

        public IReadOnlyCollection<FocusCardModel> FocusCardsDeck { get; set; }
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
