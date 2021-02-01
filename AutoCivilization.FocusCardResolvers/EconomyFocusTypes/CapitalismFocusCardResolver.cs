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

        private readonly IEconomyResolverUtility _economyResolverUtility;
        private readonly IBotRoundStateCache _botRoundStateCache;

        public CapitalismFocusCardMoveResolver(IBotRoundStateCache botRoundStateCache,
                                               IEconomyResolverUtility economyResolverUtility,
                                               ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                               ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                               ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                               IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                               ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                               IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base()
        {
            _economyResolverUtility = economyResolverUtility;
            _botRoundStateCache = botRoundStateCache;

            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl4;

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

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            // TODO: the bot round state cache is suspicious here - same the old move state cache

            _moveState = _economyResolverUtility.CreateBasicEconomyMoveState(botGameStateService, SupportedCaravans, BaseCaravanMoves);
            _moveState.CanMoveOnWater = true;
            _botRoundStateCache.SubMoveConfigurations.Add(new SubMoveConfiguration()
            {
                AdditionalFocusTypeToExecuteOnFocusBar = _moveState.ActiveFocusBarForMove.FocusSlot4.Type,
                ShouldResetSubFocusCard = false,
                SubMoveExecutionPhase = SubMoveExecutionPhase.PostPrimaryReset
            });
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            _economyResolverUtility.UpdateBaseEconomyGameStateForMove(_moveState, botGameStateService, SupportedCaravans);
            _currentStep = -1;
            return BuildMoveSummary(botGameStateService);
        }

        private string BuildMoveSummary(BotGameState gameState)
        {
            var summary = "To summarise my move I did the following;\n";
            summary = _economyResolverUtility.BuildGeneralisedEconomyMoveSummary(summary, gameState, _moveState);
            summary += $"\nAs a result of executing this focus card, I will now resolve the card in my 4th focus bar slot without reseting it.\nPlease press any key to execute this action now...\n";
            return summary;
        }
    }
}
