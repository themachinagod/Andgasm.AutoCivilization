using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class CaravanMovementInformationRequestStep : StepActionBase, ICaravanMovementInformationRequestStep
    {
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public CaravanMovementInformationRequestStep(IBotMoveStateCache botMoveStateService,
                                                     IOrdinalSuffixResolver ordinalSuffixResolver) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;

            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(_botMoveStateService.CurrentCaravanIdToMove);
            var maxMoves = _botMoveStateService.BaseCaravanMoves + _botMoveStateService.TradeTokensAvailable[FocusType.Economy];
            var options = Array.ConvertAll(Enumerable.Range(0, maxMoves + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData($"How many spaces did you manage to move my {caravanRef} trade caravan in total?",
                   options);
        }

        /// <summary>
        /// Take in the number of spaces the user managed to move a trade caravan
        /// Update move state with how many culture tokens were used to facilitate placements
        /// Update move state with how many culture tokens were recieved due to unused moves
        /// </summary>
        /// <param name="input">The number of control tokens placed next to cities</param>
        public override void ProcessActionResponse(string input)
        {
            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[_botMoveStateService.CurrentCaravanIdToMove - 1];
            movingCaravan.CaravanSpacesMoved = Convert.ToInt32(input);

            var economyTokensUsedThisTurn = movingCaravan.CaravanSpacesMoved - _botMoveStateService.BaseCaravanMoves;
            movingCaravan.EconomyTokensUsedThisTurn = economyTokensUsedThisTurn;
            if (_botMoveStateService.CurrentCaravanIdToMove == 1 || economyTokensUsedThisTurn > 0)
            {
                // only the first trade caravan should give tokens for unused moves
                _botMoveStateService.TradeTokensAvailable[FocusType.Economy] -= economyTokensUsedThisTurn;
            }
        }
    }
}
