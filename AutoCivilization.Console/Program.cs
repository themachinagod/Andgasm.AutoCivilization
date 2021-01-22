using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.StateInitialisers;
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
                    services.AddTransient<ICityStatesInitialiser, CityStatesInitialiser>();

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

                    // economy action steps
                    services.AddTransient<ICaravanDestinationInformationRequestStep, CaravanDestinationInformationRequestStep>();
                    services.AddTransient<ICaravanMovementActionRequestStep, CaravanMovementActionRequestStep>();
                    services.AddTransient<ICaravanMovementInformationRequestStep, CaravanMovementInformationRequestStep>();
                    services.AddTransient<ICityStateDestinationInformationRequestStep, CityStateDestinationInformationRequestStep>();
                    services.AddTransient<IRivalCityDestinationInformationRequestStep, RivalCityDestinationInformationRequestStep>();
                    services.AddTransient<IRemoveCaravanActionRequestStep, RemoveCaravanActionRequestStep>();
                    services.AddTransient<IRemoveAdjacentBarbariansActionRequestStep, RemoveAdjacentBarbariansActionRequestStep>();

                    // culture focus card resolvers
                    services.AddTransient<IFocusCardResolverFactory, FocusCardResolverFactory>();
                    services.AddTransient<ICultureLevel1FocusCardMoveResolver, EarlyEmpireFocusCardMoveResolver>();
                    services.AddTransient<ICultureLevel2FocusCardMoveResolver, DramaPoetryFocusCardMoveResolver>();
                    services.AddTransient<ICultureLevel3FocusCardMoveResolver, CivilServiceFocusCardMoveResolver>();
                    services.AddTransient<ICultureLevel4FocusCardMoveResolver, MassMediaFocusCardMoveResolver>();

                    // science focus resolvers
                    services.AddTransient<IScienceLevel1FocusCardMoveResolver, AstrologyFocusCardMoveResolver>();
                    services.AddTransient<IScienceLevel2FocusCardMoveResolver, MathematicsFocusCardMoveResolver>();
                    services.AddTransient<IScienceLevel3FocusCardMoveResolver, ReplaceablePartsCardMoveResolver>();
                    services.AddTransient<IScienceLevel4FocusCardMoveResolver, NuclearPowerFocusCardMoveResolver>();

                    // economy focus resolvers
                    services.AddTransient<IEconomyLevel1FocusCardMoveResolver, ForeignTradeFocusCardMoveResolver>();
                    services.AddTransient<IEconomyLevel2FocusCardMoveResolver, CurrencyFocusCardMoveResolver>();
                    services.AddTransient<IEconomyLevel3FocusCardMoveResolver, SteamPowerFocusCardMoveResolver>();
                    services.AddTransient<IEconomyLevel4FocusCardMoveResolver, CapitalismFocusCardMoveResolver>();

                    // technology resolvers
                    services.AddTransient<ITechnologyUpgradeResolver, TechnologyUpgradeResolver>();
                    services.AddTransient<IFocusBarTechnologyUpgradeResolver, FocusBarTechnologyUpgradeResolver>();
                    services.AddTransient<ITechnologyBreakthroughResolver, TechnologyBreakthroughResolver>();

                    // misc resolvers
                    services.AddTransient<ISmallestTradeTokenPileResolver, SmallestTradeTokenPileResolver>();
                    services.AddTransient<IFocusBarEndOfMoveResolver, FocusBarEndOfMoveResolver>();
                    services.AddTransient<IOrdinalSuffixResolver, OrdinalSuffixResolver>();
                });
    }
}
