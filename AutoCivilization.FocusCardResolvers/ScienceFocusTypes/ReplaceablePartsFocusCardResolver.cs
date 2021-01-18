using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class ReplaceablePartsCardResolver : FocusCardResolverBase, IScienceLevel3FocusCardResolver
    {
        private readonly ITechnologyLevelModifier _technologyLevelModifier;

        public ReplaceablePartsCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITechnologyLevelModifier technologyLevelModifier) : base(botGameStateService, botMoveStateService)
        {
            _technologyLevelModifier = technologyLevelModifier;

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl3;
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
            _technologyLevelModifier.IncrementTechnologyLevel(_botMoveStateService.BaseTechnologyIncrease);
            // TODO: upgrade the lowest tech level card on focus bar (use highest placed on tie)
            _currentStep = -1;

            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I incremented my technology points by {_botMoveStateService.BaseTechnologyIncrease} to {_botGameStateService.TechnologyLevel}\n";

            if (_technologyLevelModifier.EncounteredBreakthrough)
            {
                summary += $"As a result of my technology upgrade from x to y I had a technological breakthrough\n";
                summary += $"This breakthrough resulted in my {_technologyLevelModifier.ReplacedFocusCard.Name} being replaced with {_technologyLevelModifier.UpgradedFocusCard.Name}\n";
            }

            // TODO: summerise the tech upgrade (out of band)
            return summary;
        }
    }
}
