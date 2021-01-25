using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class CurrencyFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel2FocusCardMoveResolver
    {
        private const int SupportedCaravans = 2;
        private const int BaseCaravanMoves = 4;

        private readonly IEconomyResolverUtility _economyResolverUtility;

        public CurrencyFocusCardMoveResolver(IEconomyResolverUtility economyResolverUtility,
                                             ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                             ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                             ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                             IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                             ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                             IRemoveAdjacentBarbariansActionRequestStep removeAdjacentBarbariansActionRequest,
                                             IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base()
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl2;

            _economyResolverUtility = economyResolverUtility;

            _actionSteps.Add(0, removeAdjacentBarbariansActionRequest);

            var loopSeed = 1;
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
            _moveState = _economyResolverUtility.CreateBasicEconomyMoveState(botGameStateService, SupportedCaravans, BaseCaravanMoves);
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            _economyResolverUtility.UpdateBaseEconomyGameStateForMove(_moveState, botGameStateService, SupportedCaravans);
            _currentStep = -1;
            return BuildMoveSummary(botGameStateService);
        }

        private string BuildMoveSummary(BotGameStateCache gameState)
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I asked you to remove all barbarians adjacent to my territory from the board;\n";
            return _economyResolverUtility.BuildGeneralisedEconomyMoveSummary(summary, gameState, _moveState);
        }
    }
}
