using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class EconomyResolverUtility : IEconomyResolverUtility
    {
        public BotMoveState CreateBasicEconomyMoveState(BotGameState botGameStateCache, int supportedCaravans, int baseMoves)
        {
            var moveState = new BotMoveState();
            moveState.ActiveFocusBarForMove = botGameStateCache.ActiveFocusBar;
            moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            moveState.BaseCaravanMoves = baseMoves;
            moveState.SupportedCaravanCount = supportedCaravans;

            for (int tc = 0; tc < moveState.SupportedCaravanCount; tc++)
            {
                moveState.TradeCaravansAvailable.Add(tc, new TradeCaravanMoveState());
            }
            return moveState;
        }

        public void UpdateBaseEconomyGameStateForMove(BotMoveState movesState, BotGameState botGameStateService, int supportedCaravans)
        {
            var onRouteCaravans = 0;
            for (var tradecaravan = 0; tradecaravan < supportedCaravans; tradecaravan++)
            {
                var movingCaravan = movesState.TradeCaravansAvailable[tradecaravan];
                if (movingCaravan.CaravanDestinationType == CaravanDestinationType.CityState)
                {
                    if (!botGameStateService.CityStateDiplomacyCardsHeld.Contains(movingCaravan.CaravanCityStateDestination))
                    {
                        // TODO: add city state diplomacy card to city state diplomancy cards collection (if not visited)
                        botGameStateService.CityStateDiplomacyCardsHeld.Add(movingCaravan.CaravanCityStateDestination);
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

            botGameStateService.SupportedCaravanCount = movesState.SupportedCaravanCount;
            botGameStateService.CaravansOnRouteCount = onRouteCaravans;
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(movesState.TradeTokensAvailable);
        }

        public string BuildGeneralisedEconomyMoveSummary(string currentSummary, BotGameState gameState, BotMoveState movesState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            sb.Append($"I updated my game state to show that I have {gameState.SupportedCaravanCount} trade caravans available to me in total\n");
            sb.Append($"I updated my game state to show that I have {gameState.CaravansOnRouteCount} trade caravans currently on route to destinations on the board\n");

            var totTokensUsed = movesState.TradeCaravansAvailable[0].EconomyTokensUsedThisTurn;
            if (totTokensUsed > 0) sb.Append($"I updated my game state to show that I used {totTokensUsed} economy trade tokens I had available to me to facilitate this move\n");
            if (totTokensUsed < 0) sb.Append($"I updated my game state to show that I recieved {Math.Abs(totTokensUsed)} economy trade tokens which I may use in the future\n");

            foreach (var vc in movesState.TradeCaravansAvailable.Select(x => x.Value))
            {
                if (vc.CaravanDestinationType == CaravanDestinationType.CityState)
                {
                    // TODO: diplomacy cards for city states if first visit
                    sb.Append($"As a result of visiting {vc.CaravanCityStateDestination.Name}, I updated my game state to show that I recieved 2 {vc.CaravanCityStateDestination.Type} trade tokens which I may use in the future\n");
                }
            }

            foreach (var vc in movesState.TradeCaravansAvailable.Select(x => x.Value))
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
