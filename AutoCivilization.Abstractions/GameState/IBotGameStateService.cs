using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IBotGameStateService
    {
        int GameId { get; set; }
        LeaderCardModel ChosenLeaderCard { get; set; }
        int CurrentRoundNumber { get; set; }

        IReadOnlyCollection<FocusCardModel> FocusCardsDeck { get; set; }
        FocusBarModel ActiveFocusBar { get; set; }

        int ControlledSpaces { get; set; }
        int ControlledResources { get; set; }
        int ControlledWonders { get; set; }

        int CultureTradeTokens { get; set; }
        int MilitaryTradeTokens { get; set; }
        int EconomyTradeTokens { get; set; }
        int IndustryTradeTokens { get; set; }
        int ScienceTradeTokens { get; set; }
    }
}
