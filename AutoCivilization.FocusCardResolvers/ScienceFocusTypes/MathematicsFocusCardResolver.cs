using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class MathematicsFocusCardResolver : FocusCardResolverBase, IScienceLevel2FocusCardResolver
    {
        public MathematicsFocusCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITechnologyLevelIncreaseActionRequest technologyLevelIncreaseActionRequest,
                                             ILowestFocusTypeTokenPileIncreaseActionRequest lowestFocusTypeTokenPileIncreaseActionRequest) : base(botGameStateService, botMoveStateService)
        {
            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl2;

            _actionSteps.Add(0, technologyLevelIncreaseActionRequest);
            _actionSteps.Add(1, lowestFocusTypeTokenPileIncreaseActionRequest);
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
            // TODO: increase the lowest focus type trade token pile by 1
            _currentStep = -1;
        }
    }
}
