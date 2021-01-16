using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class MathematicsFocusCardResolver : FocusCardResolverBase, IScienceLevel2FocusCardResolver
    {
        private readonly ITechnologyLevelModifier _technologyLevelModifier;

        public MathematicsFocusCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITechnologyLevelModifier technologyLevelModifier,
                                             ITechnologyLevelIncreaseActionRequest technologyLevelIncreaseActionRequest,
                                             ILowestFocusTypeTokenPileIncreaseActionRequest lowestFocusTypeTokenPileIncreaseActionRequest) : base(botGameStateService, botMoveStateService)
        {
            _technologyLevelModifier = technologyLevelModifier;

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
            _technologyLevelModifier.IncrementTechnologyLevel(_botMoveStateService.BaseTechnologyIncrease);
            // TODO: increase the lowest focus type trade token pile by 1
            _currentStep = -1;
        }
    }
}
