using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class RemoveCaravanActionRequestStep : StepActionBase, IRemoveCaravanActionRequestStep
    {
        public RemoveCaravanActionRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction()
        {
            if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.RivalCity ||
                _botMoveStateService.CaravanDestinationType == CaravanDestinationType.CityState) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction()
        {
            // TODO: we need a players and colors
            //       currently hard wired for two player game!

            return new MoveStepActionData("Please remove the trade caravan from the city and return it to my supply in preperation for its next journey",
                   new List<string>());
        }
    }
}
