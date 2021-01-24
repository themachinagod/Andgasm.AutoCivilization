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

        /// <summary>
        /// Take in the number of control tokens the user managed to place on the board next to bot cities
        /// Update move state with how many culture tokens were used to facilitate placements
        /// Update move state with how many culture tokens were recieved due to unused placements
        /// Update move state control tokens placed counter
        /// </summary>
        /// <param name="input">The number of control tokens placed next to cities</param>
        public override void ProcessActionResponse(string input)
        {
            var cityControlTokensPlaced = Convert.ToInt32(input);
            var cultureTokensUsedThisTurn = cityControlTokensPlaced - _botMoveStateService.BaseCityControlTokensToBePlaced;
            _botMoveStateService.CultureTokensUsedThisTurn = cultureTokensUsedThisTurn;
            _botMoveStateService.TradeTokensAvailable[FocusType.Culture] -= cultureTokensUsedThisTurn;
            _botMoveStateService.CityControlTokensPlacedThisTurn = cityControlTokensPlaced;
        }
    }
}
