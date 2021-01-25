using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class RemoveCaravanActionRequestStep : StepActionBase, IRemoveCaravanActionRequestStep
    {
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public RemoveCaravanActionRequestStep(IOrdinalSuffixResolver ordinalSuffixResolver) : base()
        {
            OperationType = OperationType.InformationRequest;

            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override bool ShouldExecuteAction(BotMoveStateCache moveState)
        {
            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
            if (movingCaravan.CaravanDestinationType == CaravanDestinationType.RivalCity ||
                movingCaravan.CaravanDestinationType == CaravanDestinationType.CityState) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            return new MoveStepActionData($"Please remove my {caravanRef} trade caravan from the city and return it to my supply in preperation for its next journey",
                   new List<string>());
        }
    }
}
