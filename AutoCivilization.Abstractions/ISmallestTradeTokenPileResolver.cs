using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    public interface ISmallestTradeTokenPileResolver
    {
        FocusType ResolveSmallestTokenPile(FocusBarModel activeFocusBar, Dictionary<FocusType, int> tradeTokenPiles);
    }
}
