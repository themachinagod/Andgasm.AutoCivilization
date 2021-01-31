using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
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

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            var tradeTokensAtStartOfTurn = moveState.TradeTokensAvailable[FocusType.Economy] - moveState.TradeCaravansAvailable.Sum(x => x.Value.EconomyTokensUsedThisTurn);
            var maxMoves = moveState.BaseCaravanMoves + tradeTokensAtStartOfTurn;
            var options = Array.ConvertAll(Enumerable.Range(0, maxMoves + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData($"How many spaces did you manage to move my {caravanRef} trade caravan in total?",
                   options);
        }

        /// <summary>
        /// Take in the number of spaces the user managed to move a trade caravan
        /// Update move state with how many culture tokens were used to facilitate placements
        /// </summary>
        /// <param name="input">The number of control tokens placed next to cities</param>
        /// <param name="moveState">The current move state to work from</param>
        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
            movingCaravan.CaravanSpacesMoved = Convert.ToInt32(input);

            var economyTokensUsedThisTurn = Math.Max(0, movingCaravan.CaravanSpacesMoved - moveState.BaseCaravanMoves);
            movingCaravan.EconomyTokensUsedThisTurn = economyTokensUsedThisTurn;    
            moveState.TradeTokensAvailable[FocusType.Economy] -= economyTokensUsedThisTurn;
        }
    }
}
