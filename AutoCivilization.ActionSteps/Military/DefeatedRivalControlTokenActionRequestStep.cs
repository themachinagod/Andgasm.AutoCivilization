using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
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
            return new MoveStepActionData($"My attack on the rival control token was successful and my territory has grown, please replace the rival control token on the game board with one of my own from the supply.\nTell me, did this mighty battle take place on a natural wonder space?",
                   new List<string>() { "1. Yes", "2. No" });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var stolenNaturalWonder = input == "1";
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            attckMove.HasStolenNaturalWonder = stolenNaturalWonder;
            moveState.CityControlTokensPlacedThisTurn++;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
