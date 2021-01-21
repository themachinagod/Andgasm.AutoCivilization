using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class ForeignTradeFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel1FocusCardMoveResolver
    {
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
            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);
            _botMoveStateService.BaseCaravanMoves = 3;
            _botMoveStateService.SupportedCaravanCount = 1;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.CityState)
            {
                if (!botGameStateService.VisitedCityStates.Contains(_botMoveStateService.CaravanCityStateDestination))
                {
                    // TODO: add city state diplomacy card to city state diplomancy cards collection (if not visited)
                    botGameStateService.VisitedCityStates.Add(_botMoveStateService.CaravanCityStateDestination);
                }
                botGameStateService.CaravansOnRouteCount = 0;
            }
            else if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.RivalCity)
            {
                if (!botGameStateService.VisitedPlayerColors.Contains(_botMoveStateService.CaravanRivalCityColorDestination))
                {
                    // TODO: add players diplomacy card (in order stated in rules) to rival diplomacy cards collection
                    botGameStateService.VisitedPlayerColors.Add(_botMoveStateService.CaravanRivalCityColorDestination);
                }
                botGameStateService.CaravansOnRouteCount = 0;
            }
            else
            {
                botGameStateService.SupportedCaravanCount = _botMoveStateService.SupportedCaravanCount;
                botGameStateService.CaravansOnRouteCount = _botMoveStateService.SupportedCaravanCount;
            }

            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(_botMoveStateService.TradeTokensAvailable);
            _currentStep = -1;

            return BuildMoveSummary(botGameStateService);
        }

        private string BuildMoveSummary(BotGameStateCache gameState)
        {
            // TODO:

            var summary = "To summarise my move I did the following;\n";
            if (_botMoveStateService.CityControlTokensPlaced > 0) summary += $"I updated my game state to show that have {_botMoveStateService.SupportedCaravanCount} caravans available to me in total;\n";
            if (_botMoveStateService.CityControlTokensPlaced > 0) summary += $"I updated my game state to show that have {gameState.CaravansOnRouteCount} caravans currently on route to destinations on the board;\n";

            return summary;
        }
    }
}
