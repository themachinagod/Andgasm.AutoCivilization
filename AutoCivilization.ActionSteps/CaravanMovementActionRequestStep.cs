using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class CaravanMovementActionRequestStep : StepActionBase, ICaravanMovementActionRequestStep
    {
        public CaravanMovementActionRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var maxSpacesMoved = _botMoveStateService.BaseCaravanMoves + _botMoveStateService.TradeTokensAvailable[FocusType.Economy];
            return new MoveStepActionData($"Please move a single trade caravan {maxSpacesMoved} spaces on the shortest path to a target using the following placement priority rules:\nUnvisited City State\nRival City\nVisited City State",
                   new List<string>());
        }
    }
}
