using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class NoActionStep : StepActionBase, INoActionStep
    {
        public NoActionStep() : base()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            return new MoveStepActionData("There are no physical actions I require from you for this move, nor are there any questions I need to ask at this time.",
                   new List<string>());
        }
    }
}
