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
        private readonly IAutoCivGameService _autoCivGameService;

        public AutoCivService(IServiceScopeFactory serviceScopeFactory,                            
                              IAutoCivGameService autoCivGameService)
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

            await _autoCivGameService.InitialiseNewGame();
            do
            {
                using (var moveScope = _scopeFactory.CreateScope())
                {
                    var scopedMoveService = moveScope.ServiceProvider.GetRequiredService<IAutoCivMoveService>();
                    scopedMoveService.ExecuteMoveForActiveFocusCard();
                }
            } while (true);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }

    public interface IAutoCivGameService 
    {
        Task InitialiseNewGame();
    }

    public class AutoCivGameService : IAutoCivGameService
    {
        private readonly IFocusCardDeckInitialiser _focusCardDeckInitialiser;
        private readonly ILeaderCardInitialiser _leaderCardInitialiser;
        private readonly IFocusBarInitialiser _focusBarInitialiser;
        private readonly IBotGameStateService _botGameStateService;

        public AutoCivGameService(IBotGameStateService botGameStateService,
                                  IFocusCardDeckInitialiser focusCardDeckInitialiser,
                                  ILeaderCardInitialiser leaderCardInitialiser,
                                  IFocusBarInitialiser focusBarInitialiser)
        {
            _botGameStateService = botGameStateService;
            _focusCardDeckInitialiser = focusCardDeckInitialiser;
            _leaderCardInitialiser = leaderCardInitialiser;
            _focusBarInitialiser = focusBarInitialiser;
        }

        public async Task InitialiseNewGame()
        {
            // TODO: wonder card initialisation for bot

            WriteConsoleHeader();

            await _focusCardDeckInitialiser.InitialiseFocusCardsDeckForBot();
            _focusBarInitialiser.InitialiseFocusBarForBot();
            WriteConsoleInitialiseComplete();

            await _leaderCardInitialiser.InitialiseRandomLeaderForBot();
            _focusBarInitialiser.InitialiseFocusBarForBot();
            WriteConsoleGameStart();
        }

        private void WriteConsoleHeader()
        {
            System.Console.WriteLine("#############################");
            System.Console.WriteLine("#   AutoCivilization v1.0   #");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine();
        }

        private static void WriteConsoleInitialiseComplete()
        {
            System.Console.WriteLine("#############################");
            System.Console.WriteLine("#  Focus Deck Initialised   #");
            System.Console.WriteLine("#  Wonder Deck Initialised  #");
            System.Console.WriteLine("#  Focus Bar Initialised    #");
            System.Console.WriteLine("#  Game State Initialised   #");
            System.Console.WriteLine("#############################");
            System.Console.WriteLine();
        }

        private void WriteConsoleGameStart()
        {
            System.Console.WriteLine($"Leader Selected: {_botGameStateService.ChosenLeaderCard.Name} : {_botGameStateService.ChosenLeaderCard.Nation}");
            System.Console.WriteLine($"Focus Bar Slot 1: {_botGameStateService.ActiveFocusBar.FocusSlot1.Name}");
            System.Console.WriteLine($"Focus Bar Slot 2: {_botGameStateService.ActiveFocusBar.FocusSlot2.Name}");
            System.Console.WriteLine($"Focus Bar Slot 3: {_botGameStateService.ActiveFocusBar.FocusSlot3.Name}");
            System.Console.WriteLine($"Focus Bar Slot 4: {_botGameStateService.ActiveFocusBar.FocusSlot4.Name}");
            System.Console.WriteLine($"Focus Bar Slot 5: {_botGameStateService.ActiveFocusBar.FocusSlot5.Name}");
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

    public interface IAutoCivMoveService 
    {
        void ExecuteMoveForActiveFocusCard();
    }

    public class AutoCivMoveService : IAutoCivMoveService
    {
        private readonly IFocusCardResolverFactory _focusCardResolverFactory;

        private readonly IBotGameStateService _botGameStateService;

        public AutoCivMoveService(IFocusCardResolverFactory focusCardResolverFactory,
                                  IBotGameStateService botGameStateService)
        {
            _botGameStateService = botGameStateService;
            _focusCardResolverFactory = focusCardResolverFactory;
        }

        public void ExecuteMoveForActiveFocusCard()
        {
            WriteConsoleRoundHeader();
            var focusCardToExecute = _botGameStateService.ActiveFocusBar.ActiveFocusSlot;
            var focusCardMoveResolver = _focusCardResolverFactory.GetFocusCardMoveResolver(focusCardToExecute);
            focusCardMoveResolver.PrimeMoveState(_botGameStateService);
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

            var moveSummary = focusCardMoveResolver.UpdateGameStateForMove(_botGameStateService);
            WriteConsoleMoveSummary(moveSummary);

            // TODO: update focus bar - shift cards up by 1...

            WriteConsoleAwaitingNextTurn();
            _botGameStateService.CurrentRoundNumber++;
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

        private void WriteConsoleRoundHeader()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"# Game {_botGameStateService.GameId}                 #");
            System.Console.WriteLine($"# Round {_botGameStateService.CurrentRoundNumber}                   #");
            System.Console.WriteLine("#############################");
        }
    }
}
