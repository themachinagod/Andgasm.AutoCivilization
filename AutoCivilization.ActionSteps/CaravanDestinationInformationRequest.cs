using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using AutoCivilization.StateManagement;
using System;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class CaravanDestinationInformationRequestStep : StepActionBase, ICaravanDestinationInformationRequestStep
    {
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        public CaravanDestinationInformationRequestStep(IOrdinalSuffixResolver ordinalSuffixResolver) : base()
        {
            OperationType = OperationType.InformationRequest;

            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            return new MoveStepActionData($"Which type of destination did my {caravanRef} trade caravan arrive at?",
                   new List<string>() { "1. On Route", "2. City State", "3. Rival City" });
        }

        /// <summary>
        /// Update move state for a caravan to show what its destination result was
        /// </summary>
        /// <param name="input">The code for the destination result specified by the user</param>
        /// <param name="moveState">The current move state to work from</param>
        public override void UpdateMoveStateForUserResponse(string input, BotMoveState moveState)
        {
            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
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
