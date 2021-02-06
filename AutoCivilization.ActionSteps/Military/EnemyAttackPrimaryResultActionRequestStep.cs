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
        private int _diceRollResult = 1;

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
            _diceRollResult = _randomService.Next(5) + 1;
            _computedBotPower = moveState.BaseAttackPower + _diceRollResult +
                                   moveState.CityStatesDiplomacyCardsHeld.Where(x => x.Type == FocusType.Military).Count() +
                                   moveState.BotPurchasedWonders.Where(x => x.Type == FocusType.Military).Count() +
                                   moveState.TradeTokensAvailable[FocusType.Military];

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"Currently the scales of battle are as follows;\nBot\t\t:\tAttacker\t:\tPower => {_computedBotPower}\nTarget\t\t:\tDefender\t:\tPower => {attckMove.AttackTargetPowerWithoutTradeTokens}",
                   new List<string>() {  });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var botPowerLessTokens = _computedBotPower - moveState.TradeTokensAvailable[FocusType.Military];
            var botTokensRequired = Math.Max(0, (attckMove.AttackTargetPowerWithoutTradeTokens + 1) - botPowerLessTokens);

            attckMove.DiceRollAttackPower = _diceRollResult;
            attckMove.ComputedBotAttackPowerForTurn = _computedBotPower;
            attckMove.BotIsWinning = _computedBotPower > attckMove.AttackTargetPowerWithoutTradeTokens;

            if (attckMove.BotIsWinning) attckMove.BotSpentMilitaryTradeTokensThisTurn = botTokensRequired;
        }
    }
}
