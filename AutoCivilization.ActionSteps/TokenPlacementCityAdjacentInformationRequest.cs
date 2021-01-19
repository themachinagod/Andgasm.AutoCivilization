using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementCityAdjacentInformationRequestStep : StepActionBase, ITokenPlacementCityAdjacentInformationRequestStep
    {
        public TokenPlacementCityAdjacentInformationRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var maxControlTokensToBePlaced = _botMoveStateService.BaseCityControlTokensToBePlaced + _botMoveStateService.TradeTokensAvailable[FocusType.Culture];
            var options = Array.ConvertAll(Enumerable.Range(0, maxControlTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many control tokens did you manage to place next to my cities on the board?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            var cityControlTokensPlaced = Convert.ToInt32(input);
            var cultureTokensUsedThisTurn = cityControlTokensPlaced - _botMoveStateService.BaseCityControlTokensToBePlaced;
            _botMoveStateService.CultureTokensUsedThisTurn = cultureTokensUsedThisTurn;
            _botMoveStateService.TradeTokensAvailable[FocusType.Culture] -= cultureTokensUsedThisTurn;
            _botMoveStateService.CityControlTokensPlaced = cityControlTokensPlaced;
        }
    }
}
