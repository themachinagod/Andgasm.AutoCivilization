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
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"My attack on the {attckMove.AttackTargetType} was successful, please remove the barbarian target from the board.",
                   new List<string>() { });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.TradeTokensAvailable[FocusType.Military] += 2;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
