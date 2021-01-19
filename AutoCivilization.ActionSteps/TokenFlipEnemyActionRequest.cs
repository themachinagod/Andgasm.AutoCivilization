using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class TokenFlipEnemyActionRequest : StepActionBase, ITokenFlipEnemyActionRequest
    {
        public TokenFlipEnemyActionRequest(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            return ("For each rival control token adjacent to a friendly space, flip that token to its unreinforced side. If that token was already unreinforced, remove it from the map instead.",
                   new List<string>());
        }
    }
}
