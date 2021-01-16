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
                                             ITechnologyLevelIncreaseActionRequest technologyLevelIncreaseActionRequest,
                                             INukePlayerCityFocusCardActionRequest nukePlayerCityFocusCardActionRequest) : base(botGameStateService, botMoveStateService)
        {
            _technologyLevelModifier = technologyLevelModifier;

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl4;
           
            _actionSteps.Add(0, technologyLevelIncreaseActionRequest);
            _actionSteps.Add(1, nukePlayerCityFocusCardActionRequest);
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
