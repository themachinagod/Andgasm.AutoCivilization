using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;
using System.Collections.Generic;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class PotteryFocusCardMoveResolver : FocusCardMoveResolverBase, IIndustryLevel1FocusCardMoveResolver
    {
        private const int BaseProduction = 8;

        private ICultureResolverUtility _cultureResolverUtility;

        public PotteryFocusCardMoveResolver(
                                            IWonderPlacementCityActionRequestStep wonderPlacementCityActionRequestStep) : base()
        {
            //_cultureResolverUtility = cultureResolverUtility;

            FocusType = FocusType.Industry;
            FocusLevel = FocusLevel.Lvl1;

            // TODO:
            // steps needed:
            //  build a world wonder : 
            //      we need to attempt to build a wonder from our wonder deck
            //      use our total production capacity :- BaseProduction + (TotalResources * 2) + Industry Trade Tokens
            //      if our PC > lowest cost wonder && CityCount > WonderCount
            //          ask the user to place purchased wonder token under strongest city with no wonder
            //          add wonder to move state collection
            //          add spent resources and trade tokens to move state
            //      other wise - do nout
            //  build a city :
            //      ask user to build city on legal space
            //      ask user if they managed to place a city legally
            //      if yes 
            //          add city placed to move state
            //  resolve :
            //      set wonders built to game state from move state
            //      set trade tokens as needed to game state from move state
            //      set resources as needed to game state from move state
            //      set cities built to game state from move state
            //      if no wonder and no city built
            //          upgrade this card to next tech level 
            //
            //  new steps needed
            //  IWonderPlacementCityActionRequestStep
            //  ICityPlacementActionRequestStep
            //  ICityPlacementInformationRequestStep

            _actionSteps.Add(0, wonderPlacementCityActionRequestStep);
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            //_moveState = _cultureResolverUtility.CreateBasicCultureMoveState(botGameStateService, BaseProduction);
            _moveState = new BotMoveState();
            _moveState.ActiveWonderCardDecks = botGameStateService.WonderCardDecks;
            _moveState.PurchasedWonders = new List<WonderCardModel>(botGameStateService.PurchasedWonders);
            _moveState.VisitedCityStates = new List<CityStateModel>(botGameStateService.VisitedCityStates);
            _moveState.ControlledNaturalWonders = new List<string>(botGameStateService.ControlledNaturalWonders);
            _moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);
            _moveState.NaturalResourcesToSpend = botGameStateService.ControlledNaturalResources;
            _moveState.FriendlyCityCount = botGameStateService.FriendlyCityCount;
            _moveState.BaseProductionPoints = BaseProduction;
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            //_cultureResolverUtility.UpdateBaseCultureGameStateForMove(_moveState, botGameStateService);

            botGameStateService.WonderCardDecks = _moveState.ActiveWonderCardDecks;
            botGameStateService.PurchasedWonders = new List<WonderCardModel>(_moveState.PurchasedWonders);
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(_moveState.TradeTokensAvailable);
            botGameStateService.FriendlyCityCount += _moveState.FriendlyCitiesAddedThisTurn;
            botGameStateService.ControlledNaturalResources -= _moveState.NaturalResourcesSpentThisTurn;

            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            StringBuilder sb = new StringBuilder(summary);
            if (_moveState.WonderPurchasedThisTurn != null)
            {
                sb.Append($"I updated my game state to show that I purchased the world wonder {_moveState.WonderPurchasedThisTurn.Name}, the token of which you placed on my stongest free city\n");
                sb.Append($"I facilitated this move with the following production capacity breakdown;\n");
                sb.Append($"Pottery focus card base capacity: {_moveState.BaseProductionPoints} production points\n");
                sb.Append($"Industry diplomacy cards retained bonus: {_moveState.VisitedCityStates.Count} held diplomacy cards worth {_moveState.VisitedCityStates.Count} production points\n");
                sb.Append($"Natural wonder resources retained bonus: {_moveState.ControlledNaturalWonders.Count} held natural wonders worth {_moveState.ControlledNaturalWonders.Count * 2} production points\n");
                sb.Append($"Natural resources points : {_moveState.NaturalResourcesToSpend} held worth {_moveState.NaturalResourcesToSpend * 2} production points, of which we spent {_moveState.NaturalResourcesSpentThisTurn} resources\n");
                sb.Append($"Industry trade token points : {_moveState.TradeTokensAvailable[FocusType.Industry]} held worth {_moveState.TradeTokensAvailable[FocusType.Industry]} production points of which we spent {_moveState.IndustryTokensUsedThisTurn} trade tokens\n");
                sb.Append($"Total production capacity available before purchase: {_moveState.ComputedProductionCapacityForTurn} production points\n");
            }
            else
            {
                sb.Append($"I was unable to purchase a world wonder on this turn for the following reasons;\n");
                if (_moveState.FriendlyCityCount <= _moveState.PurchasedWonders.Count) sb.Append($"I do not have enough friendly cities without existing wonder tokens\n");
                else sb.Append($"I do not have enough production capacity to purchase any of the unlocked wonders\n");
            }



            // TOOD: show if city was managed to be placed

            // TODO: if no wonder or city accomplished we need to replace the focus card - summarise this 

            return sb.ToString(); // _cultureResolverUtility.BuildGeneralisedCultureMoveSummary(summary, _moveState);
        }
    }
}
