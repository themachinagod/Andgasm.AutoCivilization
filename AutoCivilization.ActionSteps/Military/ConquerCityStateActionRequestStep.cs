using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class ConquerCityStateInformationRequestStep : StepActionBase, IConquerCityStateInformationRequestStep
    {
        private readonly IGlobalGameCache _globalGameCache;

        public ConquerCityStateInformationRequestStep(IGlobalGameCache globalGameCache) : base ()
        {
            OperationType = OperationType.InformationRequest;

            _globalGameCache = globalGameCache;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.CityState;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: i think instrad of asking question here - we follow pattern for wonders and ask in a prev step
            //       this way we can prompt next attack and summerise single rsult here

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var cityStates = _globalGameCache.CityStates.Select(x => $"{x.Id}. {x.Name}").ToList();
            return new MoveStepActionData($"My attack on the city state was successful, please take the following physical steps;\nReplace the conquered city state with one of my own cities from the supply\nRemove the city state token from the board and place it next to my leadersheet\nAll players MUST return diplomacy cards held for the defeated city state to the side of the board.\nNow tell me... what is the name of this city state I vanquished?",
                   cityStates);
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            // TODO: how does the bot lose the city state token after gaining it??
            //       this can only happen when the city state is liberated or conquered by another player
            //       we would have to process this on an attack by a rival 
            //       during the round - a rival will initiate combat with bot - they will inform the bot that they are attacking a city state it conquered (specifically)
            //       if the rival wins that battle - they will either decide to liberate it - or conquer it
            //       either way the bot will have to give up the city state token either to the board or the rival player
            //       at this point the gamestate will be updated to remove the city from the conquered city states

            var selectedid = Convert.ToInt32(input);
            var citystate = _globalGameCache.CityStates.First(x => x.Id == selectedid);

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            attckMove.TargetCityState = citystate;

            moveState.CityStatesDiplomacyCardsHeld.Remove(citystate);
            moveState.ConqueredCityStateTokensHeld.Add(citystate);
            moveState.FriendlyCitiesAddedThisTurn++;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
