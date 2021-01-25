using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class ScienceResolverUtility : IScienceResolverUtility
    {
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;

        public ScienceResolverUtility(ITechnologyUpgradeResolver technologyUpgradeResolver)
        {
            _technologyUpgradeResolver = technologyUpgradeResolver;
        }

        public BotMoveStateCache CreateBasicScienceMoveState(BotGameStateCache botGameStateCache, int basePoints)
        {
            var moveState = new BotMoveStateCache();
            moveState.ActiveFocusBarForMove = botGameStateCache.ActiveFocusBar;
            moveState.StartingTechnologyLevel = botGameStateCache.TechnologyLevel;
            moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            moveState.BaseTechnologyIncrease = basePoints;
            return moveState;
        }

        public TechnologyUpgradeResponse UpdateBaseScienceGameStateForMove(BotMoveStateCache moveState, BotGameStateCache botGameStateService)
        {
            var techIncrementPoints = moveState.BaseTechnologyIncrease + moveState.TradeTokensAvailable[FocusType.Science];
            var techUpgradeResponse = _technologyUpgradeResolver.ResolveTechnologyLevelUpdates(moveState.StartingTechnologyLevel, techIncrementPoints,
                                                                                               moveState.ActiveFocusBarForMove);
            botGameStateService.ActiveFocusBar = techUpgradeResponse.UpgradedFocusBar;
            botGameStateService.TechnologyLevel = techUpgradeResponse.NewTechnologyLevelPoints;
            return techUpgradeResponse;
        }

        public string BuildGeneralisedScienceMoveSummary(string currentSummary, TechnologyUpgradeResponse upgradeResponse, BotMoveStateCache moveState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            var techIncrementPoints = moveState.BaseTechnologyIncrease + moveState.TradeTokensAvailable[FocusType.Science];
            sb.Append($"I updated my game state to show that I incremented my technology points by {techIncrementPoints} to {upgradeResponse.NewTechnologyLevelPoints}\n");
            if (moveState.TradeTokensAvailable[FocusType.Science] > 0) sb.Append($"I updated my game state to show that I used {moveState.TradeTokensAvailable[FocusType.Science]} science trade tokens I had available to me to facilitate this move\n");

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
