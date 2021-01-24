﻿using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class AstrologyFocusCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel1FocusCardMoveResolver
    {
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;

        private const int BaseTechIncreasePoints = 5;

        public AstrologyFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                             INoActionStep noActionRequestActionRequest,
                                             ITechnologyUpgradeResolver technologyUpgradeResolver) : base(botMoveStateService)
        {
            _technologyUpgradeResolver = technologyUpgradeResolver;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl1;
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.ActiveFocusBarForMove = botGameStateService.ActiveFocusBar;
            _botMoveStateService.StartingTechnologyLevel = botGameStateService.TechnologyLevel;
            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);
            _botMoveStateService.BaseTechnologyIncrease = BaseTechIncreasePoints;
        }

        /// <summary>
        /// Resolve the updated game state from the current move state
        /// Updtes game states active focus bar to incorporate any new upgrades
        /// Update game states technoology points
        /// Update game states trade tokens counters
        /// Increment the moves step counter
        /// </summary>
        /// <param name="botGameStateService">The game state to update for move</param>
        /// <returns>A textual summary of what the bot did this move</returns>
        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            var techUpgradeResponse = _technologyUpgradeResolver.ResolveTechnologyLevelUpdates(_botMoveStateService.StartingTechnologyLevel, techIncrementPoints, 
                                                                                               _botMoveStateService.ActiveFocusBarForMove);
            botGameStateService.ActiveFocusBar = techUpgradeResponse.UpgradedFocusBar;
            botGameStateService.TechnologyLevel = techUpgradeResponse.NewTechnologyLevelPoints;
            botGameStateService.TradeTokens[FocusType.Science] = 0;
            _currentStep = -1;

            return BuildMoveSummary(techUpgradeResponse);
        }

        private string BuildMoveSummary(TechnologyUpgradeResponse upgradeResponse)
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I incremented my technology points by {techIncrementPoints} to {upgradeResponse.NewTechnologyLevelPoints}\n";
            if (_botMoveStateService.TradeTokensAvailable[FocusType.Science] > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.TradeTokensAvailable[FocusType.Science]} science trade tokens I had available to me to facilitate this move\n";

            if (upgradeResponse.EncounteredBreakthroughs.Count > 0)
            {
                summary += $"As a result of my technology upgrade, I had a technological breakthrough\n";
                foreach (var breakthrough in upgradeResponse.EncounteredBreakthroughs)
                {
                    summary += $"This breakthrough resulted in my {breakthrough.ReplacedFocusCard.Name} being replaced with {breakthrough.UpgradedFocusCard.Name}\n";
                }
            }
            return summary;
        }
    }
}
