using AutoCivilization.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{
    public class AutoCivService : BackgroundService
    {
        private readonly IFocusCardDeckInitialiser _focusCardDeckInitialiser;
        private readonly ILeaderCardInitialiser _leaderCardInitialiser;
        private readonly IFocusBarInitialiser _focusBarInitialiser;
        private readonly IBotGameStateService _botGameStateService;
        private readonly IFocusCardResolverFactory _focusCardResolverFactory;

        public AutoCivService(IFocusCardDeckInitialiser focusCardDeckInitialiser,
                              ILeaderCardInitialiser leaderCardInitialiser,
                              IFocusBarInitialiser focusBarInitialiser,
                              IBotGameStateService botGameStateService,
                              IFocusCardResolverFactory focusCardResolverFactory)
        {
            _focusCardDeckInitialiser = focusCardDeckInitialiser;
            _leaderCardInitialiser = leaderCardInitialiser;
            _focusBarInitialiser = focusBarInitialiser;
            _botGameStateService = botGameStateService;
            _focusCardResolverFactory = focusCardResolverFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await base.StartAsync(stoppingToken);

            // TODO: take in config data such as:
            //       difficlty (settler)
            //       player count (2)
            //       active city states (brussels, carthage, buenos ares)

            WriteConsoleHeader();

            // initialise game state
            // TODO: wonder card deck for bot
            await _focusCardDeckInitialiser.InitialiseFocusCardsDeckForBot();
            _focusBarInitialiser.InitialiseFocusBarForBot();
            WriteConsoleInitialiseComplete();

            await _leaderCardInitialiser.InitialiseRandomLeaderForBot();
            _focusBarInitialiser.InitialiseFocusBarForBot();
            WriteConsoleGameStart();
            do
            {
                // execute turn
                WriteConsoleRoundHeader();
                var activeFocusCard = _botGameStateService.ActiveFocusBar.ActiveFocusSlot;
                var focusCardResolver = _focusCardResolverFactory.GetFocusCardResolverForFocusCard(activeFocusCard);
                do
                {
                    var stepAction = focusCardResolver.GetNextStep();
                    if (stepAction.ShouldExecuteAction())
                    {
                        var actionData = stepAction.ExecuteAction();
                        System.Console.WriteLine();
                        System.Console.WriteLine(actionData.Message);

                        if (stepAction.OperationType == OperationType.InformationRequest)
                        {
                            WriteConsoleResponseOptions(actionData);
                            var response = System.Console.ReadLine();
                            stepAction.ProcessActionResponse(response);
                        }
                        else { WriteConsoleAnyKeyContinue(); }
                    }
                } while (focusCardResolver.HasMoreSteps);

                focusCardResolver.Resolve();
                // TODO: update focus bar - shift cards up by 1...

                WriteConsoleAwaitingNextTurn();
                _botGameStateService.CurrentRoundNumber++;
            } while (true);
        }

        private static void WriteConsoleResponseOptions((string Message, IReadOnlyCollection<string> ResponseOptions) actionData)
        {
            foreach (var o in actionData.ResponseOptions) { System.Console.WriteLine(o); }
        }

        private static void WriteConsoleAnyKeyContinue()
        {
            System.Console.WriteLine("Press any key when you have done this...");
            System.Console.ReadKey();
        }

        private static void WriteConsoleAwaitingNextTurn()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Now you guys go on ahead and take your shot, press any key when its time for me to move...");
            System.Console.ReadKey();
            System.Console.Clear();
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

        private void WriteConsoleRoundHeader()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("#############################");
            System.Console.WriteLine($"# Game {_botGameStateService.GameId}                 #");
            System.Console.WriteLine($"# Round {_botGameStateService.CurrentRoundNumber}                   #");
            System.Console.WriteLine("#############################");
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

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
