using AutoCivilization.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCivilization.Console
{
    public class AutoCivGameHost : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IAutoCivGameClient _autoCivGameService;

        public AutoCivGameHost(IServiceScopeFactory serviceScopeFactory,                            
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
                using (var roundScope = _scopeFactory.CreateScope())
                {
                    var scopedRoundService = roundScope.ServiceProvider.GetRequiredService<IAutoCivRoundClient>();
                    scopedRoundService.ExecuteRoundForBot(gameState);
                }
            } while (true);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
