using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoCivilization.TechnologyResolvers
{
    public class FocusBarTechnologyUpgradeResolver : IFocusBarTechnologyUpgradeResolver
    {
        private readonly Random _randomService = new Random();
        private readonly IGlobalGameCache _globalGameCache;

        public FocusBarTechnologyUpgradeResolver(IGlobalGameCache globalGameCache)
        {
            _globalGameCache = globalGameCache;
        }

        public FocusBarUpgradeResponse RegenerateFocusBarForLowestTechnologyLevelUpgrade(FocusBarModel activeFocusBar, FocusLevel levelBarrierHit)
        {
            var focusCardToUpgrade = GetFocusCardToUpgrade(activeFocusBar);
            var upgradeFocusCard = GetUpgradeFocusCard(focusCardToUpgrade.Value, levelBarrierHit);
            return CreateUpgradedFocusBar(activeFocusBar, focusCardToUpgrade.Value, upgradeFocusCard);
        }

        private FocusBarUpgradeResponse CreateUpgradedFocusBar(FocusBarModel activeFocusBar, FocusCardModel focusCardToUpgrade, FocusCardModel upgradeFocusCard)
        {
            Dictionary<int, FocusCardModel> newFocusBarCards = new Dictionary<int, FocusCardModel>();
            foreach (var fs in activeFocusBar.ActiveFocusSlots)
            {
                if (fs.Value.Name == focusCardToUpgrade.Name) newFocusBarCards.Add(fs.Key, upgradeFocusCard);
                else newFocusBarCards.Add(fs.Key, fs.Value);
            }
            var upgradedFocusBar = new FocusBarModel(new ReadOnlyDictionary<int, FocusCardModel>(newFocusBarCards));
            return new FocusBarUpgradeResponse(upgradedFocusBar, focusCardToUpgrade, upgradeFocusCard);
        }

        private KeyValuePair<int, FocusCardModel> GetFocusCardToUpgrade(FocusBarModel activeFocusBar)
        {
            var lowestTechLevel = activeFocusBar.ActiveFocusSlots.Min(x => x.Value.Level);
            var lowestTechFocusCards = activeFocusBar.ActiveFocusSlots.Where(x => x.Value.Level == lowestTechLevel).ToList();
            var focusCardToUpgrade = lowestTechFocusCards.ElementAt(_randomService.Next(lowestTechFocusCards.Count - 1));
            return focusCardToUpgrade;
        }

        private FocusCardModel GetUpgradeFocusCard(FocusCardModel focusCardToUpgrade, FocusLevel levelBarrierHit)
        {
            if (focusCardToUpgrade.Level < levelBarrierHit)
            {
                return _globalGameCache.FocusCardsDeck.First(x => x.Type == focusCardToUpgrade.Type &&
                                                                                  x.Level == levelBarrierHit);
            }
            else
            {
                return _globalGameCache.FocusCardsDeck.First(x => x.Type == focusCardToUpgrade.Type &&
                                                                                  x.Level == focusCardToUpgrade.Level + 1);
            }
        }
    }
}
