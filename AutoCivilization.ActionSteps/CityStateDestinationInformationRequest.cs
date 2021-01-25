using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using AutoCivilization.StateManagement;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class CityStateDestinationInformationRequestStep : StepActionBase, ICityStateDestinationInformationRequestStep
    {
        private readonly IGlobalGameCache _globalGameCache;
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        private const int BaseTradeTokensForCityStateVisit = 2;

        public CityStateDestinationInformationRequestStep(IGlobalGameCache globalGameCache,
                                                          IOrdinalSuffixResolver ordinalSuffixResolver) : base()
        {
            OperationType = OperationType.InformationRequest;

            _globalGameCache = globalGameCache;
            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override bool ShouldExecuteAction(BotMoveStateCache moveState)
        {
            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
            if (movingCaravan.CaravanDestinationType == CaravanDestinationType.CityState) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            // TODO: this lists all avilable city states - would be better if we can limit this list:
            //       to remove already visited city states
            //       to remove city states that are not in the current game

            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            var cityStates = _globalGameCache.CityStates.Select(x => $"{x.Id}. {x.Name}").ToList();
            return new MoveStepActionData($"Which city state did my {caravanRef} trade caravan arrive at?",
                   cityStates);
        }

        /// <summary>
        /// Update move state with visited city states
        /// </summary>
        /// <param name="input">The code for the city states visited specified by the user</param>
        public override BotMoveStateCache ProcessActionResponse(string input, BotMoveStateCache moveState)
        {
            var updatedMoveState = moveState.Clone();
            var selectedid = Convert.ToInt32(input);
            var citystate = _globalGameCache.CityStates.First(x => x.Id == selectedid);
            var movingCaravan = updatedMoveState.TradeCaravansAvailable[updatedMoveState.CurrentCaravanIdToMove - 1];
            movingCaravan.CaravanCityStateDestination = citystate;
            updatedMoveState.TradeTokensAvailable[citystate.Type] += BaseTradeTokensForCityStateVisit;
            return updatedMoveState;
        }
    }
}
