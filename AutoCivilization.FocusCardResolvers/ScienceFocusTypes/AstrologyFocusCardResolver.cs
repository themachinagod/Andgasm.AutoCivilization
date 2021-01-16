using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class AstrologyFocusCardResolver : FocusCardResolverBase, IScienceLevel1FocusCardResolver
    {
        public AstrologyFocusCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITechnologyLevelIncreaseActionRequest technologyLevelIncreaseActionRequest) : base(botGameStateService, botMoveStateService)
        {
            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl1;

            _actionSteps.Add(0, technologyLevelIncreaseActionRequest);
        }

        public override IStepAction GetNextStep()
        {
            if (_currentStep == -1)
            {
                _botMoveStateService.BaseTechnologyIncrease = 5;
            }
            return base.GetNextStep();
        }

        public override void Resolve()
        {
            _botGameStateService.TechnologyLevel += _botMoveStateService.BaseTechnologyIncrease;
            _currentStep = -1;
        }
    }
}
