using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementTerritoryAdjacentInformationRequest : StepActionBase, ITokenPlacementTerritoryAdjacentInformationRequest
    {
        public TokenPlacementTerritoryAdjacentInformationRequest(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var maxControlTokensToBePlaced = _botMoveStateService.BaseTerritoryControlTokensToBePlaced + _botMoveStateService.TradeTokensAvailable[FocusType.Culture];
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
        public override void ProcessActionResponse(string input)
        {
            var territoryControlTokensPlaced = Convert.ToInt32(input);
            var cultureTokensUsedThisTurn = territoryControlTokensPlaced - _botMoveStateService.BaseTerritoryControlTokensToBePlaced;
            _botMoveStateService.CultureTokensUsedThisTurn += (cultureTokensUsedThisTurn < 0) ? 0 : cultureTokensUsedThisTurn;
            _botMoveStateService.TradeTokensAvailable[FocusType.Culture] -= (cultureTokensUsedThisTurn < 0) ? 0 : cultureTokensUsedThisTurn;
            _botMoveStateService.TerritroyControlTokensPlacedThisTurn = territoryControlTokensPlaced;
        }
    }
}
