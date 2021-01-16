using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class UpgradeLowestFocusCardActionRequest : StepActionBase, IUpgradeLowestFocusCardActionRequest
    {
        public UpgradeLowestFocusCardActionRequest(IBotMoveStateService botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            return ("I will automatically upgrade my lowest focus card on my focus bar to the next technology level.",
                   new List<string>());
        }
    }
}
