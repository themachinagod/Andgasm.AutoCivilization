using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class RemoveCaravanActionRequestStep : StepActionBase, IRemoveCaravanActionRequestStep
    {
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public RemoveCaravanActionRequestStep(IBotMoveStateCache botMoveStateService,
                                              IOrdinalSuffixResolver ordinalSuffixResolver) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;

            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override bool ShouldExecuteAction()
        {
            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[_botMoveStateService.CurrentCaravanIdToMove - 1];
            if (movingCaravan.CaravanDestinationType == CaravanDestinationType.RivalCity ||
                movingCaravan.CaravanDestinationType == CaravanDestinationType.CityState) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(_botMoveStateService.CurrentCaravanIdToMove);
            return new MoveStepActionData($"Please remove my {caravanRef} trade caravan from the city and return it to my supply in preperation for its next journey",
                   new List<string>());
        }
    }
}
