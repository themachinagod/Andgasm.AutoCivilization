using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class AstrologyFocusCardResolver : FocusCardMoveResolverBase, IScienceLevel1FocusCardResolver
    {
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;



        public AstrologyFocusCardResolver(IBotMoveStateCache botMoveStateService,
                                             INoActionRequest noActionRequestActionRequest,
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
            _botMoveStateService.ScienceTokensAvailable = botGameStateService.ScienceTradeTokens;
            _botMoveStateService.BaseTechnologyIncrease = 5;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.ScienceTokensAvailable;
            var techUpgradeResponse = _technologyUpgradeResolver.ResolveTechnologyLevelUpdates(_botMoveStateService.StartingTechnologyLevel, techIncrementPoints, 
                                                                                               _botMoveStateService.ActiveFocusBarForMove);

            botGameStateService.ActiveFocusBar = techUpgradeResponse.UpgradedFocusBar;
            botGameStateService.TechnologyLevel = techUpgradeResponse.NewTechnologyLevelPoints;
            botGameStateService.ScienceTradeTokens = 0;
            _currentStep = -1;

            return BuildMoveSummary(techUpgradeResponse);
        }

        private string BuildMoveSummary(TechnologyUpgradeResponse upgradeResponse)
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.ScienceTokensAvailable;
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I incremented my technology points by {techIncrementPoints} to {upgradeResponse.NewTechnologyLevelPoints}\n";
            if (_botMoveStateService.ScienceTokensAvailable > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.ScienceTokensAvailable} science trade tokens I had available to me to facilitate this move\n";

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
