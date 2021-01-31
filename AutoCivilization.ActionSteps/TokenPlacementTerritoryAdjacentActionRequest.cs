using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementTerritoryAdjacentActionRequestStep : StepActionBase, ITokenPlacementTerritoryAdjacentActionRequestStep
    {
        public TokenPlacementTerritoryAdjacentActionRequestStep() : base()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var maxPlacements = moveState.BaseTerritoryControlTokensToBePlaced;
            return new MoveStepActionData($"Please place {maxPlacements} control tokens on spaces adjacent to any of my friendly territory using the following placement priority rules:\nNatural wonder\nResource token\nVacant barbarian spawn point\nAdjacent to the most cities\nAdjacent to city closest to maturity\nHighest terrain difficulty",
                   new List<string>());
        }
    }
}
