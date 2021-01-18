using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class NoActionRequest : StepActionBase, INoActionRequest
    {
        public NoActionRequest(IBotMoveStateService botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            return ("There are no physical actions I require from you for this shot, nor are there any questions I need to ask at this time.",
                   new List<string>());
        }
    }
}
