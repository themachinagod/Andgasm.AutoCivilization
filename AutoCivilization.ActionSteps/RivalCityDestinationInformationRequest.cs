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
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        private const int BaseTradeTokensForRivalCityVisit = 2;

        public RivalCityDestinationInformationRequestStep(IBotMoveStateCache botMoveStateService,
                                                          IOrdinalSuffixResolver ordinalSuffixResolver,
                                                          ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;

            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;
            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override bool ShouldExecuteAction()
        {
            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[_botMoveStateService.CurrentCaravanIdToMove - 1];
            if (movingCaravan.CaravanDestinationType == CaravanDestinationType.RivalCity) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction()
        {
            // TODO: we need players and colors
            //       currently hard wired!
            // TODO: what if the bot visited more than 1?

            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(_botMoveStateService.CurrentCaravanIdToMove);
            var playerColor = new List<string> { "1. Red", "2. Green", "3. Blue", "4. White" };
            return new MoveStepActionData($"Which rival city color did my {caravanRef} trade caravan arrive at?",
                   playerColor);
        }

        /// <summary>
        /// Update move state with visited rival cities
        /// </summary>
        /// <param name="input">The code for the rival cities visited specified by the user</param>
        public override void ProcessActionResponse(string input)
        {
            // TODO: what if the bot visited more than 1?

            var movingCaravan = _botMoveStateService.TradeCaravansAvailable[_botMoveStateService.CurrentCaravanIdToMove - 1];
            switch (Convert.ToInt32(input))
            {
                case 1:
                    movingCaravan.CaravanRivalCityColorDestination = "Red";
                    break;
                case 2:
                    movingCaravan.CaravanRivalCityColorDestination = "Green";
                    break;
                case 3:
                    movingCaravan.CaravanRivalCityColorDestination = "Blue";
                    break;
                default:
                    movingCaravan.CaravanRivalCityColorDestination = "White";
                    break;
            }

            var smallestpile = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(_botMoveStateService.ActiveFocusBarForMove, _botMoveStateService.TradeTokensAvailable);
            _botMoveStateService.SmallestTradeTokenPileType = smallestpile;
            movingCaravan.SmallestTradeTokenPileType = smallestpile;
            _botMoveStateService.TradeTokensAvailable[smallestpile] += BaseTradeTokensForRivalCityVisit;
        }
    }
}
