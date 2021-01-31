using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class IndustryResolverUtility : IIndustryResolverUtility
    {
        private readonly IFocusBarTechnologyUpgradeResolver _focusBarTechnologyUpgradeResolver;

        public IndustryResolverUtility(IFocusBarTechnologyUpgradeResolver technologyUpgradeResolver)
        {
            _focusBarTechnologyUpgradeResolver = technologyUpgradeResolver;
        }

        public BotMoveState CreateBasicIndustryMoveState(BotGameState botGameStateCache, int baseProduction)
        {
            var moveState = new BotMoveState();
            moveState.ActiveFocusBarForMove = botGameStateCache.ActiveFocusBar;
            moveState.ActiveWonderCardDecks = botGameStateCache.WonderCardDecks;
            moveState.PurchasedWonders = new List<WonderCardModel>(botGameStateCache.PurchasedWonders);
            moveState.VisitedCityStates = new List<CityStateModel>(botGameStateCache.VisitedCityStates);
            moveState.ControlledNaturalWonders = new List<string>(botGameStateCache.ControlledNaturalWonders);
            moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            moveState.NaturalResourcesToSpend = botGameStateCache.ControlledNaturalResources;
            moveState.FriendlyCityCount = botGameStateCache.FriendlyCityCount;
            moveState.BaseProductionPoints = baseProduction;
            return moveState;
        }

        public FocusBarUpgradeResponse UpdateBaseIndustryGameStateForMove(BotMoveState moveState, BotGameState botGameStateService)
        {
            botGameStateService.WonderCardDecks = moveState.ActiveWonderCardDecks;
            botGameStateService.PurchasedWonders = new List<WonderCardModel>(moveState.PurchasedWonders);
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(moveState.TradeTokensAvailable);
            botGameStateService.FriendlyCityCount += moveState.FriendlyCitiesAddedThisTurn;
            botGameStateService.ControlledNaturalResources -= moveState.NaturalResourcesSpentThisTurn;

            FocusBarUpgradeResponse freeUpgrade = new FocusBarUpgradeResponse(false, moveState.ActiveFocusBarForMove, moveState.ActiveFocusBarForMove.ActiveFocusSlot, null);
            if (!moveState.HasPurchasedCityThisTurn && !moveState.HasPurchasedWonderThisTurn)
            {
                freeUpgrade = _focusBarTechnologyUpgradeResolver.RegenerateFocusBarSpecificTechnologyLevelUpgrade(moveState.ActiveFocusBarForMove, FocusType.Industry);
                botGameStateService.ActiveFocusBar = freeUpgrade.UpgradedFocusBar;
            }
            return freeUpgrade;
        }

        public string BuildGeneralisedIndustryMoveSummary(string currentSummary, BotMoveState movesState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            if (movesState.CityControlTokensPlacedThisTurn > 0) sb.Append($"I updated my game state to show that I placed {movesState.CityControlTokensPlacedThisTurn} control token(s) next to my cities on the board\n");
            if (movesState.TerritroyControlTokensPlacedThisTurn > 0) sb.Append($"I updated my game state to show that I placed {movesState.TerritroyControlTokensPlacedThisTurn} control token(s) next to my friendly territory on the board\n");
            if (movesState.CultureTokensUsedThisTurn > 0) sb.Append($"I updated my game state to show that I used {movesState.CultureTokensUsedThisTurn} culture trade token(s) I had available to me to facilitate this move\n");
            if (movesState.CultureTokensUsedThisTurn < 0) sb.Append($"I updated my game state to show that I recieved {Math.Abs(movesState.CultureTokensUsedThisTurn)} culture trade token(s) for not using all my available control token placements which I may use in the future\n");
            if (movesState.NaturalResourceTokensControlledThisTurn > 0) sb.Append($"I updated my game state to show that I controlled {movesState.NaturalResourceTokensControlledThisTurn} natural resources which I may use for future construction projects\n");
            if (movesState.NaturalWonderTokensControlledThisTurn > 0)
            {
                sb.Append($"I updated my game state to show that I controlled the {string.Join(",", movesState.ControlledNaturalWonders)} natural wonder(s)\n");
                sb.Append($"As a result of controlling natural wonders on this turn I have recieved {movesState.NaturalWonderTokensControlledThisTurn} natural resources that I may use for future construction projects\n");
            }
            return sb.ToString();
        }
    }
}
