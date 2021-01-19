using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalWondersInformationRequestStep : StepActionBase, ITokenPlacementNaturalWondersInformationRequestStep
    {
        public TokenPlacementNaturalWondersInformationRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction()
        {
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;
            return (totalTokensPlaced > 0) &&
                   (_botMoveStateService.NaturalResourceTokensControlled < totalTokensPlaced);
        }

        public override MoveStepActionData ExecuteAction()
        {
            var preplaced = _botMoveStateService.NaturalResourceTokensControlled + _botMoveStateService.NaturalWonderTokensControlled;
            var maxTokensToBePlaced = (_botMoveStateService.BaseCityControlTokensToBePlaced + _botMoveStateService.CultureTokensAvailable) - preplaced;
            var options = Array.ConvertAll(Enumerable.Range(0, maxTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many natural wonders did I manage to take control of?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            _botMoveStateService.NaturalWonderTokensControlled = Convert.ToInt32(input);
        }
    }
}
