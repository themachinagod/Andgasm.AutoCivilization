using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class DefeatedBarbarianActionRequestStep : StepActionBase, IDefeatedBarbarianActionRequestStep
    {
        public DefeatedBarbarianActionRequestStep() : base ()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.Barbarian;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: this prompt is a hack - doesnt belong here!!

            var prompt = (moveState.CurrentAttackMoveId == moveState.AttacksAvailable.Count) ? "Press any key to proceed to a summary of all my attacks" : "Press any key to advance to my next attack...";
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"My attack on the inferior {attckMove.AttackTargetType} was a complete success. As this barbarian unit has been vanquished, please remove it from the game board",
                   new List<string>() { prompt });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.TradeTokensAvailable[moveState.SmallestTradeTokenPileType] += 1;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
