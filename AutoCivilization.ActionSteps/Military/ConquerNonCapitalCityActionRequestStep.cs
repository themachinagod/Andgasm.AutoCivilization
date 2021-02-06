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
            // TODO: diplomacy cards impact??
            //       when the bot attacks a rival city - any cards held by the bot for that city need to be returned!

            // TODO: we need to consider the fact that the city COULD have once been a city state that was conquered by a rival prior to this attack
            //       if this is the case we would need to conquer the city state which means transferring the city state token to bot in gamestate and physical board
            //       we would need to ask the user if this city was prev a city state at any point (and if so what its name is)

            // TODO: this prompt is a hack - doesnt belong here!!

            var prompt = (moveState.CurrentAttackMoveId == moveState.AttacksAvailable.Count) ? "Press any key to proceed to my defensive reinforcements instructions" : "Press any key to advance to my next attack...";
            return new MoveStepActionData($"My attack on the rival city was a resounding success and the city is now under my control\nPlease replace the conquered city target with one of my own cities from the supply",
                   new List<string>() { prompt });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.FriendlyCitiesAddedThisTurn++;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
