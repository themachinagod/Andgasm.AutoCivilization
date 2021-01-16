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
                                             ITechnologyLevelModifier technologyLevelModifier,
                                             ITechnologyLevelIncreaseActionRequest technologyLevelIncreaseActionRequest,
                                             IUpgradeLowestFocusCardActionRequest upgradeLowestFocusCardActionRequest) : base(botGameStateService, botMoveStateService)
        {
            _technologyLevelModifier = technologyLevelModifier;

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl3;

            _actionSteps.Add(0, technologyLevelIncreaseActionRequest);
            _actionSteps.Add(1, upgradeLowestFocusCardActionRequest);
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
            // TODO: upgrade the lowest tech level card on focus bar (use highest placed on tie)
            _currentStep = -1;
        }
    }
}
