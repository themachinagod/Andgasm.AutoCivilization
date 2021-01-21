using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class CaravanMovementInformationRequestStep : StepActionBase, ICaravanMovementInformationRequestStep
    {
        public CaravanMovementInformationRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var maxMoves = _botMoveStateService.BaseCaravanMoves + _botMoveStateService.TradeTokensAvailable[FocusType.Economy];
            var options = Array.ConvertAll(Enumerable.Range(0, maxMoves + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData($"How many spaces did you manage to move my trade caravan in total?",
                   options);
        }

        public override void ProcessActionResponse(string input)
        {
            _botMoveStateService.CaravanSpacesMoved = Convert.ToInt32(input);

            var economyTokensUsedThisTurn = _botMoveStateService.CaravanSpacesMoved - _botMoveStateService.BaseCaravanMoves;
            _botMoveStateService.EconomyTokensUsedThisTurn = economyTokensUsedThisTurn;
            _botMoveStateService.TradeTokensAvailable[FocusType.Economy] -= economyTokensUsedThisTurn;
        }
    }
}
