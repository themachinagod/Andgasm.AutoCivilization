using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class CaravanMovementActionRequestStep : StepActionBase, ICaravanMovementActionRequestStep
    {
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public CaravanMovementActionRequestStep(IOrdinalSuffixResolver ordinalSuffixResolver) : base()
        {
            OperationType = OperationType.ActionRequest;

            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: would e useful to show some detail on the priority based on move state
            //       e.g. if the bot has already visited all city states then we can remove that from option list
            //       e.g. for each priotiy option that is still applicable - we can show whats left to visit 
            //              for city state: brussels, carthage...
            //              for rival city: red...
            //              for visited city state: buenos ares...

            if (moveState.CurrentCaravanIdToMove < moveState.SupportedCaravanCount) moveState.CurrentCaravanIdToMove++;
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            var maxSpacesMoved = moveState.BaseCaravanMoves + moveState.TradeTokensAvailable[FocusType.Economy];
            var watertext = moveState.CanMoveOnWater ? "making use of water tiles where needed" : "avoiding the use of water tiles";
            return new MoveStepActionData($"Please move my {caravanRef} trade caravan {maxSpacesMoved} spaces toward its destination, taking the shortest path {watertext}, using the following destination priority rules:\nUnvisited City State\nRival City\nVisited City State",
                   new List<string>());
        }
    }
}
