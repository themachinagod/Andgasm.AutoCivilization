using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.FocusCardResolvers
{
    public class PotteryFocusCardMoveResolver : FocusCardMoveResolverBase, IIndustryLevel1FocusCardMoveResolver
    {
        private const int BaseProduction = 5;

        private ICultureResolverUtility _cultureResolverUtility;

        public PotteryFocusCardMoveResolver(ICultureResolverUtility cultureResolverUtility,
                                                ITokenPlacementCityAdjacentActionRequestStep placementInstructionRequest,
                                                ITokenPlacementCityAdjacentInformationRequestStep placedInformationRequest,
                                                ITokenPlacementNaturalWonderControlledInformationRequestStep naturalWonderControlledInformationRequest,
                                                ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base()
        {
            _cultureResolverUtility = cultureResolverUtility;

            FocusType = FocusType.Culture;
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

            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, naturalWonderControlledInformationRequest);
            _actionSteps.Add(3, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            _moveState = _cultureResolverUtility.CreateBasicCultureMoveState(botGameStateService, BaseProduction);
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            _cultureResolverUtility.UpdateBaseCultureGameStateForMove(_moveState, botGameStateService);
            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            return _cultureResolverUtility.BuildGeneralisedCultureMoveSummary(summary, _moveState);
        }
    }
}
