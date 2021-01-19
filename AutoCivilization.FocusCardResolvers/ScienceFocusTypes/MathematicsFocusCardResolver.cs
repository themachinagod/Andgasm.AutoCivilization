using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.FocusCardResolvers
{
    public class MathematicsFocusCardResolver : FocusCardMoveResolverBase, IScienceLevel2FocusCardResolver
    {
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;
        private readonly ISmallestTradeTokenPileResolver _smallestTradeTokenPileResolver;

        public MathematicsFocusCardResolver(IBotMoveStateCache botMoveStateService,
                                             INoActionStep noActionRequestActionRequest,
                                             ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver,
                                             ITechnologyUpgradeResolver technologyUpgradeResolver) : base(botMoveStateService)
        {
            _technologyUpgradeResolver = technologyUpgradeResolver;
            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl2;
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.ActiveFocusBarForMove = botGameStateService.ActiveFocusBar;
            _botMoveStateService.StartingTechnologyLevel = botGameStateService.TechnologyLevel;

            _botMoveStateService.TradeTokensAvailable[FocusType.Science] = botGameStateService.TradeTokens[FocusType.Science];
            _botMoveStateService.SmallestTradeTokenPileType = _smallestTradeTokenPileResolver.DetermineFocusTypeForSmallestTradeTokenPile(botGameStateService.TradeTokens);
            
            _botMoveStateService.BaseTechnologyIncrease = 5;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.TradeTokensAvailable[_botMoveStateService.SmallestTradeTokenPileType] += 1;

            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            var techUpgradeResponse = _technologyUpgradeResolver.ResolveTechnologyLevelUpdates(_botMoveStateService.StartingTechnologyLevel, techIncrementPoints,
                                                                                               _botMoveStateService.ActiveFocusBarForMove);

            botGameStateService.ActiveFocusBar = techUpgradeResponse.UpgradedFocusBar;

            botGameStateService.TechnologyLevel = techUpgradeResponse.NewTechnologyLevelPoints;
            botGameStateService.TradeTokens[_botMoveStateService.SmallestTradeTokenPileType] = _botMoveStateService.TradeTokensAvailable[_botMoveStateService.SmallestTradeTokenPileType];
            botGameStateService.TradeTokens[FocusType.Science] = 0;
            _currentStep = -1;

            return BuildMoveSummary(techUpgradeResponse);
        }

        private string BuildMoveSummary(TechnologyUpgradeResponse upgradeResponse)
        {
            var techIncrementPoints = _botMoveStateService.BaseTechnologyIncrease + _botMoveStateService.TradeTokensAvailable[FocusType.Science];
            var summary = "To summarise my move I did the following;\n";
            summary += $"I updated my game state to show that I recieved 1 {_botMoveStateService.SmallestTradeTokenPileType} trade tokens which i may use in the future\n";

            summary += $"I updated my game state to show that I incremented my technology points by {techIncrementPoints} to {upgradeResponse.NewTechnologyLevelPoints}\n";
            if (_botMoveStateService.TradeTokensAvailable[FocusType.Science] > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.TradeTokensAvailable[FocusType.Science]} science trade tokens I had available to me to facilitate this move\n";

            if (upgradeResponse.EncounteredBreakthroughs.Count > 0)
            {
                summary += $"As a result of my technology upgrade, I had a technological breakthrough\n";
                foreach (var breakthrough in upgradeResponse.EncounteredBreakthroughs)
                {
                    summary += $"This breakthrough resulted in my {breakthrough.ReplacedFocusCard.Name} being replaced with {breakthrough.UpgradedFocusCard.Name}\n";
                }
            }
            return summary;
        }
    }

    public interface ISmallestTradeTokenPileResolver 
    {
        FocusType DetermineFocusTypeForSmallestTradeTokenPile(Dictionary<FocusType, int> tradeTokenPiles);
    }

    public class SmallestTradeTokenPileResolver : ISmallestTradeTokenPileResolver
    {
        public FocusType DetermineFocusTypeForSmallestTradeTokenPile(Dictionary<FocusType, int> tradeTokenPiles)
        {
            var smallestPileValue = 0;
            var smallestPileType = FocusType.Culture;
            foreach (var tokenPile in tradeTokenPiles)
            {
                if (tokenPile.Value >= smallestPileValue)
                    smallestPileType = tokenPile.Key;
            }
            return smallestPileType;
        }

        public FocusType DetermineFocusTypeForSmallestTradeTokenPileEX(Dictionary<FocusType, int> tradeTokenPiles)
        {
            var smallestPileValue = 0;
            var smallestPileType = FocusType.Culture;
            foreach (var tokenPile in tradeTokenPiles)
            {
                if (tokenPile.Value >= smallestPileValue)
                    smallestPileType = tokenPile.Key;
            }
            return smallestPileType;
        }
    }
}
