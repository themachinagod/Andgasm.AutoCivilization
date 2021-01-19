using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.ActionSteps;
using AutoCivilization.FocusCardResolvers;
using AutoCivilization.TechnologyResolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoCivilization.Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((context, config) =>
                {
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<AutoCivClient>();
                    services.AddTransient<IAutoCivGameClient, AutoCivGameClient>();
                    services.AddTransient<IAutoCivMoveClient, AutoCivMoveClient>();
                    
                    // state services
                    services.AddScoped<IBotMoveStateCache, BotMoveStateCache>();
                    services.AddSingleton<IGlobalGameCache, GlobalGameCache>();

                    // game initialisers
                    services.AddTransient<IFocusCardDeckInitialiser, FocusCardDeckInitialiser>();
                    services.AddTransient<IFocusBarInitialiser, FocusBarInitialiser>();
                    services.AddTransient<ILeaderCardInitialiser, LeaderCardInitialiser>();

                    // culture action steps
                    services.AddTransient<ITokenPlacementCityAdjacentActionRequestStep, TokenPlacementCityAdjacentActionRequestStep>();
                    services.AddTransient<ITokenPlacementTerritoryAdjacentActionRequestStep, TokenPlacementTerritoryAdjacentActionRequestStep>();
                    services.AddTransient<ITokenPlacementCityAdjacentInformationRequestStep, TokenPlacementCityAdjacentInformationRequestStep>();
                    services.AddTransient<ITokenPlacementTerritoryAdjacentInformationRequest, TokenPlacementTerritoryAdjacentInformationRequest>();
                    services.AddTransient<ITokenPlacementNaturalWondersInformationRequestStep, TokenPlacementNaturalWondersInformationRequestStep>();
                    services.AddTransient<ITokenPlacementNaturalResourcesInformationRequestStep, TokenPlacementNaturalResourcesInformationRequestStep>();
                    services.AddTransient<ITokenFlipEnemyActionRequestStep, TokenFlipEnemyActionRequestStep>();

                    // science action steps
                    services.AddTransient<INukePlayerCityFocusCardActionRequestStep, NukePlayerCityFocusCardActionRequestStep>();
                    services.AddTransient<INoActionStep, NoActionStep>();

                    // culture focus card resolvers
                    services.AddTransient<IFocusCardResolverFactory, FocusCardResolverFactory>();
                    services.AddTransient<ICultureLevel1FocusCardResolver, EarlyEmpireFocusCardResolver>();
                    services.AddTransient<ICultureLevel2FocusCardResolver, DramaPoetryFocusCardResolver>();
                    services.AddTransient<ICultureLevel3FocusCardMoveResolver, CivilServiceFocusCardMoveResolver>();
                    services.AddTransient<ICultureLevel4FocusCardResolver, MassMediaFocusCardResolver>();

                    // science focus resolvers
                    services.AddTransient<IScienceLevel1FocusCardResolver, AstrologyFocusCardResolver>();
                    services.AddTransient<IScienceLevel2FocusCardResolver, MathematicsFocusCardResolver>();
                    services.AddTransient<IScienceLevel3FocusCardResolver, ReplaceablePartsCardResolver>();
                    services.AddTransient<IScienceLevel4FocusCardResolver, NuclearPowerFocusCardResolver>();

                    // technology resolvers
                    services.AddTransient<ITechnologyUpgradeResolver, TechnologyUpgradeResolver>();
                    services.AddTransient<IFocusBarTechnologyUpgradeResolver, FocusBarTechnologyUpgradeResolver>();
                    services.AddTransient<ITechnologyBreakthroughResolver, TechnologyBreakthroughResolver>();
                });
    }
}
