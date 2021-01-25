using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using AutoCivilization.StateManagement;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class CaravanMovementInformationRequestStep : StepActionBase, ICaravanMovementInformationRequestStep
    {
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public CaravanMovementInformationRequestStep(IOrdinalSuffixResolver ordinalSuffixResolver) : base()
        {
            OperationType = OperationType.InformationRequest;

            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            var maxMoves = moveState.BaseCaravanMoves + moveState.TradeTokensAvailable[FocusType.Economy];
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
        /// <param name="moveState">The current move state to work from</param>
        public override BotMoveStateCache ProcessActionResponse(string input, BotMoveStateCache moveState)
        {
            var updatedMoveState = moveState.Clone();
            var movingCaravan = updatedMoveState.TradeCaravansAvailable[updatedMoveState.CurrentCaravanIdToMove - 1];
            movingCaravan.CaravanSpacesMoved = Convert.ToInt32(input);

            var economyTokensUsedThisTurn = movingCaravan.CaravanSpacesMoved - updatedMoveState.BaseCaravanMoves;
            movingCaravan.EconomyTokensUsedThisTurn = economyTokensUsedThisTurn;
            if (updatedMoveState.CurrentCaravanIdToMove == 1 || economyTokensUsedThisTurn > 0)
            {
                // only the first trade caravan should give tokens for unused moves
                updatedMoveState.TradeTokensAvailable[FocusType.Economy] -= economyTokensUsedThisTurn;
            }
            return updatedMoveState;
        }
    }
}
