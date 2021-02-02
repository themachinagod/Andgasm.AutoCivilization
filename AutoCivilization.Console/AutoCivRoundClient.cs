using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.MiscResolvers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.Console
{
    public interface IAutoCivRoundClient
    {
        void ExecuteRoundForBot(BotGameState gameState);
    }

    public class AutoCivRoundClient : IAutoCivRoundClient
    {
        private readonly IFocusCardResolverFactory _focusCardResolverFactory;
        private readonly IFocusBarResetResolver _focusBarEndOfMoveResolver;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBotRoundStateCache _botRoundState;

        public AutoCivRoundClient(IFocusCardResolverFactory focusCardResolverFactory,
                                  IFocusBarResetResolver focusBarEndOfMoveResolver,
                                  IBotRoundStateCache botRoundState,
                                  IServiceScopeFactory serviceScopeFactory)
        {
            _botRoundState = botRoundState;
            _focusCardResolverFactory = focusCardResolverFactory;
            _focusBarEndOfMoveResolver = focusBarEndOfMoveResolver;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void ExecuteRoundForBot(BotGameState gameState)
        {
            // TODO: we can track the event dial which should move at the start of each of the bots turns (except first)
            //       certain events will require actions - trade tokens etc

            // TODO: human players require the ability to interact with the bot duering their turns
            //       this will happen after the bot has moved but before the round has completed
            //       currently we await any key to continue
            //       instead we should prompt if any player would like to interact with it
            //       options provided should be
            //          - I wish to attack one of the bot cities
            //          - I have purchased a wonder, please update the unlocked cards
            //          - no, please proceed to the bots next move

            // TODO: what if we get a chain of reruns and resets - not sure how this would propegate
            //       e.g. capitalism - play it reset it and draw economy as next active (primary)
            //            ubanization - play it reset it and draw culture as ruled (post -> will add new culture execute to round)
            //            culture - play it reset it (post -> will this get picked up in that same loop even tho added after execution - i think it will exception with changed collection tbh)
            //                      if it does not exception and does not fire in that loop - it will be missed!

            WriteConsoleRoundHeader(gameState);

            ExecutePrimaryMove(gameState);
            ExecuteSubMoveChains(gameState);

            WriteConsoleAwaitingNextTurn();
            gameState.CurrentRoundNumber++;
        }

        private void ExecutePrimaryMove(BotGameState gameState)
        {
            using (var primaryMoveScope = _serviceScopeFactory.CreateScope())
            {
                var focusCardToExecute = gameState.ActiveFocusBar.ActiveFocusSlot;
                ExecuteMoveForScope(primaryMoveScope, gameState, focusCardToExecute);
                ResetFocusBarForNextMove(gameState);
            }
        }

        private void ExecuteSubMoveChains(BotGameState gameState)
        {
            var unexecutedSubMoves = new List<SubMoveConfiguration>(_botRoundState.SubMoveConfigurations.Where(x => !x.HasCompletedExecution));
            if (unexecutedSubMoves.Count() == 0) return;

            foreach (var phasesubmove in unexecutedSubMoves.ToList())
            {
                System.Console.ReadKey();
                using (var subMoveScope = _serviceScopeFactory.CreateScope())
                {
                    var focusCardModel = gameState.ActiveFocusBar.ActiveFocusSlots.First(x => x.Value.Type == phasesubmove.AdditionalFocusTypeToExecuteOnFocusBar).Value;
                    ExecuteMoveForScope(subMoveScope, gameState, focusCardModel);
                    if (phasesubmove.ShouldResetSubFocusCard)
                    {
                        ResetFocusBarForSubMoveFocusType(gameState, focusCardModel.Type);
                    }
                }
            }
            ExecuteSubMoveChains(gameState);
        }

        private void ExecuteMoveForScope(IServiceScope scope, BotGameState gameState, FocusCardModel focusCard)
        {
            var scopedMoveContext = scope.ServiceProvider.GetRequiredService<IAutoCivMoveClient>();
            var focusCardMoveResolver = _focusCardResolverFactory.GetFocusCardMoveResolver(focusCard);
            scopedMoveContext.ExecuteMoveForResolver(gameState, focusCardMoveResolver);
        }

        private void ResetFocusBarForNextMove(BotGameState gameState)
        {
            gameState.ActiveFocusBar = _focusBarEndOfMoveResolver.ResetFocusBarForNextMove(gameState.ActiveFocusBar);
        }

        private void ResetFocusBarForSubMoveFocusType(BotGameState gameState, FocusType type)
        {
            gameState.ActiveFocusBar = _focusBarEndOfMoveResolver.ResetFocusBarForSubMove(gameState.ActiveFocusBar, type);
        }

        private static void WriteConsoleAwaitingNextTurn()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Now you guys go on ahead and take your shot, press any key when its time for me to move...");
            System.Console.ReadKey();
            System.Console.Clear();
        }

        private void WriteConsoleRoundHeader(BotGameState gameState)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Game {gameState.GameId}");
            System.Console.WriteLine($"Round {gameState.CurrentRoundNumber}");              
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Friendly City Count: {gameState.FriendlyCityCount}");
            System.Console.WriteLine($"Supported Trade Caravan Count: {gameState.SupportedCaravanCount}");
            System.Console.WriteLine($"Trade Caravans on Route Count: {gameState.CaravansOnRouteCount}");
            System.Console.WriteLine($"City State Diplomacy Cards: {string.Join(",", gameState.VisitedCityStates)}");
            System.Console.WriteLine($"Rival Diplomacy Cards: {string.Join(",", gameState.VisitedPlayerColors)}");
            System.Console.WriteLine($"Purchased World Wonders: { string.Join(",", gameState.PurchasedWonders.Select(x => x.Name))}");
            System.Console.WriteLine($"Controlled Natural Wonders: { string.Join(",", gameState.ControlledNaturalWonders)}");
            System.Console.WriteLine($"Controlled Natural Resource Count: {gameState.ControlledNaturalResources}");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Unlocked Culture Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Culture]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Culture]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Culture]?.Cost}");
            System.Console.WriteLine($"Unlocked Economy Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Economy]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Economy]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Economy]?.Cost}");
            System.Console.WriteLine($"Unlocked Science Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Science]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Science]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Science]?.Cost}");
            System.Console.WriteLine($"Unlocked Military Wonder: {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Military]?.Name} ({gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Military]?.Era}) : {gameState.WonderCardDecks.UnlockedWonderCards[FocusType.Military]?.Cost}");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Focus Bar Slot 1: {gameState.ActiveFocusBar.FocusSlot1.Name} ({gameState.ActiveFocusBar.FocusSlot1.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot1.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 2: {gameState.ActiveFocusBar.FocusSlot2.Name} ({gameState.ActiveFocusBar.FocusSlot2.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot2.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 3: {gameState.ActiveFocusBar.FocusSlot3.Name} ({gameState.ActiveFocusBar.FocusSlot3.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot3.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 4: {gameState.ActiveFocusBar.FocusSlot4.Name} ({gameState.ActiveFocusBar.FocusSlot4.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot4.Type]} tokens");
            System.Console.WriteLine($"Focus Bar Slot 5: {gameState.ActiveFocusBar.FocusSlot5.Name} ({gameState.ActiveFocusBar.FocusSlot5.Level}) : {gameState.TradeTokens[gameState.ActiveFocusBar.FocusSlot5.Type]} tokens");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"Active Move Focus: {gameState.ActiveFocusBar.FocusSlot5.Name}");
            System.Console.WriteLine("#############################");
        }
    }
}
