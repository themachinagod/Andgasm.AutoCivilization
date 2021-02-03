using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class SupplementAttackPowerInformationRequestStep : StepActionBase, ISupplementAttackPowerInformationRequestStep
    {
        private const int MaxTradeTokensHeld = 3;
        private int _powerDelta = 0;

        public SupplementAttackPowerInformationRequestStep() : base ()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            _powerDelta = attckMove.ComputedBotAttackPowerForTurn - attckMove.AttackTargetPowerWithoutTradeTokens;

            var autoWin = _powerDelta <= MaxTradeTokensHeld;
            var typeHoldsTokens = attckMove.AttackTargetType == AttackTargetType.RivalCapitalCity || attckMove.AttackTargetType == AttackTargetType.RivalCity || attckMove.AttackTargetType == AttackTargetType.RivalControlToken;
            return attckMove.IsTargetWithinRange && attckMove.BotIsWinning && autoWin && typeHoldsTokens;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            return new MoveStepActionData($"Does the defending target have {_powerDelta} military trade tokens available to spend in order to steal victory from the jaws of defeat?",
                   new List<string>() { "1. Yes", "2. No" });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var sufficientTradeTokensToWin = input == "1";
            if (sufficientTradeTokensToWin)
            {
                // target wins by virtue of spent trade tokens
                // bot spends none
                // target spends whats needed to tie (as defender wins)
                var requiredTokens = _powerDelta - moveState.TradeTokensAvailable[FocusType.Military];
                attckMove.TargetSpentMilitaryTradeTokensThisTurn = requiredTokens;
                attckMove.BotSpentMilitaryTradeTokensThisTurn = 0;
                attckMove.BotIsWinning = false;
            }
            else
            {
                // bot wins
                // target spends no trade tokens
                // bot spends only what is needed to gain a +1 victory
                var botPowerLessTokens = attckMove.ComputedBotAttackPowerForTurn - moveState.TradeTokensAvailable[FocusType.Military];
                var botTokensRequired = botPowerLessTokens - (attckMove.AttackTargetPowerWithoutTradeTokens + 1);
                attckMove.TargetSpentMilitaryTradeTokensThisTurn = 0;
                attckMove.BotSpentMilitaryTradeTokensThisTurn = botTokensRequired;
                attckMove.BotIsWinning = true;
            }
        }
    }
}
