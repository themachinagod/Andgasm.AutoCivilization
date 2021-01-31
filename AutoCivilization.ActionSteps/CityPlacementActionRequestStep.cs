using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class CityPlacementActionRequestStep : StepActionBase, ICityPlacementActionRequestStep
    {
        public CityPlacementActionRequestStep() : base()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var maxPlacements = moveState.BaseCityControlTokensToBePlaced + moveState.TradeTokensAvailable[FocusType.Culture];
            return new MoveStepActionData($"Please build me 1 new friendly city on a legal space that is within 2 spaces of my friendly territory using the following priority order;\nAdjacent to the most natural wonder and/or resource tokens\nA space with a barbarian spawn point\nHighest defense",
                   new List<string>());
        }
    }
}
