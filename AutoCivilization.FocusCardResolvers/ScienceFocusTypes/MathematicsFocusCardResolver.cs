using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.FocusCardResolvers
{
    public class MathematicsFocusCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel2FocusCardMoveResolver
    {
        private readonly IScienceResolverUtility _scienceResolverUtility;
        private readonly ISmallestTradeTokenPileResolver _smallestTradeTokenPileResolver;

        private const int BaseTechIncreasePoints = 5;

        public MathematicsFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                                INoActionStep noActionRequestActionRequest,
                                                ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver,
                                                IScienceResolverUtility scienceResolverUtility) : base(botMoveStateService)
        {
            _scienceResolverUtility = scienceResolverUtility;
            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl2;
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _scienceResolverUtility.PrimeBaseScienceState(botGameStateService, BaseTechIncreasePoints);
            _botMoveStateService.SmallestTradeTokenPileType = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(botGameStateService.ActiveFocusBar, botGameStateService.TradeTokens);
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.TradeTokensAvailable[_botMoveStateService.SmallestTradeTokenPileType] += 1;
            var techUpgradeResponse = _scienceResolverUtility.UpdateBaseScienceGameStateForMove(botGameStateService);
            botGameStateService.TradeTokens[_botMoveStateService.SmallestTradeTokenPileType] = _botMoveStateService.TradeTokensAvailable[_botMoveStateService.SmallestTradeTokenPileType];
            botGameStateService.TradeTokens[FocusType.Science] = 0;
            _currentStep = -1;
            return BuildMoveSummary(techUpgradeResponse);
        }

        private string BuildMoveSummary(TechnologyUpgradeResponse upgradeResponse)
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I recieved 1 {_botMoveStateService.SmallestTradeTokenPileType} trade token as it was the smallest token pile\n";
            return _scienceResolverUtility.BuildGeneralisedScienceMoveSummary(summary, upgradeResponse);
        }
    }
}
