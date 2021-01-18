using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class AstrologyFocusCardResolver : FocusCardResolverBase, IScienceLevel1FocusCardResolver
    {
        private readonly ITechnologyLevelModifier _technologyLevelModifier;

        public AstrologyFocusCardResolver(IBotMoveStateService botMoveStateService,
                                             INoActionRequest noActionRequestActionRequest,
                                             ITechnologyLevelModifier technologyLevelModifier) : base(botMoveStateService)
        {
            _technologyLevelModifier = technologyLevelModifier;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl1;
        }

        public override void PrimeMoveState(IBotGameStateService botGameStateService)
        {
            _botMoveStateService.BaseTechnologyIncrease = 5;
        }

        public override string UpdateGameStateForMove(IBotGameStateService botGameStateService)
        {
            // TODO: potential bug here with reset of science tokens
            //       whereby the bot will use all available tokens on every science turn - when it hits max it wont use any
            //       this means that the bot will never accumulate science tokens even though it should after max levels hit
            //       i dont htink that the bot can do anything iwth these tokens after max level is reached so im ignoring it just now...

            // TODO the component for increase is directly updating game state - i think that should be working off move state amd then we resolve game state here!!
            var techPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.ScienceTokensAvailable;
            _botMoveStateService.ScienceTokensUsedThisTurn = _botMoveStateService.ScienceTokensAvailable;
            _technologyLevelModifier.IncrementTechnologyLevel(techPoints);
            botGameStateService.ScienceTradeTokens = 0;
            _currentStep = -1;

            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I incremented my technology points by {_botMoveStateService.BaseTechnologyIncrease} to x\n";
            if (_botMoveStateService.ScienceTokensUsedThisTurn > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.ScienceTokensUsedThisTurn} culture trade tokens I had available to me to facilitate this move\n";

            if (_technologyLevelModifier.EncounteredBreakthrough)
            {
                summary += $"As a result of my technology upgrade from x to y I had a technological breakthrough\n";
                foreach (var breakthrough in _technologyLevelModifier.BreakthroughsEncountered)
                {
                    summary += $"This breakthrough resulted in my {breakthrough.ReplacedFocusCard.Name} being replaced with {breakthrough.UpgradedFocusCard.Name}\n";
                }
            }
            return summary;
        }
    }
}
