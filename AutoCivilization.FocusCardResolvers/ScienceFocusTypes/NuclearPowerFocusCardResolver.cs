using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class NuclearPowerFocusCardResolver : FocusCardResolverBase, IScienceLevel4FocusCardResolver
    {
        private readonly ITechnologyLevelModifier _technologyLevelModifier;

        public NuclearPowerFocusCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITechnologyLevelModifier technologyLevelModifier,
                                             INukePlayerCityFocusCardActionRequest nukePlayerCityFocusCardActionRequest) : base(botGameStateService, botMoveStateService)
        {
            _technologyLevelModifier = technologyLevelModifier;

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl4;
           
            _actionSteps.Add(0, nukePlayerCityFocusCardActionRequest);
        }

        public override IStepAction GetNextStep()
        {
            if (_currentStep == -1)
            {
                _botMoveStateService.BaseTechnologyIncrease = 5;
            }
            return base.GetNextStep();
        }

        public override string Resolve()
        {
            // TODO: potential bug here with reset of science tokens
            //       whereby the bot will use all available tokens on every science turn - when it hits max it wont use any
            //       this means that the bot will never accumulate science tokens even though it should after max levels hit
            //       i dont htink that the bot can do anything iwth these tokens after max level is reached so im ignoring it just now...

            var techPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.ScienceTokensAvailable;
            _botMoveStateService.ScienceTokensUsedThisTurn = _botMoveStateService.ScienceTokensAvailable;
            _technologyLevelModifier.IncrementTechnologyLevel(techPoints);
            _botGameStateService.ScienceTradeTokens = 0;
            _currentStep = -1;

            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I incremented my technology points by {_botMoveStateService.BaseTechnologyIncrease} to {_botGameStateService.TechnologyLevel}\n";
            if (_botMoveStateService.ScienceTokensUsedThisTurn > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.ScienceTokensUsedThisTurn} culture trade tokens I had available to me to facilitate this move\n";

            if (_technologyLevelModifier.EncounteredBreakthrough)
            {
                summary += $"As a result of my technology upgrade from x to y I had a technological breakthrough\n";
                summary += $"This breakthrough resulted in my {_technologyLevelModifier.ReplacedFocusCard.Name} being replaced with {_technologyLevelModifier.UpgradedFocusCard.Name}\n";
            }

            summary += "I had each player destroy one of their cities and its surrounding controlled territory via the use of a nuclear assault\n";
            return summary;
        }
    }
}
