using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class NoActionRequest : StepActionBase, INoActionRequest
    {
        public NoActionRequest(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction()
        {
            return new MoveStepActionData("There are no physical actions I require from you for this move, nor are there any questions I need to ask at this time.",
                   new List<string>());
        }
    }
}
