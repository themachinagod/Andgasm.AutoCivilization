using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    public interface IFocusCardMoveResolver
    {
        FocusType FocusType { get; set; }
        FocusLevel FocusLevel { get; set; }
        bool HasMoreSteps { get; }

        void PrimeMoveState(BotGameStateCache botGameStateService);
        MoveStepActionData ProcessMoveStepRequest();
        void ProcessMoveStepResponse(string response);
        string UpdateGameStateForMove(BotGameStateCache botGameStateService);
    }

    public class MoveStepActionData
    {
        public string Message { get; set; }
        public IReadOnlyCollection<string> ResponseOptions { get; set; }

        public MoveStepActionData(string message, IReadOnlyCollection<string> responseOpts)
        {
            Message = message;
            ResponseOptions = responseOpts;
        }
    }
}
