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
        (string Message, IReadOnlyCollection<string> ResponseOptions) ProcessMoveStepRequest();
        void ProcessMoveStepResponse(string response);
        string UpdateGameStateForMove(BotGameStateCache botGameStateService);
    }
}
