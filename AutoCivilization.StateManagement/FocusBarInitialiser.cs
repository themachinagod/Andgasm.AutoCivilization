using AutoCivilization.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoCivilization.Console
{
    public class FocusBarInitialiser : IFocusBarInitialiser
    {
        private readonly Random _randomService = new Random();

        private readonly IGlobalGameCache _globalGameCache;

        public FocusBarInitialiser(IGlobalGameCache globalGameCache)
        {
            _globalGameCache = globalGameCache;
        }

        public FocusBarModel InitialiseFocusBarForBot()
        {
            return InitialiseFocusBar();
        }

        private FocusBarModel InitialiseFocusBar()
        {
            var cardPool = _globalGameCache.FocusCardsDeck.Where(x => x.Level == FocusLevel.Lvl1).ToList();
            var activeBar = new Dictionary<int, FocusCardModel>();
            for (int slot = 0; slot < 5; slot++)
            {
                var cardPoolRndIndex = _randomService.Next(cardPool.Count - 1);
                activeBar.Add(slot, cardPool.ElementAt(cardPoolRndIndex));
                cardPool.RemoveAt(cardPoolRndIndex);
            }
            return new FocusBarModel(new ReadOnlyDictionary<int, FocusCardModel>(activeBar));
        }
    }
}
