using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class TechnologyLevelIncreaseActionRequest : StepActionBase, ITechnologyLevelIncreaseActionRequest
    {
        public TechnologyLevelIncreaseActionRequest(IBotMoveStateService botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            return ("I will automatically incremented my technology level by 5 points.",
                   new List<string>());
        }
    }
}
