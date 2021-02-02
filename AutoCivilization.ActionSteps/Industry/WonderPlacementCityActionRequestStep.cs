using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class WonderPlacementCityActionRequestStep : StepActionBase, IWonderPlacementCityActionRequestStep
    {
        private readonly Random _randomService = new Random();
        
        private readonly IWonderCardDecksInitialiser _wonderCardDecksInitialiser;

        private WonderCardModel _wonderToPurchase;

        public WonderPlacementCityActionRequestStep(IWonderCardDecksInitialiser wonderCardDecksInitialiser) : base()
        {
            _wonderCardDecksInitialiser = wonderCardDecksInitialiser;

            OperationType = OperationType.ActionRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            // we only want to ask the user to place a wonder token on city if the bot can afford to purchase one in first plce
            // we only want to ask the user to place a wonder token on city if we have a city that has no wonders 

            if (moveState.FriendlyCityCount <= moveState.PurchasedWonders.Count) return false;

            var diplomacyPointsAvailable = moveState.VisitedCityStates.Count(x => x.Type == FocusType.Industry);
            var naturalWonderPointsAvailable = (moveState.ControlledNaturalWonders.Count * 2);
            var resourcePointsAvailable = (moveState.NaturalResourcesToSpend * 2);
            var tradeTokensAvailable = moveState.TradeTokensAvailable[FocusType.Industry];

            var productionPoints = (moveState.BaseProductionPoints + diplomacyPointsAvailable + naturalWonderPointsAvailable + resourcePointsAvailable + tradeTokensAvailable);
            var unlockedWonders = moveState.ActiveWonderCardDecks.UnlockedWonderCards.Select(x => x.Value);
            var cheapestWonderCost = unlockedWonders.Min(x => x.Cost);
            if (productionPoints >= cheapestWonderCost)
            {
                return true;
            }
            return false;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            _wonderToPurchase = GetWonderToPurchase(moveState);
            return new MoveStepActionData($"As I have purchased the {_wonderToPurchase.Name} {_wonderToPurchase.Era} Wonder, please could you place the appropriate wonders token under my strongest city that currently has no built wonder.",
                   new List<string>());
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            // add purchased wonder to move state
            moveState.PurchasedWonders.Add(_wonderToPurchase);
            moveState.WonderPurchasedThisTurn = _wonderToPurchase;
            moveState.HasPurchasedWonderThisTurn = true;

            // regenerate the wonder decks and unlocked cards accounting for purchased wonder
            var currentWonders = moveState.ActiveWonderCardDecks.AvailableWonderCardDecks.SelectMany(x => x.Value).ToList();
            currentWonders.Remove(_wonderToPurchase);
            moveState.ActiveWonderCardDecks = _wonderCardDecksInitialiser.RegenerateDeckForPurchasedWonder(currentWonders, moveState.ActiveWonderCardDecks.UnlockedWonderCards, _wonderToPurchase);

            // update trade tokens and resources spent in move state
            //  1.Bonus trade tokens from industry city-state diplomacy cards(see Wonders and Diplomacy cards for details).
            //  2.Natural wonder tokens of any type. (no loss of token)
            //  3.Resource tokens of any type. If the AP would overspend by one and has one or more trade tokens, it saves a resource token and spends one trade token instead. (los of resources)
            //  4.Trade tokens on the industry card. (loss of tokens)
            // potential issue with step 3 - we dont check for this overrun

            var diplomacyPointsAvailable = moveState.VisitedCityStates.Count(x => x.Type == FocusType.Industry);
            var naturalWonderPointsAvailable = (moveState.ControlledNaturalWonders.Count * 2);
            var resourcePointsAvailable = (moveState.NaturalResourcesToSpend * 2);
            var tradeTokensAvailable = moveState.TradeTokensAvailable[FocusType.Industry];
            moveState.ComputedProductionCapacityForTurn = (moveState.BaseProductionPoints + diplomacyPointsAvailable + naturalWonderPointsAvailable + resourcePointsAvailable + tradeTokensAvailable);

            var purchasePointGap = _wonderToPurchase.Cost - moveState.BaseProductionPoints;
            if (purchasePointGap > 0)
            {
                var diplomacyPointsUsed = diplomacyPointsAvailable - purchasePointGap < 0 ? diplomacyPointsAvailable : diplomacyPointsAvailable - purchasePointGap; // 1
                purchasePointGap = purchasePointGap - diplomacyPointsUsed;

                var naturalWonderPointsUsed = naturalWonderPointsAvailable - purchasePointGap < 0 ? naturalWonderPointsAvailable : naturalWonderPointsAvailable - purchasePointGap; // 2
                purchasePointGap = purchasePointGap - naturalWonderPointsUsed;

                var resourcePointsUsed = resourcePointsAvailable - purchasePointGap < 0 ? resourcePointsAvailable : resourcePointsAvailable - purchasePointGap; // 2
                purchasePointGap = purchasePointGap - resourcePointsUsed;
                moveState.NaturalResourcesSpentThisTurn = (resourcePointsUsed / 2);

                var tradeTokenPointsUsed = tradeTokensAvailable - purchasePointGap < 0 ? tradeTokensAvailable : tradeTokensAvailable - purchasePointGap; // 2
                purchasePointGap = purchasePointGap - tradeTokenPointsUsed;
                moveState.IndustryTokensUsedThisTurn = tradeTokenPointsUsed;
                moveState.TradeTokensAvailable[FocusType.Industry] -= tradeTokenPointsUsed;
            }
        }

        private WonderCardModel GetWonderToPurchase(BotMoveState moveState)
        {
            var availableWonders = moveState.ActiveWonderCardDecks.UnlockedWonderCards.Select(x => x.Value);
            var cheapestWonderCost = availableWonders.Min(x => x.Cost);
            var wondersToPurchase = availableWonders.Where(x => x.Cost == cheapestWonderCost).ToList();
            var wonderToPurchase = wondersToPurchase.ElementAt(_randomService.Next(wondersToPurchase.Count - 1));
            return wonderToPurchase;
        }
    }
}
