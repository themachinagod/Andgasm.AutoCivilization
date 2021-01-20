using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class CityStateDestinationInformationRequestStep : StepActionBase, ICityStateDestinationInformationRequestStep
    {
        public CityStateDestinationInformationRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction()
        {
            if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.CityState) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction()
        {
            // TODO: we need a list of city states that are active in this game
            //       currently hard wired for two player game!

            var cityStates = new List<string> { "1. Brussels", "2. Carthage", "3. Buenos Ares" };
            return new MoveStepActionData("Which city state was arrived at?",
                   cityStates);
        }

        public override void ProcessActionResponse(string input)
        {
            // TODO: just using string to start with - these city states will need to be modelled as they will have associated properties!

            switch (Convert.ToInt32(input))
            {
                case 1:
                    _botMoveStateService.CaravanCityStateDestination = "Brussels";
                    break;
                case 2:
                    _botMoveStateService.CaravanCityStateDestination = "Carthage";
                    break;
                case 3:
                    _botMoveStateService.CaravanCityStateDestination = "Buenos Ares";
                    break;
                default:
                    _botMoveStateService.CaravanCityStateDestination = "Brussels";
                    break;
            }
        }
    }
}
