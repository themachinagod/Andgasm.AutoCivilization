using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class DramaPoetryFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel2FocusCardMoveResolver
    {
        private const int BaseCityControlTokens = 3;

        private readonly ICultureResolverUtility _cultureResolverUtility;

        public DramaPoetryFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                                ICultureResolverUtility cultureResolverUtility,
                                                ITokenPlacementCityAdjacentActionRequestStep placementInstructionRequest,
                                                ITokenPlacementCityAdjacentInformationRequestStep placedInformationRequest,
                                                ITokenPlacementNaturalWonderControlledInformationRequestStep wondersControlledInformationRequest,
                                                ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl2;

            _cultureResolverUtility = cultureResolverUtility;

            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, wondersControlledInformationRequest);
            _actionSteps.Add(3, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _cultureResolverUtility.PrimeBaseCultureState(botGameStateService, BaseCityControlTokens);
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            _cultureResolverUtility.UpdateBaseGameStateForMove(botGameStateService);
            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            return _cultureResolverUtility.BuildGeneralisedCultureMoveSummary(summary);
        }
    }
}
