using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class CityStateDestinationInformationRequestStep : StepActionBase, ICityStateDestinationInformationRequestStep
    {
        private readonly IGlobalGameCache _globalGameCache;
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public CityStateDestinationInformationRequestStep(IGlobalGameCache globalGameCache,
                                                          IOrdinalSuffixResolver ordinalSuffixResolver,
                                                          IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;

            _globalGameCache = globalGameCache;
            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override bool ShouldExecuteAction()
        {
            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[_botMoveStateService.CurrentCaravanIdToMove - 1];
            if (movingCaravan.CaravanDestinationType == CaravanDestinationType.CityState) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction()
        {
            // TODO: this lists all avilable city states - would be better if we can limit this list:
            //       to remove already visited city states
            //       to remove city states that are not in the current game

            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(_botMoveStateService.CurrentCaravanIdToMove);
            var cityStates = _globalGameCache.CityStates.Select(x => $"{x.Id}. {x.Name}").ToList();
            return new MoveStepActionData($"Which city state did my {caravanRef} trade caravan arrive at?",
                   cityStates);
        }

        public override void ProcessActionResponse(string input)
        {

            var selectedid = Convert.ToInt32(input);
            var citystate = _globalGameCache.CityStates.First(x => x.Id == selectedid);
            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[_botMoveStateService.CurrentCaravanIdToMove - 1];
            movingCaravan.CaravanCityStateDestination = citystate;
            _botMoveStateService.TradeTokensAvailable[citystate.Type] += 2;
        }
    }
}
