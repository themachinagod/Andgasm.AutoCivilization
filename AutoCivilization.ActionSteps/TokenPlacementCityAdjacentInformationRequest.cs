using AutoCivilization.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementCityAdjacentInformationRequest : StepActionBase
    {
        public TokenPlacementCityAdjacentInformationRequest(IBotMoveStateService botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            var maxTokensToBePlaced = _botMoveStateService.BaseCityControlTokensToBePlaced + _botMoveStateService.CultureTokensAvailable;
            var prevAllocatedTokens = _botMoveStateService.TerritroyControlTokensPlaced;
            var availableTokens = maxTokensToBePlaced - prevAllocatedTokens;
            var options = Array.ConvertAll(Enumerable.Range(0, availableTokens + 1).ToArray(), ele => ele.ToString());
            return ("How many control tokens did you manage to place next to my cities on the board?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            _botMoveStateService.CityControlTokensPlaced = Convert.ToInt32(input);
        }
    }
}
