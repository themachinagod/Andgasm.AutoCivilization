using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class ConquerCityStateActionRequestStep : StepActionBase, IConquerCityStateActionRequestStep
    {
        private readonly IGlobalGameCache _globalGameCache;

        public ConquerCityStateActionRequestStep(IGlobalGameCache globalGameCache) : base ()
        {
            OperationType = OperationType.ActionRequest;

            _globalGameCache = globalGameCache;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.CityState;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: we need to know which city state has been conquered in order to remove any diplomacy cards held
            //       perhaps turn this into info request?

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var cityStates = _globalGameCache.CityStates.Select(x => $"{x.Id}. {x.Name}").ToList();
            return new MoveStepActionData($"My attack on the city state was successful, please replace the conquered city state target with one of my own cities from the supply and place the city state token next to my leadersheet\n All players MUST return diplomacy cards held for the defeated city state to the side of the board.\nNow tell me... what is the name of this city state I vanquished?",
                   cityStates);
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            // TODO: process input as city state conquered
            //       remove city state from visited states (dip cards)
            // TODO: diplomacy cards impact - we need to remove diplomacy cards from the bot state (visitedcitystates)
            //       if so we need to remove the diplomacy card from collection

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            moveState.FriendlyCitiesAddedThisTurn++;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
