using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class ForeignTradeFocusCardMoveResolver : FocusCardMoveResolverBase, IEconomyLevel1FocusCardMoveResolver
    {
        public ForeignTradeFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                            ICaravanMovementActionRequestStep caravanMovementActionRequest,
                                            ICaravanDestinationInformationRequestStep caravanDestinationInformationRequest,
                                            IRivalCityDestinationInformationRequestStep rivalCityDestinationInformationRequest,
                                            ICityStateDestinationInformationRequestStep cityStateDestinationInformationRequest,
                                            IRemoveCaravanActionRequestStep removeCaravanActionRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Economy;
            FocusLevel = FocusLevel.Lvl1;

            // DBr: how do we determine how many trade tokens were used
            //      i.e. base = 3, tokens = 2 : total 5
            //      move to closest unvisted city state move cost = 4
            //      we should only use 1 of our two tokens
            //      but we currently dont ask the user how many spaces we moved said caravan
            //      TODO: we need to ask the user how many spaces the caravan was moved
            //            only need to do this if we are not on route - on route can presume we used all moves and still didnt reach target

            _actionSteps.Add(0, caravanMovementActionRequest);
            _actionSteps.Add(1, caravanDestinationInformationRequest);
            _actionSteps.Add(2, cityStateDestinationInformationRequest);
            _actionSteps.Add(3, rivalCityDestinationInformationRequest);
            _actionSteps.Add(4, removeCaravanActionRequest);
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
            }
            else if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.RivalCity)
            {
                // TODO:
                // add chosen player color to gamestates visited rival cities collection (if not visited)
                // add players random diplomacy card to rival diplomacy cards collection (if not visited)
                // add two trade tokens to the focus type with the smallest pile of trade tokens (always)
            }
            else
            {
                botGameStateService.SupportedCaravanCount = _botMoveStateService.SupportedCaravanCount;
                botGameStateService.CaravansOnRouteCount = _botMoveStateService.SupportedCaravanCount;
            }
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
