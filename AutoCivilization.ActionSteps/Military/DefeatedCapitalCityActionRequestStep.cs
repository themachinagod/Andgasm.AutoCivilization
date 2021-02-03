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
            // TODO: we need to consider the fact that the city may contain a world wonder
            //       if this is the case then we need to transfer the world wonder to the new owner in both game state and the physical board
            //       this will require a question to the user - what world wonder is associated with this city?
            //       this step will only be applicable if the target is a rival city or rival capital city
            //       in the future this will also only be applicable if the defending rival has purchase any wonders (we would know this if we asked who we were fighting for rival tokens, cities & also if we asked which user bought a specific wonder at round end time)
            //       if so - we will add the world wonder to the bots move state and ask the defending rival to put the wonder card next to the bots player sheet
            //       if it is a capital city we will also ask the defending rival to place the wonder token under the bots strongest free city
            //       if the bot does not have a city without a wonder - we will instead inform the user to remove the wonder token and cards from the game

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
