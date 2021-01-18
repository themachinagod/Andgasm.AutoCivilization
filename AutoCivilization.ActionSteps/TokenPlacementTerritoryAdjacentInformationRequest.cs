using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementTerritoryAdjacentInformationRequest : StepActionBase, ITokenPlacementTerritoryAdjacentInformationRequest
    {
        public TokenPlacementTerritoryAdjacentInformationRequest(IBotMoveStateService botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            var maxControlTokensToBePlaced = _botMoveStateService.BaseTerritoryControlTokensToBePlaced + _botMoveStateService.CultureTokensAvailable;
            var options = Array.ConvertAll(Enumerable.Range(0, maxControlTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return ("How many control tokens did you manage to place next to my friendly territory on the board?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            var territoryControlTokensPlaced = Convert.ToInt32(input);
            var cultureTokensUsedThisTurn = territoryControlTokensPlaced - _botMoveStateService.BaseTerritoryControlTokensToBePlaced;
            _botMoveStateService.CultureTokensUsedThisTurn += cultureTokensUsedThisTurn;
            _botMoveStateService.CultureTokensAvailable -= (cultureTokensUsedThisTurn < 0) ? 0 : cultureTokensUsedThisTurn;
            _botMoveStateService.TerritroyControlTokensPlaced = Convert.ToInt32(input);
        }
    }
}
