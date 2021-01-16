using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class AstrologyFocusCardResolver : FocusCardResolverBase, IScienceLevel1FocusCardResolver
    {
        private readonly ITechnologyLevelModifier _technologyLevelModifier;

        public AstrologyFocusCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITechnologyLevelModifier technologyLevelModifier,
                                             ITechnologyLevelIncreaseActionRequest technologyLevelIncreaseActionRequest) : base(botGameStateService, botMoveStateService)
        {
            _technologyLevelModifier = technologyLevelModifier;

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
            _technologyLevelModifier.IncrementTechnologyLevel(_botMoveStateService.BaseTechnologyIncrease);
            _currentStep = -1;
        }
    }
}
