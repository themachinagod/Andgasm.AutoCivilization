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
            // TODO: would e useful to show some detail on the priority based on move state
            //       e.g. if the bot has already visited all city states then we can remove that from option list
            //       e.g. for each priotiy option that is still applicable - we can show whats left to visit 
            //              for city state: brussels, carthage...
            //              for rival city: red...
            //              for visited city state: buenos ares...

            var maxSpacesMoved = _botMoveStateService.BaseCaravanMoves + _botMoveStateService.TradeTokensAvailable[FocusType.Economy];
            return new MoveStepActionData($"Please move a single trade caravan {maxSpacesMoved} spaces on the shortest path to a target using the following placement priority rules:\nUnvisited City State\nRival City\nVisited City State",
                   new List<string>());
        }
    }
}
