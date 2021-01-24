using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class ScienceResolverUtility : IScienceResolverUtility
    {
        private readonly IBotMoveStateCache _botMoveStateService;
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;

        public ScienceResolverUtility(IBotMoveStateCache botMoveStateService,
                                      ITechnologyUpgradeResolver technologyUpgradeResolver)
        {
            _botMoveStateService = botMoveStateService;
            _technologyUpgradeResolver = technologyUpgradeResolver;
        }

        public void PrimeBaseEconomyState(BotGameStateCache botGameStateCache, int basePoints)
        {
            _botMoveStateService.ActiveFocusBarForMove = botGameStateCache.ActiveFocusBar;
            _botMoveStateService.StartingTechnologyLevel = botGameStateCache.TechnologyLevel;
            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            _botMoveStateService.BaseTechnologyIncrease = basePoints;
        }

        public TechnologyUpgradeResponse UpdateBaseEconomyGameStateForMove(BotGameStateCache botGameStateService)
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            var techUpgradeResponse = _technologyUpgradeResolver.ResolveTechnologyLevelUpdates(_botMoveStateService.StartingTechnologyLevel, techIncrementPoints,
                                                                                               _botMoveStateService.ActiveFocusBarForMove);
            botGameStateService.ActiveFocusBar = techUpgradeResponse.UpgradedFocusBar;
            botGameStateService.TechnologyLevel = techUpgradeResponse.NewTechnologyLevelPoints;
            return techUpgradeResponse;
        }

        public string BuildGeneralisedEconomyMoveSummary(string currentSummary, TechnologyUpgradeResponse upgradeResponse)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            sb.Append($"I updated my game state to show that I incremented my technology points by {techIncrementPoints} to {upgradeResponse.NewTechnologyLevelPoints}\n");
            if (_botMoveStateService.TradeTokensAvailable[FocusType.Science] > 0) sb.Append($"I updated my game state to show that I used {_botMoveStateService.TradeTokensAvailable[FocusType.Science]} science trade tokens I had available to me to facilitate this move\n");

            if (upgradeResponse.EncounteredBreakthroughs.Count > 0)
            {
                sb.Append($"As a result of my technology upgrade, I had a technological breakthrough\n");
                foreach (var breakthrough in upgradeResponse.EncounteredBreakthroughs)
                {
                    sb.Append($"This breakthrough resulted in my {breakthrough.ReplacedFocusCard.Name} being replaced with {breakthrough.UpgradedFocusCard.Name}\n");
                }
            }
            return sb.ToString();
        }
    }
}
