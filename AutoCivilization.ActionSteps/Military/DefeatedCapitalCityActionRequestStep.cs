using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class DefeatedCapitalCityActionRequestStep : StepActionBase, IDefeatedCapitalCityActionRequestStep
    {
        public DefeatedCapitalCityActionRequestStep() : base ()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.RivalCapitalCity;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: we need to consider the fact that the city may contain a world wonderr
            //       if this is the case then we need to transfer the world wonder to the new owner both in game state and on the board (as the city will not be replaced)
            //       we would needd to ask the user if and what world wonder exists on the vanquished city!

            // TODO: diplomacy cards impact??

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"My attack on the {attckMove.AttackTargetType} was successful, as this is a capital city DO NOT replace the city on the board.",
                   new List<string>() { });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.TradeTokensAvailable[moveState.SmallestTradeTokenPileType] += 2;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
