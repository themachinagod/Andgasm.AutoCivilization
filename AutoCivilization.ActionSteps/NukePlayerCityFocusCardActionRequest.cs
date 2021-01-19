using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class NukePlayerCityFocusCardActionRequest : StepActionBase, INukePlayerCityFocusCardActionRequest
    {
        public NukePlayerCityFocusCardActionRequest(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            return ("Each human player should pick a city that is not their capital, and remove the city as well as all adjacent control tokens from the board.",
                   new List<string>());
        }
    }
}
