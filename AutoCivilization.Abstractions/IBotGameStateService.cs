using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IBotGameStateService
    {
        LeaderCardModel ChosenLeaderCard { get; set; }
        IReadOnlyCollection<FocusCardModel> FocusCardsDeck { get; set; }
        FocusBarModel ActiveFocusBar { get; set; }
    }
}
