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
        public BotMoveState CreateBasicIndustryMoveState(BotGameState botGameStateCache, int baseProduction, int baseDistance)
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
            moveState.BaseCityDistance = baseDistance;
            return moveState;
        }

        public void UpdateBaseIndustryGameStateForMove(BotMoveState moveState, BotGameState botGameStateService)
        {
            botGameStateService.WonderCardDecks = moveState.ActiveWonderCardDecks;
            botGameStateService.PurchasedWonders = new List<WonderCardModel>(moveState.PurchasedWonders);
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(moveState.TradeTokensAvailable);
            botGameStateService.FriendlyCityCount += moveState.FriendlyCitiesAddedThisTurn;
            botGameStateService.ControlledNaturalResources -= moveState.NaturalResourcesSpentThisTurn;
        }

        public string BuildGeneralisedIndustryMoveSummary(string currentSummary, BotMoveState moveState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            if (moveState.WonderPurchasedThisTurn != null)
            {
                sb.Append($"I updated my game state to show that I purchased the world wonder {moveState.WonderPurchasedThisTurn.Name}, the token of which you placed on my stongest free city\n");
                sb.Append($"I facilitated this move with the following production capacity breakdown;\n");
                sb.Append($"Focus card base capacity: {moveState.BaseProductionPoints} production points\n");
                sb.Append($"Industry diplomacy cards retained bonus: {moveState.VisitedCityStates.Count} held diplomacy cards worth {moveState.VisitedCityStates.Count} production points\n");
                sb.Append($"Natural wonder resources retained bonus: {moveState.ControlledNaturalWonders.Count} held natural wonders worth {moveState.ControlledNaturalWonders.Count * 2} production points\n");
                sb.Append($"Natural resources points : {moveState.NaturalResourcesToSpend} held worth {moveState.NaturalResourcesToSpend * 2} production points, of which we spent {moveState.NaturalResourcesSpentThisTurn} resources\n");
                sb.Append($"Industry trade token points : {moveState.TradeTokensAvailable[FocusType.Industry]} held worth {moveState.TradeTokensAvailable[FocusType.Industry]} production points of which we spent {moveState.IndustryTokensUsedThisTurn} trade tokens\n");
                sb.Append($"Total production capacity available before purchase: {moveState.ComputedProductionCapacityForTurn} production points\n");
            }
            else
            {
                sb.Append($"I was unable to purchase a world wonder on this turn for the following reasons;\n");
                if (moveState.FriendlyCityCount <= moveState.PurchasedWonders.Count) sb.Append($"I do not have enough friendly cities without existing wonder tokens\n");
                else sb.Append($"I do not have enough production capacity to purchase any of the unlocked wonders\n");
            }

            if (moveState.HasPurchasedCityThisTurn) sb.Append($"I updated my game state to show that I build 1 new city of which you placed on the board in a legal space\n");

            return sb.ToString();
        }
    }
}
