using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    public class BotGameStateService : IBotGameStateService
    {
        public int TechnologyLevel { get; set; }
        public IReadOnlyCollection<FocusCardModel> FocusCardsDeck { get; set; }
        public LeaderCardModel ChosenLeaderCard { get; set; }
        
        public FocusBarModel ActiveFocusBar { get; set; }
    }
}
