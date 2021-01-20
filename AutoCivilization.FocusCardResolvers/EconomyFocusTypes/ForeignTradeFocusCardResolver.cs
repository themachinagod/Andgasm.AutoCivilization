using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

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
            _botMoveStateService.TradeTokensAvailable[FocusType.Economy] = botGameStateService.TradeTokens[FocusType.Economy];
            _botMoveStateService.BaseCaravanMoves = 3;
            _botMoveStateService.SupportedCaravanCount = 1;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.CityState)
            {
                // TODO:
                // add chosen city state to gamestates visited city states collection (if not visited)
                // add city state diplomacy card to city state diplomancy cards collection (if not visited)
                // add two trade tokens to the focus type associated with chosen city state (always)
                botGameStateService.CaravansOnRouteCount = 0;
            }
            else if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.RivalCity)
            {
                // TODO:
                // add chosen player color to gamestates visited rival cities collection (if not visited)
                // add players random diplomacy card (in order stated in rules) to rival diplomacy cards collection (if not visited)
                // add two trade tokens to the focus type with the smallest pile of trade tokens (always)
                botGameStateService.CaravansOnRouteCount = 0;
            }
            else
            {
                botGameStateService.SupportedCaravanCount = _botMoveStateService.SupportedCaravanCount;
                botGameStateService.CaravansOnRouteCount = _botMoveStateService.SupportedCaravanCount;
            }

            botGameStateService.TradeTokens[FocusType.Economy] = _botMoveStateService.EconomyTokensUsedThisTurn;
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
