using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.TechnologyResolvers
{
    public class SmallestTradeTokenPileResolver : ISmallestTradeTokenPileResolver
    {
        public FocusType ResolveSmallestTokenPile(FocusBarModel activeFocusBar, Dictionary<FocusType, int> tradeTokenPiles)
        {
            var smallestPileValue = 0;
            var smallestPileType = FocusType.Culture;
            foreach (var x in activeFocusBar.ActiveFocusSlots)
            {
                var tokenpile = tradeTokenPiles[x.Value.Type];
                if (tokenpile <= smallestPileValue)
                    smallestPileType = x.Value.Type;
            }
            return smallestPileType;
        }
    }
}
