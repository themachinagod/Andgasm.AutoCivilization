using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class ReplaceablePartsCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel3FocusCardMoveResolver
    {
        private const int BaseTechIncreasePoints = 5;

        private readonly IScienceResolverUtility _scienceResolverUtility;
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;

        public ReplaceablePartsCardMoveResolver(INoActionStep noActionRequestActionRequest,
                                                IScienceResolverUtility scienceResolverUtility,
                                                ITechnologyUpgradeResolver technologyUpgradeResolver) : base()
        {
            _scienceResolverUtility = scienceResolverUtility;
            _technologyUpgradeResolver = technologyUpgradeResolver;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl3;
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            _moveState = _scienceResolverUtility.CreateBasicScienceMoveState(botGameStateService, BaseTechIncreasePoints);
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            var freeUpgradeResponse = _technologyUpgradeResolver.ResolveFreeTechnologyUpdate(_moveState.ActiveFocusBarForMove);
            var techUpgradeResponse = _scienceResolverUtility.UpdateBaseScienceGameStateForMove(_moveState, botGameStateService);
            botGameStateService.TradeTokens[FocusType.Science] = 0;
            _currentStep = -1;
            return BuildMoveSummary(freeUpgradeResponse, techUpgradeResponse);
        }

        private string BuildMoveSummary(FocusBarUpgradeResponse freeTechUpgradeResponse, TechnologyUpgradeResponse techLevelUpgradeResponse)
        {
            var summary = "To summarise my move I did the following;\n";
            if (freeTechUpgradeResponse.HasUpgraded && freeTechUpgradeResponse.OldTechnology.Name != freeTechUpgradeResponse.NewTechnology.Name)
            {
                summary += $"I received a free technology upgrade breakthrough allowing me to upgrade {freeTechUpgradeResponse.OldTechnology.Name} to {freeTechUpgradeResponse.NewTechnology.Name}\n";
            }
            return _scienceResolverUtility.BuildGeneralisedScienceMoveSummary(summary, techLevelUpgradeResponse, _moveState);
        }
    }
}
