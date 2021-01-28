using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IWonderCardDecksInitialiser
    {
        WonderCardDecksModel InitialiseDecksForBot(int playerCount);
        WonderCardDecksModel RegenerateDeckForPurchasedWonder(IList<WonderCardModel> availableWonders, IDictionary<FocusType, WonderCardModel> unlockedWonders, WonderCardModel purchasedWonder);
    }
}
