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

        public override bool ShouldExecuteAction()
        {
            // TODO: not so sure this should be conditional - if its on route then we wont be setting the trade tokens used!
            //       perhaps a workaround is to prime the economy tokens used as all available and set the TradeTokensAvailable value to 0
            //       this way: when we on route - we presume all tokens were used and supply is now 0
            //                 when we reached target - we ask this question and overwrite the primed value
            //                 what would not work in this isntance is the -= of the TradeTokensAvailable??

            return _botMoveStateService.CaravanDestinationType != CaravanDestinationType.OnRoute;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var maxMoves = _botMoveStateService.BaseCaravanMoves + _botMoveStateService.TradeTokensAvailable[FocusType.Economy];
            var options = Array.ConvertAll(Enumerable.Range(0, maxMoves + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData($"How many spaces did you manage to move my trade caravan in total?",
                   new List<string>());
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
