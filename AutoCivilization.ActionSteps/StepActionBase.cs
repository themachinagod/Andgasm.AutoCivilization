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

        public virtual bool ShouldExecuteAction(BotMoveState moveState)
        {
            return true;
        }

        public abstract MoveStepActionData ExecuteAction(BotMoveState moveState);

        public virtual void UpdateMoveStateForUserResponse(string input, BotMoveState moveState)
        {
        }
    }
}
