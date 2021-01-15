using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public abstract class StepActionBase : IStepAction
    {
        internal readonly IBotMoveStateService _botMoveStateService;

        public int StepIndex { get; set; }
        public OperationType OperationType { get; set; }

        public StepActionBase(IBotMoveStateService botMoveStateService)
        {
            _botMoveStateService = botMoveStateService;
        }

        public virtual bool ShouldExecuteAction()
        {
            return true;
        }

        public abstract (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction();

        public virtual void ProcessActionResponse(string input)
        {
        }
    }
}
