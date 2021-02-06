using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.MiscResolvers;
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
                    services.AddHostedService<AutoCivGameHost>();
                    services.AddTransient<IAutoCivGameClient, AutoCivGameClient>();
                    services.AddTransient<IAutoCivRoundClient, AutoCivRoundClient>();
                    services.AddTransient<IAutoCivMoveClient, AutoCivMoveClient>();

                    // state services
                    services.AddScoped<IBotRoundStateCache, BotRoundStateCache>();
                    services.AddSingleton<IGlobalGameCache, GlobalGameCache>();

                    // game initialisers
                    services.AddTransient<IFocusCardDeckInitialiser, FocusCardDeckInitialiser>();
                    services.AddTransient<IFocusBarInitialiser, FocusBarInitialiser>();
                    services.AddTransient<ILeaderCardInitialiser, LeaderCardInitialiser>();
                    services.AddTransient<ICityStatesInitialiser, CityStatesInitialiser>();
                    services.AddTransient<IWonderCardDecksInitialiser, WonderCardDecksInitialiser>();
                    services.AddTransient<IWonderCardsDeckInitialiser, WonderCardsDeckInitialiser>();

                    // culture action steps
                    services.AddTransient<ITokenPlacementCityAdjacentActionRequestStep, TokenPlacementCityAdjacentActionRequestStep>();
                    services.AddTransient<ITokenPlacementTerritoryAdjacentActionRequestStep, TokenPlacementTerritoryAdjacentActionRequestStep>();
                    services.AddTransient<ITokenPlacementCityAdjacentInformationRequestStep, TokenPlacementCityAdjacentInformationRequestStep>();
                    services.AddTransient<ITokenPlacementTerritoryAdjacentInformationRequest, TokenPlacementTerritoryAdjacentInformationRequestStep>();
                    services.AddTransient<ITokenPlacementNaturalWonderCountInformationRequestStep, TokenPlacementNaturalWonderCountInformationRequestStep>();
                    services.AddTransient<ITokenPlacementNaturalWonderControlledInformationRequestStep, TokenPlacementNaturalWonderInformationRequestStep>();
                    services.AddTransient<ITokenPlacementNaturalResourcesInformationRequestStep, TokenPlacementNaturalResourcesInformationRequestStep>();
                    services.AddTransient<ITokenFlipEnemyActionRequestStep, TokenFlipEnemyActionRequestStep>();

                    // science action steps
                    services.AddTransient<INukePlayerCityFocusCardActionRequestStep, NukePlayerCityFocusCardActionRequestStep>();
                    services.AddTransient<INoActionStep, NoActionStep>();

                    // economy action steps
                    services.AddTransient<ICaravanDestinationInformationRequestStep, CaravanDestinationInformationRequestStep>();
                    services.AddTransient<ICaravanMovementActionRequestStep, CaravanMovementActionRequestStep>();
                    services.AddTransient<ICaravanMovementInformationRequestStep, CaravanMovementInformationRequestStep>();
                    services.AddTransient<ICityStateCaravanDestinationInformationRequestStep, CityStateCaravanDestinationInformationRequestStep>();
                    services.AddTransient<IRivalCityCaravanDestinationInformationRequestStep, RivalCityCaravanDestinationInformationRequestStep>();
                    services.AddTransient<IRemoveCaravanActionRequestStep, RemoveCaravanActionRequestStep>();
                    services.AddTransient<IRemoveAdjacentBarbariansActionRequestStep, RemoveAdjacentBarbariansActionRequestStep>();

                    // industry action steps
                    services.AddTransient<IWonderPlacementCityActionRequestStep, WonderPlacementCityActionRequestStep>();
                    services.AddTransient<ICityPlacementActionRequestStep, CityPlacementActionRequestStep>();
                    services.AddTransient<ICityPlacementInformationRequestStep, CityPlacementInformationRequestStep>();

                    // military action steps
                    services.AddTransient<IEnemyWithinAttackDistanceInformationRequestStep, EnemyWithinAttackDistanceInformationRequestStep>();
                    services.AddTransient<IEnemyTypeToAttackInformationRequestStep, EnemyTypeToAttackInformationRequestStep>();
                    services.AddTransient<IEnemyAttackPowerInformationRequestStep, EnemyAttackPowerInformationRequestStep>();
                    services.AddTransient<IAttackPrimaryResultActionRequestStep, EnemyAttackPrimaryResultActionRequestStep>();
                    services.AddTransient<IDefeatedBarbarianActionRequestStep, DefeatedBarbarianActionRequestStep>();
                    services.AddTransient<IConquerCityStateInformationRequestStep, ConquerCityStateInformationRequestStep>();
                    services.AddTransient<IDefeatedRivalControlTokenActionRequestStep, DefeatedRivalControlTokenActionRequestStep>();
                    services.AddTransient<IDefeatedCapitalCityActionRequestStep, DefeatedCapitalCityActionRequestStep>();
                    services.AddTransient<IConquerNonCapitalCityActionRequestStep, ConquerNonCapitalCityActionRequestStep>();
                    services.AddTransient<IFailedAttackActionRequestStep, FailedAttackActionRequestStep>();
                    services.AddTransient<ISupplementAttackPowerInformationRequestStep, SupplementAttackPowerInformationRequestStep>();
                    services.AddTransient<IConquerWorldWonderInformationRequestStep, ConquerWorldWonderInformationRequestStep>();
                    services.AddTransient<IConquerdNaturalWonderInformationRequestStep, ConquerdNaturalWonderInformationRequestStep>();
                    
                    // culture focus card resolvers
                    services.AddTransient<IFocusCardResolverFactory, FocusCardResolverFactory>();
                    services.AddTransient<ICultureResolverUtility, CultureResolverUtility>();
                    services.AddTransient<ICultureLevel1FocusCardMoveResolver, EarlyEmpireFocusCardMoveResolver>();
                    services.AddTransient<ICultureLevel2FocusCardMoveResolver, DramaPoetryFocusCardMoveResolver>();
                    services.AddTransient<ICultureLevel3FocusCardMoveResolver, CivilServiceFocusCardMoveResolver>();
                    services.AddTransient<ICultureLevel4FocusCardMoveResolver, MassMediaFocusCardMoveResolver>();

                    // science focus resolvers
                    services.AddTransient<IScienceResolverUtility, ScienceResolverUtility>();
                    services.AddTransient<IScienceLevel1FocusCardMoveResolver, AstrologyFocusCardMoveResolver>();
                    services.AddTransient<IScienceLevel2FocusCardMoveResolver, MathematicsFocusCardMoveResolver>();
                    services.AddTransient<IScienceLevel3FocusCardMoveResolver, ReplaceablePartsCardMoveResolver>();
                    services.AddTransient<IScienceLevel4FocusCardMoveResolver, NuclearPowerFocusCardMoveResolver>();

                    // economy focus resolvers
                    services.AddTransient<IEconomyResolverUtility, EconomyResolverUtility>();
                    services.AddTransient<IEconomyLevel1FocusCardMoveResolver, ForeignTradeFocusCardMoveResolver>();
                    services.AddTransient<IEconomyLevel2FocusCardMoveResolver, CurrencyFocusCardMoveResolver>();
                    services.AddTransient<IEconomyLevel3FocusCardMoveResolver, SteamPowerFocusCardMoveResolver>();
                    services.AddTransient<IEconomyLevel4FocusCardMoveResolver, CapitalismFocusCardMoveResolver>();

                    // industry focus resolvers
                    services.AddTransient<IIndustryResolverUtility, IndustryResolverUtility>();
                    services.AddTransient<IIndustryLevel1FocusCardMoveResolver, PotteryFocusCardMoveResolver>();
                    services.AddTransient<IIndustryLevel2FocusCardMoveResolver, AnimalHusbandryFocusCardMoveResolver>();
                    services.AddTransient<IIndustryLevel3FocusCardMoveResolver, NationalismFocusCardMoveResolver>();
                    services.AddTransient<IIndustryLevel4FocusCardMoveResolver, UrbanizationFocusCardMoveResolver>();

                    // military focus resolvers
                    services.AddTransient<IMilitaryResolverUtility, MilitaryResolverUtility>();
                    services.AddTransient<IMilitaryLevel1FocusCardMoveResolver, MasonryFocusCardMoveResolver>();

                    // technology resolvers
                    services.AddTransient<ITechnologyUpgradeResolver, TechnologyUpgradeResolver>();
                    services.AddTransient<IFocusBarTechnologyUpgradeResolver, FocusBarTechnologyUpgradeResolver>();
                    services.AddTransient<ITechnologyBreakthroughResolver, TechnologyBreakthroughResolver>();

                    // misc resolvers
                    services.AddTransient<ISmallestTradeTokenPileResolver, SmallestTradeTokenPileResolver>();
                    services.AddTransient<IFocusBarResetResolver, FocusBarResetResolver>();
                    services.AddTransient<IOrdinalSuffixResolver, OrdinalSuffixResolver>();
                });
    }
}
