using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class SteamPowerFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel3FocusCardMoveResolver
    {
        private const int SupportedCaravans = 2;
        private const int BaseCaravanMoves = 6;

        private readonly IEconomyResolverUtility _economyResolverUtility;

        public SteamPowerFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                               IEconomyResolverUtility economyResolverUtility,
                                               ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                               ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                               ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                               IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                               ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                               IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl3;

            _economyResolverUtility = economyResolverUtility;

            var loopSeed = 0;
            for (var tradecaravan = 0; tradecaravan < SupportedCaravans; tradecaravan++)
            {
                _actionSteps.Add(loopSeed, caravanMovementActionRequest);
                _actionSteps.Add(loopSeed + 1, caravanDestinationInformationRequest);
                _actionSteps.Add(loopSeed + 2, caravanMovementInformationRequest);
                _actionSteps.Add(loopSeed + 3, cityStateDestinationInformationRequest);
                _actionSteps.Add(loopSeed + 4, rivalCityDestinationInformationRequest);
                _actionSteps.Add(loopSeed + 5, removeCaravanActionRequest);
                loopSeed = _actionSteps.Count;
            }
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _economyResolverUtility.PrimeBaseEconomyState(botGameStateService, SupportedCaravans, BaseCaravanMoves);
            _botMoveStateService.CanMoveOnWater = true;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            _economyResolverUtility.UpdateBaseEconomyGameStateForMove(botGameStateService, SupportedCaravans);
            botGameStateService.ControlledNaturalResources += 1;
            _currentStep = -1;
            return BuildMoveSummary(botGameStateService);
        }

        private string BuildMoveSummary(BotGameStateCache gameState)
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I recieved 1 natural resource which I may use for future construction projects;\n";
            return _economyResolverUtility.BuildGeneralisedEconomyMoveSummary(summary, gameState);
        }
    }
}
