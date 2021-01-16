using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class LowestFocusTypeTokenPileIncreaseActionRequest : StepActionBase, ILowestFocusTypeTokenPileIncreaseActionRequest
    {
        public LowestFocusTypeTokenPileIncreaseActionRequest(IBotMoveStateService botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            return ("I will automatically incremented my smallest focus type token pile by 1.",
                   new List<string>());
        }
    }
}
