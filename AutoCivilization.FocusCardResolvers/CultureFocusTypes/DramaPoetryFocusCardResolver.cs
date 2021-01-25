using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.FocusCardResolvers
{
    public class DramaPoetryFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel2FocusCardMoveResolver
    {
        private const int BaseCityControlTokens = 3;

        private readonly ICultureResolverUtility _cultureResolverUtility;

        public DramaPoetryFocusCardMoveResolver(ICultureResolverUtility cultureResolverUtility,
                                                ITokenPlacementCityAdjacentActionRequestStep placementInstructionRequest,
                                                ITokenPlacementCityAdjacentInformationRequestStep placedInformationRequest,
                                                ITokenPlacementNaturalWonderControlledInformationRequestStep wondersControlledInformationRequest,
                                                ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base()
        {
            _cultureResolverUtility = cultureResolverUtility;

            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl2;

            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, wondersControlledInformationRequest);
            _actionSteps.Add(3, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _moveState = _cultureResolverUtility.CreateBasicCultureMoveState(botGameStateService, BaseCityControlTokens);
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
