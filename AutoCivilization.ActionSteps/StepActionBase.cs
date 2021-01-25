using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public abstract class StepActionBase : IStepAction
    {
        public int StepIndex { get; set; }
        public OperationType OperationType { get; set; }

        public StepActionBase()
        {
        }

        public virtual bool ShouldExecuteAction(BotMoveStateCache moveState)
        {
            return true;
        }

        public abstract MoveStepActionData ExecuteAction(BotMoveStateCache moveState);

        public virtual BotMoveStateCache ProcessActionResponse(string input, BotMoveStateCache moveState)
        {
            return moveState;
        }
    }
}
