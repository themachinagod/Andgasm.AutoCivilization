using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.FocusCardResolvers
{
    public class CivilServiceFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel3FocusCardMoveResolver
    {
        private const int BaseCityControlTokens = 3;
        private const int BaseTerritoryControlTokens = 1;

        private readonly ICultureResolverUtility _cultureResolverUtility;

        public CivilServiceFocusCardMoveResolver(ICultureResolverUtility cultureResolverUtility,
                                                 ITokenPlacementCityAdjacentActionRequestStep placementCityInstructionRequest,
                                                 ITokenPlacementTerritoryAdjacentActionRequestStep placementTerritoryInstructionRequest,
                                                 ITokenPlacementCityAdjacentInformationRequestStep placedCityInformationRequest,
                                                 ITokenPlacementTerritoryAdjacentInformationRequest placedTerritoryInformationRequest,
                                                 ITokenPlacementNaturalWonderControlledInformationRequestStep wondersControlledInformationRequest,
                                                 ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base()
        {
            _cultureResolverUtility = cultureResolverUtility;

            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl3;

            _actionSteps.Add(0, placementCityInstructionRequest);
            _actionSteps.Add(1, placedCityInformationRequest);
            _actionSteps.Add(2, placementTerritoryInstructionRequest);
            _actionSteps.Add(3, placedTerritoryInformationRequest);
            _actionSteps.Add(4, wondersControlledInformationRequest);
            _actionSteps.Add(5, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _moveState = _cultureResolverUtility.CreateBasicCultureMoveState(botGameStateService, BaseCityControlTokens);
            _moveState.BaseTerritoryControlTokensToBePlaced = BaseTerritoryControlTokens;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
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
