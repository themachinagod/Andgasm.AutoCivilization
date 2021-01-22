using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class ForeignTradeFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel1FocusCardMoveResolver
    {
        private const int SupportedCaravans = 1;
        private const int BaseCaravanMoves = 3;

        public ForeignTradeFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                            ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                            ICaravanMovementInformationRequestStep caravanMovementInformationRequest,
                                            ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                            IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                            ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                            IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl1;

            _actionSteps.Add(0, caravanMovementActionRequest);
            _actionSteps.Add(1, caravanDestinationInformationRequest);
            _actionSteps.Add(2, caravanMovementInformationRequest);
            _actionSteps.Add(3, cityStateDestinationInformationRequest);
            _actionSteps.Add(4, rivalCityDestinationInformationRequest);
            _actionSteps.Add(5, removeCaravanActionRequest);
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

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var onRouteCaravans = 0;
            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[0];
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
            
            botGameStateService.SupportedCaravanCount = _botMoveStateService.SupportedCaravanCount;
            botGameStateService.CaravansOnRouteCount = onRouteCaravans;
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(_botMoveStateService.TradeTokensAvailable);
            _currentStep = -1;

            return BuildMoveSummary(botGameStateService);
        }

        private string BuildMoveSummary(BotGameStateCache gameState)
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that have {gameState.SupportedCaravanCount} caravans available to me in total;\n";
            summary += $"I updated my game state to show that have {gameState.CaravansOnRouteCount} caravans currently on route to destinations on the board;\n";

            var totTokensUsed = _botMoveStateService.TradeCaravansAvailable[0].EconomyTokensUsedThisTurn;
            if (totTokensUsed > 0) summary += $"I updated my game state to show that I used {totTokensUsed} economy trade tokens I had available to me to facilitate this move\n";
            if (totTokensUsed < 0) summary += $"I updated my game state to show that I recieved {Math.Abs(totTokensUsed)} economy trade tokens which I may use in the future\n";

            var vc = _botMoveStateService.TradeCaravansAvailable[0];
            if (vc.CaravanDestinationType == CaravanDestinationType.CityState)
            {
                // TODO: diplomacy cards for city states if first visit
                summary += $"As a result of visiting {vc.CaravanCityStateDestination.Name}, I updated my game state to show that I recieved 2 {vc.CaravanCityStateDestination.Type} trade tokens which I may use in the future\n";
            }
           
            if (vc.CaravanDestinationType == CaravanDestinationType.RivalCity)
            {
                // TODO: diplomacy cards for city states if first visit
                summary += $"As a result of visiting the {vc.CaravanRivalCityColorDestination} players city, I updated my game state to show that I recieved 2 {vc.SmallestTradeTokenPileType} trade tokens which I may use in the future\n";
            }
            
            return summary;
        }
    }
}
