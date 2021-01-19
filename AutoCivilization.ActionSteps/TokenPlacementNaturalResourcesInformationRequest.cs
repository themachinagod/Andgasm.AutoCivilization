using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalResourcesInformationRequest : StepActionBase, ITokenPlacementNaturalResourcesInformationRequest
    {
        public TokenPlacementNaturalResourcesInformationRequest(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            StepIndex = 3;
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction()
        {
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;
            return (totalTokensPlaced > 0) &&
                   (_botMoveStateService.NaturalWonderTokensControlled < totalTokensPlaced);
        }

        public override (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction()
        {
            var preplaced = _botMoveStateService.NaturalResourceTokensControlled + _botMoveStateService.NaturalWonderTokensControlled;
            var maxTokensToBePlaced = (_botMoveStateService.BaseCityControlTokensToBePlaced + _botMoveStateService.CultureTokensAvailable) - preplaced;

            var options = Array.ConvertAll(Enumerable.Range(0, maxTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return ("How many natural resources did I manage to take control of?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            _botMoveStateService.NaturalResourceTokensControlled = Convert.ToInt32(input);
        }
    }
}
