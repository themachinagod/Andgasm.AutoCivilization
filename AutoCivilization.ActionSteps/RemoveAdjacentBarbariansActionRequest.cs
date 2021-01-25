using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class RemoveAdjacentBarbariansActionRequestStep : StepActionBase, IRemoveAdjacentBarbariansActionRequestStep
    {
        public RemoveAdjacentBarbariansActionRequestStep() : base()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            return new MoveStepActionData($"Please remove all barbarians from the board that are adjacent to my friendly space, I do not gain trade tokens for this",
                   new List<string>());
        }
    }
}
