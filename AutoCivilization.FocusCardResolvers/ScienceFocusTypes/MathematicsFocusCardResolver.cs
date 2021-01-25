using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class MathematicsFocusCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel2FocusCardMoveResolver
    {
        private readonly IScienceResolverUtility _scienceResolverUtility;
        private readonly ISmallestTradeTokenPileResolver _smallestTradeTokenPileResolver;

        private const int BaseTechIncreasePoints = 5;

        public MathematicsFocusCardMoveResolver(INoActionStep noActionRequestActionRequest,
                                                ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver,
                                                IScienceResolverUtility scienceResolverUtility) : base()
        {
            _scienceResolverUtility = scienceResolverUtility;
            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl2;
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _moveState = _scienceResolverUtility.CreateBasicScienceMoveState(botGameStateService, BaseTechIncreasePoints);
            _moveState.SmallestTradeTokenPileType = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(botGameStateService.ActiveFocusBar, botGameStateService.TradeTokens);
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            _moveState.TradeTokensAvailable[_moveState.SmallestTradeTokenPileType] += 1;
            var techUpgradeResponse = _scienceResolverUtility.UpdateBaseScienceGameStateForMove(_moveState, botGameStateService);
            botGameStateService.TradeTokens[_moveState.SmallestTradeTokenPileType] = _moveState.TradeTokensAvailable[_moveState.SmallestTradeTokenPileType];
            botGameStateService.TradeTokens[FocusType.Science] = 0;
            _currentStep = -1;
            return BuildMoveSummary(techUpgradeResponse);
        }

        private string BuildMoveSummary(TechnologyUpgradeResponse upgradeResponse)
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I recieved 1 {_moveState.SmallestTradeTokenPileType} trade token as it was the smallest token pile\n";
            return _scienceResolverUtility.BuildGeneralisedScienceMoveSummary(summary, upgradeResponse, _moveState);
        }
    }
}
