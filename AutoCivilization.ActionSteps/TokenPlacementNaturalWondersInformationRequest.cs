using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalWondersInformationRequest : StepActionBase, ITokenPlacementNaturalWondersInformationRequest
    {
        public TokenPlacementNaturalWondersInformationRequest(IBotMoveStateService botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction()
        {
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;
            return (totalTokensPlaced > 0) &&
                   (_botMoveStateService.NaturalResourceTokensControlled < totalTokensPlaced);
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            var maxTokensToBePlaced = _botMoveStateService.BaseCityControlTokensToBePlaced + _botMoveStateService.CultureTokensAvailable;
            var options = Array.ConvertAll(Enumerable.Range(0, maxTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return ("How many natural wonders did I manage to take control of?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            _botMoveStateService.NaturalWonderTokensControlled = Convert.ToInt32(input);
        }
    }
}
