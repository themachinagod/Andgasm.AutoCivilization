using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementTerritoryAdjacentInformationRequest : StepActionBase, ITokenPlacementTerritoryAdjacentInformationRequest
    {
        public TokenPlacementTerritoryAdjacentInformationRequest() : base()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var maxControlTokensToBePlaced = moveState.BaseTerritoryControlTokensToBePlaced + moveState.TradeTokensAvailable[FocusType.Culture];
            var options = Array.ConvertAll(Enumerable.Range(0, maxControlTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many control tokens did you manage to place next to my friendly territory on the board?",
                   options);
        }

        /// <summary>
        /// Take in the number of control tokens the user managed to place on the board next to bot territory
        /// Update move state with how many culture tokens were used to facilitate placements
        /// Update move state control tokens placed counter
        /// </summary>
        /// <param name="input">The number of control tokens placed next to friendly territory</param>
        /// /// <param name="moveState">The current move state to work from</param>
        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var territoryControlTokensPlaced = Convert.ToInt32(input);
            var cultureTokensUsedThisTurn = territoryControlTokensPlaced - moveState.BaseTerritoryControlTokensToBePlaced;
            moveState.CultureTokensUsedThisTurn += (cultureTokensUsedThisTurn < 0) ? 0 : cultureTokensUsedThisTurn;
            moveState.TradeTokensAvailable[FocusType.Culture] -= (cultureTokensUsedThisTurn < 0) ? 0 : cultureTokensUsedThisTurn;
            moveState.TerritroyControlTokensPlacedThisTurn = territoryControlTokensPlaced;
        }
    }
}
