using AutoCivilization.Abstractions.ActionSteps;
using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    /// <summary>
    /// Card move resolution resolver
    /// Configuration of individual steps for each move at construction
    /// Initialisation of the move state 
    /// Traversal of configured steps
    /// Resolution of game state after all steps executed
    /// </summary>
    public interface IFocusCardMoveResolver
    {
        FocusType FocusType { get; set; }
        FocusLevel FocusLevel { get; set; }
        bool HasMoreSteps { get; }

        /// <summary>
        /// Initialise the move state as required for the resolving of the card
        /// </summary>
        /// <param name="botGameStateService">The game state as per start of the move</param>
        void PrimeMoveState(BotGameStateCache botGameStateService);

        /// <summary>
        /// Initaite the next step if applicable for move state 
        /// Retreive data payload for step
        /// </summary>
        /// <returns>The next steps data payload: Action Request & Information Request data</returns>
        MoveStepActionData ProcessMoveStepRequest();

        /// <summary>
        /// Process the user input for the current action step
        /// </summary>
        /// <param name="response">The user input</param>
        void ProcessMoveStepResponse(string response);

        /// <summary>
        /// Resolve the updated game state from the current move state
        /// </summary>
        /// <param name="botGameStateService">The game state to update for move</param>
        /// <returns>A textual summary of what the bot did this move</returns>
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
