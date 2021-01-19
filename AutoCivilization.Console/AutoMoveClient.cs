using AutoCivilization.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Console
{
    public interface IAutoCivMoveClient
    {
        void ExecuteMoveForActiveFocusCard(BotGameStateCache gameState);
    }

    public class AutoCivMoveClient : IAutoCivMoveClient
    {
        private readonly IFocusCardResolverFactory _focusCardResolverFactory;

        public AutoCivMoveClient(IFocusCardResolverFactory focusCardResolverFactory)
        {
            _focusCardResolverFactory = focusCardResolverFactory;
        }

        public void ExecuteMoveForActiveFocusCard(BotGameStateCache gameState)
        {
            WriteConsoleRoundHeader(gameState);
            var focusCardToExecute = gameState.ActiveFocusBar.ActiveFocusSlot;
            var focusCardMoveResolver = _focusCardResolverFactory.GetFocusCardMoveResolver(focusCardToExecute);
            focusCardMoveResolver.PrimeMoveState(gameState);
            do
            {
                if (focusCardMoveResolver.HasMoreSteps)
                {
                    var nextMoveData = focusCardMoveResolver.ProcessMoveStepRequest();
                    PromptUser(nextMoveData.Message);

                    var response = TakeUserInput(nextMoveData.ResponseOptions.ToList());
                    focusCardMoveResolver.ProcessMoveStepResponse(response);
                }
            } while (focusCardMoveResolver.HasMoreSteps);

            var moveSummary = focusCardMoveResolver.UpdateGameStateForMove(gameState);
            WriteConsoleMoveSummary(moveSummary);

            // TODO: update focus bar - shift cards up by 1...

            WriteConsoleAwaitingNextTurn();
            gameState.CurrentRoundNumber++; 
        }

        private void PromptUser(string msg)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(msg);
        }

        private string TakeUserInput(List<string> options)
        {
            if (options.Count > 0)
            {
                foreach (var o in options) { System.Console.WriteLine(o); }
                return System.Console.ReadLine();
            }
            else
            {
                System.Console.WriteLine("Press any key to proceed...");
                System.Console.ReadKey();
                return string.Empty;
            }
        }

        private static void WriteConsoleMoveSummary(string summary)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(summary);
        }

        private static void WriteConsoleAwaitingNextTurn()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Now you guys go on ahead and take your shot, press any key when its time for me to move...");
            System.Console.ReadKey();
            System.Console.Clear();
        }

        private void WriteConsoleRoundHeader(BotGameStateCache gameState)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"# Game {gameState.GameId}                 #");
            System.Console.WriteLine($"# Round {gameState.CurrentRoundNumber}                   #");
            System.Console.WriteLine("#############################");
        }
    }
}
