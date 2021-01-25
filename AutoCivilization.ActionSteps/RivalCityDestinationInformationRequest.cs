using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using AutoCivilization.StateManagement;
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

        public RivalCityDestinationInformationRequestStep(IOrdinalSuffixResolver ordinalSuffixResolver,
                                                          ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver) : base()
        {
            OperationType = OperationType.InformationRequest;

            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;
            _ordinalSuffixResolver = ordinalSuffixResolver;
        }

        public override bool ShouldExecuteAction(BotMoveStateCache moveState)
        {
            var movingCaravan = moveState.TradeCaravansAvailable[moveState.CurrentCaravanIdToMove - 1];
            if (movingCaravan.CaravanDestinationType == CaravanDestinationType.RivalCity) return true;
            return false;
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            // TODO: we need players and colors
            //       currently hard wired!
            // TODO: what if the bot visited more than 1?

            var caravanRef = _ordinalSuffixResolver.GetOrdinalSuffixWithInput(moveState.CurrentCaravanIdToMove);
            var playerColor = new List<string> { "1. Red", "2. Green", "3. Blue", "4. White" };
            return new MoveStepActionData($"Which rival city color did my {caravanRef} trade caravan arrive at?",
                   playerColor);
        }

        /// <summary>
        /// Update move state with visited rival cities
        /// </summary>
        /// <param name="input">The code for the rival cities visited specified by the user</param>
        public override BotMoveStateCache ProcessActionResponse(string input, BotMoveStateCache moveState)
        {
            // TODO: what if the bot visited more than 1?
            // TODO: potentially some state mutation issues on caravan obj

            var updatedMoveState = moveState.Clone();
            var movingCaravan = updatedMoveState.TradeCaravansAvailable[updatedMoveState.CurrentCaravanIdToMove - 1];
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

            var smallestpile = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(updatedMoveState.ActiveFocusBarForMove, updatedMoveState.TradeTokensAvailable);
            updatedMoveState.SmallestTradeTokenPileType = smallestpile;
            movingCaravan.SmallestTradeTokenPileType = smallestpile;
            updatedMoveState.TradeTokensAvailable[smallestpile] += BaseTradeTokensForRivalCityVisit;
            return updatedMoveState;
        }
    }
}
