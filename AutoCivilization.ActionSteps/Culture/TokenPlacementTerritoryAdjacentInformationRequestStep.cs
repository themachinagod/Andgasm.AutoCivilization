using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementTerritoryAdjacentInformationRequestStep : StepActionBase, ITokenPlacementTerritoryAdjacentInformationRequest
    {
        public TokenPlacementTerritoryAdjacentInformationRequestStep() : base()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var maxControlTokensToBePlaced = moveState.BaseTerritoryControlTokensToBePlaced;
            var options = Array.ConvertAll(Enumerable.Range(0, maxControlTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many control tokens did you manage to place next to my friendly territory on the board?",
                   options);
        }

        /// <summary>
        /// Take in the number of control tokens the user managed to place on the board next to bot territory
        /// This action does NOT allow use of trade tokens as it is a secondary action
        /// Update territory tokens placed in movestate
        /// </summary>
        /// <param name="input">The number of control tokens placed next to friendly territory</param>
        /// /// <param name="moveState">The current move state to work from</param>
        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var territoryControlTokensPlaced = Convert.ToInt32(input);
            moveState.TerritroyControlTokensPlacedThisTurn = territoryControlTokensPlaced;
        }
    }
}
