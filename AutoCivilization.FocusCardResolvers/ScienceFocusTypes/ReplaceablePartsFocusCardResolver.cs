using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class ReplaceablePartsCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel3FocusCardMoveResolver
    {
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;

        private FocusBarUpgradeResponse _freeTechUpgradeResponse;
        private TechnologyUpgradeResponse _techLevelUpgradeResponse;

        public ReplaceablePartsCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                             INoActionStep noActionRequestActionRequest,
                                             ITechnologyUpgradeResolver technologyUpgradeResolver) : base(botMoveStateService)
        {
            _technologyUpgradeResolver = technologyUpgradeResolver;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl3;
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.ActiveFocusBarForMove = botGameStateService.ActiveFocusBar;
            _botMoveStateService.StartingTechnologyLevel = botGameStateService.TechnologyLevel;

            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);

            _botMoveStateService.BaseTechnologyIncrease = 5;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            _freeTechUpgradeResponse = _technologyUpgradeResolver.ResolveFreeTechnologyUpdate(_botMoveStateService.ActiveFocusBarForMove);
            _techLevelUpgradeResponse = _technologyUpgradeResolver.ResolveTechnologyLevelUpdates(_botMoveStateService.StartingTechnologyLevel, techIncrementPoints,
                                                                                               _freeTechUpgradeResponse.UpgradedFocusBar);

            botGameStateService.ActiveFocusBar = _techLevelUpgradeResponse.UpgradedFocusBar;
            botGameStateService.TechnologyLevel = _techLevelUpgradeResponse.NewTechnologyLevelPoints;
            botGameStateService.TradeTokens[FocusType.Science] = 0;
            _currentStep = -1;

            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            var summary = "To summarise my move I did the following;\n";
            if (_freeTechUpgradeResponse.HasUpgraded && _freeTechUpgradeResponse.OldTechnology.Name != _freeTechUpgradeResponse.NewTechnology.Name)
            {
                summary += $"I received a free technology upgrade breakthrough allowing me to upgrade {_freeTechUpgradeResponse.OldTechnology.Name} to {_freeTechUpgradeResponse.NewTechnology.Name}\n";
            }

            summary += $"I updated my game state to show that I incremented my technology points by {techIncrementPoints} to {_techLevelUpgradeResponse.NewTechnologyLevelPoints}\n";
            if (_botMoveStateService.TradeTokensAvailable[FocusType.Science] > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.TradeTokensAvailable[FocusType.Science]} science trade tokens I had available to me to facilitate this move\n";

            if (_techLevelUpgradeResponse.EncounteredBreakthroughs.Count > 0)
            {
                summary += $"As a result of my technology upgrade, I had a technological breakthrough\n";
                foreach (var breakthrough in _techLevelUpgradeResponse.EncounteredBreakthroughs)
                {
                    summary += $"This breakthrough resulted in my {breakthrough.ReplacedFocusCard.Name} being replaced with {breakthrough.UpgradedFocusCard.Name}\n";
                }
            }
            return summary;
        }
    }
}
