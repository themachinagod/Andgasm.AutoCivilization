using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementCityAdjacentActionRequest : StepActionBase, ITokenPlacementCityAdjacentActionRequest
    {
        public TokenPlacementCityAdjacentActionRequest(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            return ("Please place 2 control tokens on spaces adjacent to any of my cities using the following placement priority rules:\nNatural wonder\nResource token\nVacant barbarian spawn point\nAdjacent to the most cities\nAdjacent to city closest to maturity\nHighest terrain difficulty",
                   new List<string>());
        }
    }
}
