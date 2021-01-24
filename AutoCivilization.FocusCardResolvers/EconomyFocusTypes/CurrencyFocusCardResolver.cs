﻿using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.FocusCardResolvers
{
    public class CurrencyFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel2FocusCardMoveResolver
    {
        private const int SupportedCaravans = 2;
        private const int BaseCaravanMoves = 4;

        public CurrencyFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                            ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                            ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                            ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                            IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                            ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                            IRemoveAdjacentBarbariansActionRequestStep removeAdjacentBarbariansActionRequest,
                                            IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl2;

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
            _botMoveStateService.ActiveFocusBarForMove = botGameStateService.ActiveFocusBar;
            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);
            _botMoveStateService.BaseCaravanMoves = BaseCaravanMoves;
            _botMoveStateService.SupportedCaravanCount = SupportedCaravans;

            for (int tc = 0; tc < _botMoveStateService.SupportedCaravanCount; tc++)
            {
                _botMoveStateService.TradeCaravansAvailable.Add(tc, new TradeCaravanMoveState());
            }
        }

        /// <summary>
        /// Resolve the updated game state from the current move state
        /// Updtes game states caravan status
        /// Update game states visited city states
        /// Update game states visited rivals cities
        /// Update game states trade tokens counters
        /// Increment the moves step counter
        /// </summary>
        /// <param name="botGameStateService">The game state to update for move</param>
        /// <returns>A textual summary of what the bot did this move</returns>
        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var onRouteCaravans = 0;
            for (var tradecaravan = 0; tradecaravan < SupportedCaravans; tradecaravan++)
            {
                var movingCaravan = _botMoveStateService.TradeCaravansAvailable[tradecaravan];
                if (movingCaravan.CaravanDestinationType == CaravanDestinationType.CityState)
                {
                    if (!botGameStateService.VisitedCityStates.Contains(movingCaravan.CaravanCityStateDestination))
                    {
                        // TODO: add city state diplomacy card to city state diplomancy cards collection (if not visited)
                        botGameStateService.VisitedCityStates.Add(movingCaravan.CaravanCityStateDestination);
                    }
                }
                else if (movingCaravan.CaravanDestinationType == CaravanDestinationType.RivalCity)
                {
                    if (!botGameStateService.VisitedPlayerColors.Contains(movingCaravan.CaravanRivalCityColorDestination))
                    {
                        // TODO: add players diplomacy card (in order stated in rules) to rival diplomacy cards collection
                        botGameStateService.VisitedPlayerColors.Add(movingCaravan.CaravanRivalCityColorDestination);
                    }
                }
                else { onRouteCaravans += 1; }
            }

            botGameStateService.SupportedCaravanCount = _botMoveStateService.SupportedCaravanCount;
            botGameStateService.CaravansOnRouteCount = onRouteCaravans;
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(_botMoveStateService.TradeTokensAvailable);
            _currentStep = -1;

            return BuildMoveSummary(botGameStateService);
        }

        private string BuildMoveSummary(BotGameStateCache gameState)
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I asked you to remove all barbarians adjacent to my territory from the board;\n";
            summary += $"I updated my game state to show that I have {gameState.SupportedCaravanCount} trade caravans available to me in total;\n";
            summary += $"I updated my game state to show that I have {gameState.CaravansOnRouteCount} trade caravans currently on route to destinations on the board;\n";

            var totTokensUsed = _botMoveStateService.TradeCaravansAvailable[0].EconomyTokensUsedThisTurn;
            if (totTokensUsed > 0) summary += $"I updated my game state to show that I used {totTokensUsed} economy trade tokens I had available to me to facilitate this move\n";
            if (totTokensUsed < 0) summary += $"I updated my game state to show that I recieved {Math.Abs(totTokensUsed)} economy trade tokens which I may use in the future\n";

            foreach(var vc in _botMoveStateService.TradeCaravansAvailable.Select(x => x.Value))
            {
                if (vc.CaravanDestinationType == CaravanDestinationType.CityState)
                {
                    // TODO: diplomacy cards for city states if first visit
                    summary += $"As a result of visiting {vc.CaravanCityStateDestination.Name}, I updated my game state to show that I recieved 2 {vc.CaravanCityStateDestination.Type} trade tokens which I may use in the future\n";
                }
            }

            foreach (var vc in _botMoveStateService.TradeCaravansAvailable.Select(x => x.Value))
            {
                if (vc.CaravanDestinationType == CaravanDestinationType.RivalCity)
                {
                    // TODO: diplomacy cards for city states if first visit
                    summary += $"As a result of visiting the {vc.CaravanRivalCityColorDestination} players city, I updated my game state to show that I recieved 2 {vc.SmallestTradeTokenPileType} trade tokens which I may use in the future\n";
                }
            }
            return summary;
        }
    }
}
