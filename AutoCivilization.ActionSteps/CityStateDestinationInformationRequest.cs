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

        public CityStateDestinationInformationRequestStep(IGlobalGameCache globalGameCache,
                                                          IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;

            _globalGameCache = globalGameCache;
        }

        public override bool ShouldExecuteAction()
        {
            if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.CityState) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction()
        {
            // TODO: this lists all avilable city states - would be better if we can limit this list:
            //       to remove already visited city states
            //       to remove city states that are not in the current game

            var cityStates = _globalGameCache.CityStates.Select(x => $"{x.Id}. {x.Name}").ToList();
            return new MoveStepActionData("Which city state was arrived at?",
                   cityStates);
        }

        public override void ProcessActionResponse(string input)
        {
            var selectedid = Convert.ToInt32(input);
            var citystate = _globalGameCache.CityStates.First(x => x.Id == selectedid);
            _botMoveStateService.CaravanCityStateDestination = citystate;
            _botMoveStateService.TradeTokensAvailable[citystate.Type] += 2;
        }
    }
}
