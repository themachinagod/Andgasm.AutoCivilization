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

        public FocusBarUpgradeResponse RegenerateFocusBarLowestTechnologyLevelUpgrade(FocusBarModel activeFocusBar, FocusLevel levelBarrierHit)
        {
            var focusCardToUpgrade = GetFocusCardToUpgrade(activeFocusBar);
            var upgradeFocusCard = GetUpgradeFocusCard(focusCardToUpgrade, levelBarrierHit);
            return CreateUpgradedFocusBar(activeFocusBar, focusCardToUpgrade, upgradeFocusCard);
        }

        public FocusBarUpgradeResponse RegenerateFocusBarLowestTechnologyLevelUpgrade(FocusBarModel activeFocusBar)
        {
            var focusCardToUpgrade = GetFocusCardToUpgrade(activeFocusBar);
            var targetLevel = DetermineNextTechnologyLevelForFocusCard(focusCardToUpgrade);

            var upgradeFocusCard = GetUpgradeFocusCard(focusCardToUpgrade, targetLevel);
            return CreateUpgradedFocusBar(activeFocusBar, focusCardToUpgrade, upgradeFocusCard);
        }

        private FocusLevel DetermineNextTechnologyLevelForFocusCard(FocusCardModel cardToInspect)
        {
            if (cardToInspect.Level != FocusLevel.Lvl4)
            {
                return cardToInspect.Level++;
            }
            return cardToInspect.Level;
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

        private FocusCardModel GetFocusCardToUpgrade(FocusBarModel activeFocusBar)
        {
            var lowestTechLevel = activeFocusBar.ActiveFocusSlots.Min(x => x.Value.Level);
            var lowestTechFocusCards = activeFocusBar.ActiveFocusSlots.Where(x => x.Value.Level == lowestTechLevel).ToList();
            var focusCardToUpgrade = lowestTechFocusCards.ElementAt(_randomService.Next(lowestTechFocusCards.Count - 1));
            return focusCardToUpgrade.Value;
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
