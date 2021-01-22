using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class RemoveAdjacentBarbariansActionRequestStep : StepActionBase, IRemoveAdjacentBarbariansActionRequestStep
    {
        public RemoveAdjacentBarbariansActionRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction()
        {
            return new MoveStepActionData($"Please remove all barbarians from the board that are adjacent to my friendly space, I do not gain trade tokens for this",
                   new List<string>());
        }
    }
}
