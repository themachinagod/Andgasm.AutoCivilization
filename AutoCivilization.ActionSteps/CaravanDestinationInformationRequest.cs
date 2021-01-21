using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class CaravanDestinationInformationRequestStep : StepActionBase, ICaravanDestinationInformationRequestStep
    {
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public CaravanDestinationInformationRequestStep(IBotMoveStateCache botMoveStateService,
                                                        IOrdinalSuffixResolver ordinalSuffixResolver) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;

            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override MoveStepActionData ExecuteAction()
        {
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(_botMoveStateService.CurrentCaravanIdToMove);
            return new MoveStepActionData($"Which type of destination did my {caravanRef} trade caravan arrive at?",
                   new List<string>() { "1. On Route", "2. City State", "3. Rival City" });
        }

        public override void ProcessActionResponse(string input)
        {
            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[_botMoveStateService.CurrentCaravanIdToMove - 1];
            switch (Convert.ToInt32(input))
            {
                case 1:
                    movingCaravan.CaravanDestinationType = CaravanDestinationType.OnRoute;
                    break;
                case 2:
                    movingCaravan.CaravanDestinationType = CaravanDestinationType.CityState;
                    break;
                case 3:
                    movingCaravan.CaravanDestinationType = CaravanDestinationType.RivalCity;
                    break;
                default:
                    movingCaravan.CaravanDestinationType = CaravanDestinationType.OnRoute;
                    break;
            }
        }
    }
}
