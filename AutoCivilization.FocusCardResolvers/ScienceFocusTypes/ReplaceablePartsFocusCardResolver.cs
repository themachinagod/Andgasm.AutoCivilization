using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class ReplaceablePartsCardResolver : FocusCardResolverBase, IScienceLevel3FocusCardResolver
    {
        public ReplaceablePartsCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITechnologyLevelIncreaseActionRequest technologyLevelIncreaseActionRequest,
                                             IUpgradeLowestFocusCardActionRequest upgradeLowestFocusCardActionRequest) : base(botGameStateService, botMoveStateService)
        {
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
            _botGameStateService.TechnologyLevel += _botMoveStateService.BaseTechnologyIncrease;
            // TODO: upgrade the lowest tech level card on focus bar (use highest placed on tie)
            _currentStep = -1;
        }
    }
}
