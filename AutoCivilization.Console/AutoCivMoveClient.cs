using AutoCivilization.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Console
{
    public interface IAutoCivMoveClient
    {
        void ExecuteMoveForResolver(BotGameState gameState, IFocusCardMoveResolver moveresolver);
    }

    public class AutoCivMoveClient : IAutoCivMoveClient
    {
        public void ExecuteMoveForResolver(BotGameState gameState, IFocusCardMoveResolver focusCardMoveResolver)
        {
            focusCardMoveResolver.PrimeMoveState(gameState);
            do
            {
                if (focusCardMoveResolver.HasMoreSteps)
                {
                    var nextMoveData = focusCardMoveResolver.ProcessMoveStepRequest();
                    if (nextMoveData != null) // DBr: this smells - we return null on should excute and then have to do null check here!!
                    {
                        PromptUser(nextMoveData.Message);

                        var response = TakeUserInput(nextMoveData.ResponseOptions.ToList());
                        focusCardMoveResolver.ProcessMoveStepResponse(response);
                    }
                }
            } while (focusCardMoveResolver.HasMoreSteps);

            var moveSummary = focusCardMoveResolver.UpdateGameStateForMove(gameState);
            WriteConsoleMoveSummary(moveSummary);
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
    }
}
