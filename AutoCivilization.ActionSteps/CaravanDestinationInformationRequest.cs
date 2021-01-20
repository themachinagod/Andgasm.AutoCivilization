using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class CaravanDestinationInformationRequestStep : StepActionBase, ICaravanDestinationInformationRequestStep
    {
        public CaravanDestinationInformationRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction()
        {
            return new MoveStepActionData("Which type of destination was arrived at?",
                   new List<string>() { "1. On Route", "2. City State", "3. Rival City" });
        }

        public override void ProcessActionResponse(string input)
        {
            switch (Convert.ToInt32(input))
            {
                case 1:
                    _botMoveStateService.CaravanDestinationType = CaravanDestinationType.OnRoute;
                    break;
                case 2:
                    _botMoveStateService.CaravanDestinationType = CaravanDestinationType.CityState;
                    break;
                case 3:
                    _botMoveStateService.CaravanDestinationType = CaravanDestinationType.RivalCity;
                    break;
                default:
                    _botMoveStateService.CaravanDestinationType = CaravanDestinationType.OnRoute;
                    break;
            }
        }
    }
}
