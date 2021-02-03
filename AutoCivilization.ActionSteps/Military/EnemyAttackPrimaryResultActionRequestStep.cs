using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class EnemyAttackPrimaryResultActionRequestStep : StepActionBase, IAttackPrimaryResultActionRequestStep
    {
        private readonly Random _randomService = new Random();

        private int _computedBotPower = 0;

        public EnemyAttackPrimaryResultActionRequestStep() : base ()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.IsTargetWithinRange;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var dieRoll = _randomService.Next(5) + 1;
            _computedBotPower = moveState.BaseAttackPower + dieRoll +
                                   moveState.CityStatesDiplomacyCardsHeld.Where(x => x.Type == FocusType.Military).Count() +
                                   moveState.PurchasedWonders.Where(x => x.Type == FocusType.Military).Count() +
                                   moveState.TradeTokensAvailable[FocusType.Military];

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"Currently the scales of battle are as follows;\nBot\t\t:\tAttacker\t:\tPower => {_computedBotPower}\nTarget\t\t:\tDefender\t:\tPower => {attckMove.AttackTargetPowerWithoutTradeTokens}",
                   new List<string>() { "Press any key to proceed to the final stage of battle..." });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            attckMove.ComputedBotAttackPowerForTurn = _computedBotPower;
            attckMove.BotIsWinning = _computedBotPower > attckMove.AttackTargetPowerWithoutTradeTokens;
        }
    }
}
