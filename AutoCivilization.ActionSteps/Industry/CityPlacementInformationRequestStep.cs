using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class CityPlacementInformationRequestStep : StepActionBase, ICityPlacementInformationRequestStep
    {
        public CityPlacementInformationRequestStep() : base ()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            return new MoveStepActionData("Did you manage to place my new city on the board legally?",
                   new List<string>() { "1. Yes", "2. No" });
        }

        /// <summary>
        /// Take in the number of control tokens the user managed to place on the board next to bot cities
        /// Update move state with how many culture tokens were used to facilitate placements
        /// Update move state with how many culture tokens were recieved due to unused placements
        /// Update move state control tokens placed counter
        /// </summary>
        /// <param name="input">The number of control tokens placed next to cities</param>
        /// <param name="moveState">The current move state to work from</param>
        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            bool placedCity = input == "1";
            moveState.HasPurchasedCityThisTurn = placedCity;
            if (placedCity) moveState.FriendlyCitiesAddedThisTurn = 1;
        }
    }
}
