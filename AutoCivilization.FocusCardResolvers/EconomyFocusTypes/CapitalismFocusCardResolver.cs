using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.FocusCardResolvers
{
    public class CapitalismFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel4FocusCardMoveResolver
    {
        private const int SupportedCaravans = 3;
        private const int BaseCaravanMoves = 6;

        private readonly IBotRoundStateCache _botRoundStateCache;
        private readonly IEconomyResolverUtility _economyResolverUtility;

        public CapitalismFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                               IBotRoundStateCache botRoundState,
                                               IEconomyResolverUtility economyResolverUtility,
                                               ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                               ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                               ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                               IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                               ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                               IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl4;

            _economyResolverUtility = economyResolverUtility;
            _botRoundStateCache = botRoundState;

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
            _botRoundStateCache.ShouldExecuteAdditionalFocusCard = true;
            _botRoundStateCache.AdditionalFocusTypeToExecuteOnFocusBar = _botMoveStateService.ActiveFocusBarForMove.FocusSlot4.Type;
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
            summary = _economyResolverUtility.BuildGeneralisedEconomyMoveSummary(summary, gameState);
            summary += $"\nAs a result of executing this focus card, I will now resolve the card in my 4th focus bar slot without reseting it. Please press any key to execute this action now...\n";
            return summary;
        }
    }
}
