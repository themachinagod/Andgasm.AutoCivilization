using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class RivalCityDestinationInformationRequestStep : StepActionBase, IRivalCityDestinationInformationRequestStep
    {
        private readonly ISmallestTradeTokenPileResolver _smallestTradeTokenPileResolver;
        private readonly IOrdinalSuffixResolver _ordinalSuffixResolver;

        private const int BaseTradeTokensForRivalCityVisit = 2;

        public RivalCityDestinationInformationRequestStep(IOrdinalSuffixResolver ordinalSuffixResolver,
                                                          ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver) : base()
        {
            OperationType = OperationType.InformationRequest;

            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;
            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
            if (movingCaravan.CaravanDestinationType == CaravanDestinationType.RivalCity) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: we need players and colors
            //       currently hard wired

            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            var playerColor = new List<string> { "1. Red", "2. Green", "3. Blue", "4. White" };
            return new MoveStepActionData($"Which rival city color did my {caravanRef} trade caravan arrive at?",
                   playerColor);
        }

        /// <summary>
        /// Update move state with visited rival cities
        /// </summary>
        /// <param name="input">The code for the rival cities visited specified by the user</param>
        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            // TODO: what if the bot visited more than 1?

            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
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

            var smallestpile = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(moveState.ActiveFocusBarForMove, moveState.TradeTokensAvailable);
            moveState.SmallestTradeTokenPileType = smallestpile;
            movingCaravan.SmallestTradeTokenPileType = smallestpile;
            moveState.TradeTokensAvailable[smallestpile] += BaseTradeTokensForRivalCityVisit;
        }
    }
}
