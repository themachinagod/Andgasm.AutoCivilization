using AutoCivilization.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        IResolveFocusBarEndOfMoveResolver _focusBarEndOfMoveResolver;

        public AutoCivMoveClient(IFocusCardResolverFactory focusCardResolverFactory,
                                 IResolveFocusBarEndOfMoveResolver focusBarEndOfMoveResolver)
        {
            _focusCardResolverFactory = focusCardResolverFactory;
            _focusBarEndOfMoveResolver = focusBarEndOfMoveResolver;
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

            gameState.ActiveFocusBar = _focusBarEndOfMoveResolver.ResolveFocusBarForNextMove(gameState.ActiveFocusBar);
            // TODO: summarise new focus bar state

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
            System.Console.WriteLine($"Focus Bar Slot 1: {gameState.ActiveFocusBar.FocusSlot1.Name} ({gameState.ActiveFocusBar.FocusSlot1.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot1.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 2: {gameState.ActiveFocusBar.FocusSlot2.Name} ({gameState.ActiveFocusBar.FocusSlot2.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot2.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 3: {gameState.ActiveFocusBar.FocusSlot3.Name} ({gameState.ActiveFocusBar.FocusSlot3.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot3.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 4: {gameState.ActiveFocusBar.FocusSlot4.Name} ({gameState.ActiveFocusBar.FocusSlot4.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot4.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 5: {gameState.ActiveFocusBar.FocusSlot5.Name} ({gameState.ActiveFocusBar.FocusSlot5.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot5.Type]} tokens");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Active Move Focus: {gameState.ActiveFocusBar.FocusSlot5.Name}");
        }
    }

    public interface IResolveFocusBarEndOfMoveResolver
    {
        FocusBarModel ResolveFocusBarForNextMove(FocusBarModel activeFocusBar);
    }

    public class ResolveFocusBarEndOfMoveResolver : IResolveFocusBarEndOfMoveResolver
    {
        public FocusBarModel ResolveFocusBarForNextMove(FocusBarModel activeFocusBar)
        {
            // shift all cards up by 1 with slot resolved this turn being reset to first slot

            var tmpSlot2 = activeFocusBar.FocusSlot1;
            var newFocusBar = new Dictionary<int, FocusCardModel>();
            newFocusBar.Add(0, activeFocusBar.FocusSlot5);
            newFocusBar.Add(4, activeFocusBar.FocusSlot4);
            newFocusBar.Add(3, activeFocusBar.FocusSlot3);
            newFocusBar.Add(2, activeFocusBar.FocusSlot2);
            newFocusBar.Add(1, tmpSlot2);
            return new FocusBarModel(new ReadOnlyDictionary<int, FocusCardModel>(newFocusBar));
        }
    }

}
