using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementCityAdjacentActionRequestStep : StepActionBase, ITokenPlacementCityAdjacentActionRequestStep
    {
        public TokenPlacementCityAdjacentActionRequestStep() : base()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            var maxPlacements = moveState.BaseCityControlTokensToBePlaced + moveState.TradeTokensAvailable[FocusType.Culture];
            return new MoveStepActionData($"Please place {maxPlacements} control tokens on spaces adjacent to any of my cities using the following placement priority rules:\nNatural wonder\nResource token\nVacant barbarian spawn point\nAdjacent to the most cities\nAdjacent to city closest to maturity\nHighest terrain difficulty\nFor each token that cannot be placed, I will recieve 1 culture trade token to be used in the future",
                   new List<string>());
        }
    }
}
