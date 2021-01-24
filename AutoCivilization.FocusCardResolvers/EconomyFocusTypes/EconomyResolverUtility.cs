using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class EconomyResolverUtility : IEconomyResolverUtility
    {
        private readonly IBotMoveStateCache _botMoveStateService;

        public EconomyResolverUtility(IBotMoveStateCache botMoveStateService)
        {
            _botMoveStateService = botMoveStateService;
        }

        public void PrimeBaseEconomyState(BotGameStateCache botGameStateCache, int supportedCaravans, int baseMoves)
        {
            _botMoveStateService.ActiveFocusBarForMove = botGameStateCache.ActiveFocusBar;
            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            _botMoveStateService.BaseCaravanMoves = baseMoves;
            _botMoveStateService.SupportedCaravanCount = supportedCaravans;
            _botMoveStateService.CanMoveOnWater = true;

            for (int tc = 0; tc < _botMoveStateService.SupportedCaravanCount; tc++)
            {
                _botMoveStateService.TradeCaravansAvailable.Add(tc, new TradeCaravanMoveState());
            }
        }

        public void UpdateBaseEconomyGameStateForMove(BotGameStateCache botGameStateService, int supportedCaravans)
        {
            var onRouteCaravans = 0;
            for (var tradecaravan = 0; tradecaravan < supportedCaravans; tradecaravan++)
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
        }

        public string BuildGeneralisedEconomyMoveSummary(string currentSummary, BotGameStateCache gameState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            sb.Append($"I updated my game state to show that I have {gameState.SupportedCaravanCount} trade caravans available to me in total\n");
            sb.Append($"I updated my game state to show that I have {gameState.CaravansOnRouteCount} trade caravans currently on route to destinations on the board\n");

            var totTokensUsed = _botMoveStateService.TradeCaravansAvailable[0].EconomyTokensUsedThisTurn;
            if (totTokensUsed > 0) sb.Append($"I updated my game state to show that I used {totTokensUsed} economy trade tokens I had available to me to facilitate this move\n");
            if (totTokensUsed < 0) sb.Append($"I updated my game state to show that I recieved {Math.Abs(totTokensUsed)} economy trade tokens which I may use in the future\n");

            foreach (var vc in _botMoveStateService.TradeCaravansAvailable.Select(x => x.Value))
            {
                if (vc.CaravanDestinationType == CaravanDestinationType.CityState)
                {
                    // TODO: diplomacy cards for city states if first visit
                    sb.Append($"As a result of visiting {vc.CaravanCityStateDestination.Name}, I updated my game state to show that I recieved 2 {vc.CaravanCityStateDestination.Type} trade tokens which I may use in the future\n");
                }
            }

            foreach (var vc in _botMoveStateService.TradeCaravansAvailable.Select(x => x.Value))
            {
                if (vc.CaravanDestinationType == CaravanDestinationType.RivalCity)
                {
                    // TODO: diplomacy cards for city states if first visit
                    sb.Append($"As a result of visiting the {vc.CaravanRivalCityColorDestination} players city, I updated my game state to show that I recieved 2 {vc.SmallestTradeTokenPileType} trade tokens which I may use in the future\n");
                }
            }
            return sb.ToString();
        }
    }
}
