using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class NukePlayerCityFocusCardActionRequestStep : StepActionBase, INukePlayerCityFocusCardActionRequestStep
    {
        public NukePlayerCityFocusCardActionRequestStep() : base()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            return new MoveStepActionData("Each human player should pick a city that is not their capital, and remove the city as well as all adjacent control tokens from the board.",
                   new List<string>());
        }
    }
}
