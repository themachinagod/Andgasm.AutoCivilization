using AutoCivilization.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{
    public class AutoCivService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IAutoCivGameClient _autoCivGameService;

        public AutoCivService(IServiceScopeFactory serviceScopeFactory,                            
                              IAutoCivGameClient autoCivGameService)
        {
            _scopeFactory = serviceScopeFactory;
            _autoCivGameService = autoCivGameService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await base.StartAsync(stoppingToken);

            var gameState = await _autoCivGameService.InitialiseNewGame();
            do
            {
                using (var moveScope = _scopeFactory.CreateScope())
                {
                    var scopedMoveService = moveScope.ServiceProvider.GetRequiredService<IAutoCivMoveClient>();
                    scopedMoveService.ExecuteMoveForActiveFocusCard(gameState);
                }
            } while (true);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }

    public interface IAutoCivGameClient 
    {
        Task<BotGameStateCache> InitialiseNewGame();
    }

    public class AutoCivGameClient : IAutoCivGameClient
    {
        private readonly IFocusCardDeckInitialiser _focusCardDeckInitialiser;
        private readonly ILeaderCardInitialiser _leaderCardInitialiser;
        private readonly IFocusBarInitialiser _focusBarInitialiser;
        private readonly IGlobalGameCache _globalGameCache;

        public AutoCivGameClient(IGlobalGameCache globalGameCache,
                                 IFocusCardDeckInitialiser focusCardDeckInitialiser,
                                 ILeaderCardInitialiser leaderCardInitialiser,
                                 IFocusBarInitialiser focusBarInitialiser)
        {
            _globalGameCache = globalGameCache;
            _focusCardDeckInitialiser = focusCardDeckInitialiser;
            _leaderCardInitialiser = leaderCardInitialiser;
            _focusBarInitialiser = focusBarInitialiser;
        }

        public async Task<BotGameStateCache> InitialiseNewGame()
        {
            // TODO: wonder card initialisation for bot

            WriteConsoleHeader();

            var initialFocustCards = await _focusCardDeckInitialiser.InitialiseFocusCardsDeckForBot();
            _globalGameCache.FocusCardsDeck = initialFocustCards;

            var focusBar = _focusBarInitialiser.InitialiseFocusBarForBot();
            var chosenLeader = await _leaderCardInitialiser.InitialiseRandomLeaderForBot();
            var gameState = new BotGameStateCache(focusBar, chosenLeader);

            WriteConsoleGameStart(gameState);
            return gameState;
        }

        private void WriteConsoleHeader()
        {
            System.Console.WriteLine("#############################");
            System.Console.WriteLine("#   AutoCivilization v1.0   #");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine();
        }


        private void WriteConsoleGameStart(BotGameStateCache gameState)
        {
            System.Console.WriteLine($"Leader Selected: {gameState.ChosenLeaderCard.Name} : {gameState.ChosenLeaderCard.Nation}");
            System.Console.WriteLine($"Focus Bar Slot 1: {gameState.ActiveFocusBar.FocusSlot1.Name}");
            System.Console.WriteLine($"Focus Bar Slot 2: {gameState.ActiveFocusBar.FocusSlot2.Name}");
            System.Console.WriteLine($"Focus Bar Slot 3: {gameState.ActiveFocusBar.FocusSlot3.Name}");
            System.Console.WriteLine($"Focus Bar Slot 4: {gameState.ActiveFocusBar.FocusSlot4.Name}");
            System.Console.WriteLine($"Focus Bar Slot 5: {gameState.ActiveFocusBar.FocusSlot5.Name}");
            System.Console.WriteLine();
            System.Console.WriteLine("Please go ahead and select leaders for all human players and setup the physical game board as normal.");
            System.Console.WriteLine("No need to deal me in, I will manage my own focus cards, focus bar and wonder decks.");
            System.Console.WriteLine("If I need any physical interaction with the board, I will ask you to do this for me.");
            System.Console.WriteLine("If I need any information about moves that were made I will ask you some simple questions.");
            System.Console.WriteLine("All you need to do just now is pick a color for me, place my captial city on the board and set aside my other peices.");
            System.Console.WriteLine("When everything is setup and you are happy to start the game, press any key and I will make the first move.");
            System.Console.ReadKey();
        }
    }

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
