using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.Abstractions.ActionSteps
{
    public enum OperationType
    {
        ActionRequest,
        InformationRequest
    }

    public interface IStepAction
    {
        int StepIndex { get; set; }
        OperationType OperationType { get; set; }

        bool ShouldExecuteAction(BotMoveStateCache moveState);
        MoveStepActionData ExecuteAction(BotMoveStateCache moveState);
        BotMoveStateCache ProcessActionResponse(string input, BotMoveStateCache moveState);
    }

    public interface INoActionStep : IStepAction 
    {
    }
}
