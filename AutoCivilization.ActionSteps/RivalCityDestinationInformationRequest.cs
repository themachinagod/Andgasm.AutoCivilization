using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class RivalCityDestinationInformationRequestStep : StepActionBase, IRivalCityDestinationInformationRequestStep
    {
        private readonly ISmallestTradeTokenPileResolver _smallestTradeTokenPileResolver;

        public RivalCityDestinationInformationRequestStep(IBotMoveStateCache botMoveStateService,
                                                          ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;

            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;
        }

        public override bool ShouldExecuteAction()
        {
            if (_botMoveStateService.CaravanDestinationType == CaravanDestinationType.RivalCity) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction()
        {
            // TODO: we need a players and colors
            //       currently hard wired for two player game!

            var playerColor = new List<string> { "1. Red", "2. Green", "3. Blue", "4. White" };
            return new MoveStepActionData("Which rival city color was arrived at?",
                   playerColor);
        }

        public override void ProcessActionResponse(string input)
        {
            // TODO: just using string to start with

            switch (Convert.ToInt32(input))
            {
                case 1:
                    _botMoveStateService.CaravanRivalCityColorDestination = "Red";
                    break;
                case 2:
                    _botMoveStateService.CaravanRivalCityColorDestination = "Green";
                    break;
                case 3:
                    _botMoveStateService.CaravanRivalCityColorDestination = "Blue";
                    break;
                default:
                    _botMoveStateService.CaravanRivalCityColorDestination = "White";
                    break;
            }

            _botMoveStateService.SmallestTradeTokenPileType = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(_botMoveStateService.ActiveFocusBarForMove, _botMoveStateService.TradeTokensAvailable);
            _botMoveStateService.TradeTokensAvailable[_botMoveStateService.SmallestTradeTokenPileType] += 2;
        }
    }
}
