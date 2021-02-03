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
            // TODO: we need to consider the fact that the control token may be on a natural wonder
            //       if this is the case then we need to transfer the token to the new owner both in game state and on physical board!

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"My attack on the {attckMove.AttackTargetType} was successful, please replace the control token target with one of my own control tokens.",
                   new List<string>() { });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.CityControlTokensPlacedThisTurn++;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
