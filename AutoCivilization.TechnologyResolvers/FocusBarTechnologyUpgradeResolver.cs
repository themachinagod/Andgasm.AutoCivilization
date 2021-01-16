using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.TechnologyResolvers
{
    public class FocusBarTechnologyUpgradeResolver : IFocusBarTechnologyUpgradeResolver
    {
        private readonly Random _randomService = new Random();
        private readonly IBotGameStateService _botGameStateService;

        public FocusBarTechnologyUpgradeResolver(IBotGameStateService botGameStateService)
        {
            _botGameStateService = botGameStateService;
        }

        public void UpgradeFocusBarsLowestTechLevel(FocusType focusType, FocusLevel levelBarrierHit)
        {
            var focusCardToUpgrade = GetFocusCardToUpgrade();
            var upgradeFocusCard = GetUpgradeFocusCard(focusCardToUpgrade.Value, levelBarrierHit);
            _botGameStateService.ActiveFocusBar.ActiveFocusSlots[focusCardToUpgrade.Key] = upgradeFocusCard;
        }

        private KeyValuePair<int, FocusCardModel> GetFocusCardToUpgrade()
        {
            var lowestTechLevel = _botGameStateService.ActiveFocusBar.ActiveFocusSlots.Min(x => x.Value.Level);
            var lowestTechFocusCards = _botGameStateService.ActiveFocusBar.ActiveFocusSlots.Where(x => x.Value.Level == lowestTechLevel).ToList();
            var focusCardToUpgrade = lowestTechFocusCards.ElementAt(_randomService.Next(lowestTechFocusCards.Count - 1));
            return focusCardToUpgrade;
        }

        private FocusCardModel GetUpgradeFocusCard(FocusCardModel focusCardToUpgrade, FocusLevel levelBarrierHit)
        {
            if (focusCardToUpgrade.Level < levelBarrierHit)
            {
                return _botGameStateService.FocusCardsDeck.First(x => x.Type == focusCardToUpgrade.Type &&
                                                                                  x.Level == levelBarrierHit);
            }
            else
            {
                return _botGameStateService.FocusCardsDeck.First(x => x.Type == focusCardToUpgrade.Type &&
                                                                                  x.Level == focusCardToUpgrade.Level + 1);
            }
        }
    }
}
