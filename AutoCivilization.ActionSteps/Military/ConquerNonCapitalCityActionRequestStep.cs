using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class ConquerNonCapitalCityActionRequestStep : StepActionBase, IConquerNonCapitalCityActionRequestStep
    {
        public ConquerNonCapitalCityActionRequestStep() : base ()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.RivalCity;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: we need to consider the fact that the city may contain a world wonderr
            //       if this is the case then we need to transfer the world wonder to the new owner both in game state
            //       we would needd to ask the user if and what world wonder exists on the vanquished city!

            // TODO: we need to consider the fact that the city COULD have once been a city state that was conquered by a rival prior to this attack
            //       if this is the case we would need to conquer the city state which means transferring the city state token to bot in gamestate and physical board
            //       we would need to ask the user if this city was prev a city state at any point

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"My attack on the {attckMove.AttackTargetType} was successful, please replace the conquered city target with one of my own cities from the supply.",
                   new List<string>() { });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.FriendlyCitiesAddedThisTurn++;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
