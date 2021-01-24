using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class ReplaceablePartsCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel3FocusCardMoveResolver
    {
        private const int BaseTechIncreasePoints = 5;

        private readonly IScienceResolverUtility _scienceResolverUtility;
        private readonly ITechnologyUpgradeResolver _technologyUpgradeResolver;

        public ReplaceablePartsCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                             INoActionStep noActionRequestActionRequest,
                                             IScienceResolverUtility scienceResolverUtility,
                                             ITechnologyUpgradeResolver technologyUpgradeResolver) : base(botMoveStateService)
        {
            _scienceResolverUtility = scienceResolverUtility;
            _technologyUpgradeResolver = technologyUpgradeResolver;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl3;
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _scienceResolverUtility.PrimeBaseEconomyState(botGameStateService, BaseTechIncreasePoints);
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var freeUpgradeResponse = _technologyUpgradeResolver.ResolveFreeTechnologyUpdate(_botMoveStateService.ActiveFocusBarForMove);
            var techUpgradeResponse = _scienceResolverUtility.UpdateBaseEconomyGameStateForMove(botGameStateService);
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
            return _scienceResolverUtility.BuildGeneralisedEconomyMoveSummary(summary, techLevelUpgradeResponse);
        }
    }
}
