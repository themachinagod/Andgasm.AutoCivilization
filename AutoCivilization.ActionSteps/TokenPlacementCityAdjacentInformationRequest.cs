using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementCityAdjacentInformationRequest : StepActionBase, ITokenPlacementCityAdjacentInformationRequest
    {
        public TokenPlacementCityAdjacentInformationRequest(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            var maxControlTokensToBePlaced = _botMoveStateService.BaseCityControlTokensToBePlaced + _botMoveStateService.CultureTokensAvailable;
            var options = Array.ConvertAll(Enumerable.Range(0, maxControlTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return ("How many control tokens did you manage to place next to my cities on the board?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            var cityControlTokensPlaced = Convert.ToInt32(input);
            var cultureTokensUsedThisTurn = cityControlTokensPlaced - _botMoveStateService.BaseCityControlTokensToBePlaced;
            _botMoveStateService.CultureTokensUsedThisTurn = cultureTokensUsedThisTurn;
            _botMoveStateService.CultureTokensAvailable -= cultureTokensUsedThisTurn;
            _botMoveStateService.CityControlTokensPlaced = cityControlTokensPlaced;
        }
    }
}
