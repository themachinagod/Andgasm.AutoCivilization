using System.Collections.Generic;

namespace AutoCivilization.Abstractions.ActionSteps
{
    public interface IStepAction
    {
        int StepIndex { get; set; }
        OperationType OperationType { get; set; }

        bool ShouldExecuteAction();
        (string Message, IReadOnlyCollection<string> ResponseOptions) ExecuteAction();
        void ProcessActionResponse(string input);
    }

    public interface INoActionRequest : IStepAction 
    {
    }
}
