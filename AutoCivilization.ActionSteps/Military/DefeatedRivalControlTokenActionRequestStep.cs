using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class DefeatedRivalControlTokenActionRequestStep : StepActionBase, IDefeatedRivalControlTokenActionRequestStep
    {
        public DefeatedRivalControlTokenActionRequestStep() : base ()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.RivalControlToken;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: this prompt is a hack - doesnt belong here!!

            var prompt = (moveState.CurrentAttackMoveId == moveState.AttacksAvailable.Count) ? "Press any key to proceed to my defensive reinforcements instructions" : "Press any key to advance to my next attack...";
            return new MoveStepActionData($"My attack on the rival control token was successful and my territory has grown, please replace the rival control token on the game board with one of my own from the supply",
                   new List<string>() { prompt });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.CityControlTokensPlacedThisTurn++;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
