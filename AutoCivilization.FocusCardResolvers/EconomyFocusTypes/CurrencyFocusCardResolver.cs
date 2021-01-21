using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class CurrencyFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel2FocusCardMoveResolver
    {
        public CurrencyFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                            ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                            ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                            ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                            IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                            ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                            IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl2;

            var loopSeed = 0;
            for (var tradecaravan = 0; tradecaravan < 2; tradecaravan++)
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
            _botMoveStateService.BaseCaravanMoves = 4;
            _botMoveStateService.SupportedCaravanCount = 2;

            for (int tc = 0; tc < _botMoveStateService.SupportedCaravanCount; tc++)
            {
                _botMoveStateService.TradeCaravansAvailable.Add(tc, new TradeCaravanMoveState());
            }
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var onRouteCaravans = 0;
            for (var tradecaravan = 0; tradecaravan < 2; tradecaravan++)
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
            // seeing issue with trade token in summary even thos game state tracked correctly

            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that have {gameState.SupportedCaravanCount} caravans available to me in total;\n";
            summary += $"I updated my game state to show that have {gameState.CaravansOnRouteCount} caravans currently on route to destinations on the board;\n";

            if (_botMoveStateService.EconomyTokensUsedThisTurn > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.EconomyTokensUsedThisTurn} economy trade tokens I had available to me to facilitate this move\n";
            if (_botMoveStateService.EconomyTokensUsedThisTurn < 0) summary += $"I updated my game state to show that I recieved {Math.Abs(_botMoveStateService.EconomyTokensUsedThisTurn)} economy trade tokens which I may use in the future\n";

            // TODO: need to replicate the below accounting for multiple caravans
            //if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.CityState)
            //{
            //    // TODO: diplomacy cards for city states if first visit
            //    summary += $"As a result of visiting {_botMoveStateService.CaravanCityStateDestination.Name}, I updated my game state to show that I recieved 2 {_botMoveStateService.CaravanCityStateDestination.Type} trade tokens which I may use in the future\n";
            //}

            //if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.RivalCity)
            //{
            //    // TODO: diplomacy cards for city states if first visit
            //    summary += $"As a result of visiting the {_botMoveStateService.CaravanRivalCityColorDestination} players city, I updated my game state to show that I recieved 2 {_botMoveStateService.SmallestTradeTokenPileType} trade tokens which I may use in the future\n";
            //}
            return summary;
        }
    }
}
