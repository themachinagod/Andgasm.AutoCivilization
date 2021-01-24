using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalResourcesInformationRequestStep : StepActionBase, ITokenPlacementNaturalResourcesInformationRequestStep
    {
        public TokenPlacementNaturalResourcesInformationRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            StepIndex = 3;
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction()
        {
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlacedThisTurn + _botMoveStateService.TerritroyControlTokensPlacedThisTurn;
            return (totalTokensPlaced > 0) &&
                   (_botMoveStateService.NaturalWonderTokensControlledThisTurn < totalTokensPlaced);
        }

        public override MoveStepActionData ExecuteAction()
        {
            var preplaced = _botMoveStateService.NaturalResourceTokensControlledThisTurn + _botMoveStateService.NaturalWonderTokensControlledThisTurn;
            var maxTokensToBePlaced = (_botMoveStateService.BaseCityControlTokensToBePlaced + _botMoveStateService.TradeTokensAvailable[FocusType.Culture]) - preplaced;
            var options = Array.ConvertAll(Enumerable.Range(0, maxTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many natural resources did I manage to take control of this turn?",
                   options);
        }

        /// <summary>
        /// Update move state with how many natural wonders were recieved due to placed control tokens
        /// </summary>
        /// <param name="input">The number of natural wonder tokens the bot controlled this turn</param>
        public override void ProcessActionResponse(string input)
        {
            _botMoveStateService.NaturalResourceTokensControlledThisTurn = Convert.ToInt32(input);
        }
    }
}
