using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.MiscResolvers;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AutoCivilization.Console
{
    public interface IAutoCivRoundClient
    {
        void ExecuteRoundForBot(BotGameStateCache gameState);
    }

    public class AutoCivRoundClient : IAutoCivRoundClient
    {
        private readonly IFocusCardResolverFactory _focusCardResolverFactory;
        private readonly IFocusBarEndOfRoundResolver _focusBarEndOfMoveResolver;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBotRoundStateCache _botRoundState;

        public AutoCivRoundClient(IFocusCardResolverFactory focusCardResolverFactory,
                                  IFocusBarEndOfRoundResolver focusBarEndOfMoveResolver,
                                  IBotRoundStateCache botRoundState,
                                  IServiceScopeFactory serviceScopeFactory)
        {
            _botRoundState = botRoundState;
            _focusCardResolverFactory = focusCardResolverFactory;
            _focusBarEndOfMoveResolver = focusBarEndOfMoveResolver;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void ExecuteRoundForBot(BotGameStateCache gameState)
        {
            // TODO: we can track the event dial which should move at the start of each of the bots turns (except first)
            //       certain events will require actions - trade tokens etc

            // TODO: human players require the ability to interact with the bot duering their turns
            //       this will happen after the bot has moved but before the round has completed
            //       currently we await any key to continue
            //       instead we should prompt if any player would like to interact with it
            //       options provided should be
            //          - I wish to attach one of your cities
            //          - no, please proceed to the next round

            WriteConsoleRoundHeader(gameState);
            ExecutePrimaryMove(gameState);
            ExecuteSubMoves(gameState);
            ResetFocusBarForNextMove(gameState);
            WriteConsoleAwaitingNextTurn();
            gameState.CurrentRoundNumber++;
        }

        private void ExecutePrimaryMove(BotGameStateCache gameState)
        {
            using (var primaryMoveScope = _serviceScopeFactory.CreateScope())
            {
                var focusCardToExecute = gameState.ActiveFocusBar.ActiveFocusSlot;
                ExecuteMoveForScope(primaryMoveScope, gameState, focusCardToExecute);
            }
        }

        private void ExecuteSubMoves(BotGameStateCache gameState)
        {
            if (_botRoundState.ShouldExecuteAdditionalFocusCard)
            {
                System.Console.ReadKey();
                using (var subMoveScope = _serviceScopeFactory.CreateScope())
                {
                    var focusCardToExecute = gameState.ActiveFocusBar.ActiveFocusSlots.First(x => x.Value.Type == _botRoundState.AdditionalFocusTypeToExecuteOnFocusBar).Value;
                    ExecuteMoveForScope(subMoveScope, gameState, focusCardToExecute);
                }
            }
        }

        private void ExecuteMoveForScope(IServiceScope scope, BotGameStateCache gameState, FocusCardModel focusCard)
        {
            var scopedMoveContext = scope.ServiceProvider.GetRequiredService<IAutoCivMoveClient>();
            var focusCardMoveResolver = _focusCardResolverFactory.GetFocusCardMoveResolver(focusCard);
            scopedMoveContext.ExecuteMoveForResolver(gameState, focusCardMoveResolver);
        }

        private void ResetFocusBarForNextMove(BotGameStateCache gameState)
        {
            gameState.ActiveFocusBar = _focusBarEndOfMoveResolver.ResetFocusBarForNextMove(gameState.ActiveFocusBar);
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
}
