using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class ForeignTradeFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel1FocusCardMoveResolver
    {
        private const int SupportedCaravans = 1;
        private const int BaseCaravanMoves = 3;

        private readonly IEconomyResolverUtility _economyResolverUtility;

        public ForeignTradeFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                                 IEconomyResolverUtility economyResolverUtility,
                                                 ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                                 ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                                 ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                                 IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                                 ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                                 IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl1;

            _economyResolverUtility = economyResolverUtility;

            _actionSteps.Add(0, caravanMovementActionRequest);
            _actionSteps.Add(1, caravanDestinationInformationRequest);
            _actionSteps.Add(2, caravanMovementInformationRequest);
            _actionSteps.Add(3, cityStateDestinationInformationRequest);
            _actionSteps.Add(4, rivalCityDestinationInformationRequest);
            _actionSteps.Add(5, removeCaravanActionRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _economyResolverUtility.PrimeBaseEconomyState(botGameStateService, SupportedCaravans, BaseCaravanMoves);
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            _economyResolverUtility.UpdateBaseEconomyGameStateForMove(botGameStateService, SupportedCaravans);
            _currentStep = -1;
            return BuildMoveSummary(botGameStateService);
        }

        private string BuildMoveSummary(BotGameStateCache gameState)
        {
            var summary = "To summarise my move I did the following;\n";
            return _economyResolverUtility.BuildGeneralisedEconomyMoveSummary(summary, gameState);
        }
    }
}
