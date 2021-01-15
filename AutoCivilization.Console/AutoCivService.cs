using AutoCivilization.Abstractions;
using Microsoft.Extensions.Hosting;
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

            // initialise game state
            // TODO: wonder card deck for bot
            await _focusCardDeckInitialiser.InitialiseFocusCardsDeckForBot();
            await _leaderCardInitialiser.InitialiseRandomLeaderForBot();
            _focusBarInitialiser.InitialiseFocusBarForBot();

            // execute turn
            var activeFocusCard = _botGameStateService.ActiveFocusBar.ActiveFocusSlot;
            var focusCardResolver = _focusCardResolverFactory.GetFocusCardResolverForFocusCard(activeFocusCard);

            focusCardResolver.InitialiseMoveState();
            do
            {
                var stepAction = focusCardResolver.GetNextStep();
                if (stepAction.ShouldExecuteAction())
                {
                    var actionData = stepAction.ExecuteAction();
                    System.Console.WriteLine(actionData.Message);

                    if (stepAction.OperationType == OperationType.InformationRequest)
                    {
                        foreach(var o in actionData.ResponseOptions) { System.Console.WriteLine(o); }
                        var response = System.Console.ReadLine();
                        stepAction.ProcessActionResponse(response);
                    }
                }
            } while (focusCardResolver.HasMoreSteps);

            focusCardResolver.Resolve();

            // TODO: update focus bar - shift cards up by 1...
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
