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
        public override void UpdateMoveStateForUserResponse(string input, BotMoveState moveState)
        {
            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
            movingCaravan.CaravanSpacesMoved = Convert.ToInt32(input);

            var economyTokensUsedThisTurn = movingCaravan.CaravanSpacesMoved - moveState.BaseCaravanMoves;
            movingCaravan.EconomyTokensUsedThisTurn = economyTokensUsedThisTurn;
            if (economyTokensUsedThisTurn > 0)
            {
                // we should not gain tokens for unspent moves however the rules technically state:
                // —The AP only spends trade tokens
                //  from this card if doing so allows at least one of its caravans
                //  to reach its destination when that caravan would not reach
                //  the destination without the token
                // currently we will always use tokens if they are available 
                // above rule cannot be satisfied witout a further information request to the user
                moveState.TradeTokensAvailable[FocusType.Economy] -= economyTokensUsedThisTurn;
            }
        }
    }
}
