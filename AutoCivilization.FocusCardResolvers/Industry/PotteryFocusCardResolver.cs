using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class PotteryFocusCardMoveResolver : FocusCardMoveResolverBase, IIndustryLevel1FocusCardMoveResolver
    {
        private const int BaseProduction = 5;
        private const int BaseCityDistance = 2;

        private IIndustryResolverUtility _industryResolverUtiliity;
        private readonly IFocusBarTechnologyUpgradeResolver _focusBarTechnologyUpgradeResolver;

        public PotteryFocusCardMoveResolver(IWonderPlacementCityActionRequestStep wonderPlacementCityActionRequestStep,
                                            ICityPlacementActionRequestStep cityPlacementActionRequestStep,
                                            ICityPlacementInformationRequestStep cityPlacementInformationRequestStep,
                                            IFocusBarTechnologyUpgradeResolver focusBarTechnologyUpgradeResolver,
                                            IIndustryResolverUtility industryResolverUtility) : base()
        {
            _industryResolverUtiliity = industryResolverUtility;
            _focusBarTechnologyUpgradeResolver = focusBarTechnologyUpgradeResolver;

            FocusType = FocusType.Industry;
            FocusLevel = FocusLevel.Lvl1;

            _actionSteps.Add(0, wonderPlacementCityActionRequestStep);
            _actionSteps.Add(1, cityPlacementActionRequestStep);
            _actionSteps.Add(2, cityPlacementInformationRequestStep);
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            _moveState = _industryResolverUtiliity.CreateBasicIndustryMoveState(botGameStateService, BaseProduction, BaseCityDistance);
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            _industryResolverUtiliity.UpdateBaseIndustryGameStateForMove(_moveState, botGameStateService);

            // DBr: Im torn between using the below ITechnoologyLevelModifier service or the IFocusBarTechnologyUpgradeResolver - which suggest something smells!!
            FocusBarUpgradeResponse freeUpgrade = new FocusBarUpgradeResponse(false, _moveState.ActiveFocusBarForMove, _moveState.ActiveFocusBarForMove.ActiveFocusSlot, null);
            if (!_moveState.HasPurchasedCityThisTurn && !_moveState.HasPurchasedWonderThisTurn)
            {
                freeUpgrade = _focusBarTechnologyUpgradeResolver.RegenerateFocusBarSpecificTechnologyLevelUpgrade(_moveState.ActiveFocusBarForMove, FocusType.Industry);
                botGameStateService.ActiveFocusBar = freeUpgrade.UpgradedFocusBar;
            }

            _currentStep = -1;
            return BuildMoveSummary(freeUpgrade);
        }

        private string BuildMoveSummary(FocusBarUpgradeResponse freeTechUpgradeResponse)
        {
            var summary = "To summarise my move I did the following;\n";
            var exsummary = _industryResolverUtiliity.BuildGeneralisedIndustryMoveSummary(summary, _moveState);

            if (!_moveState.HasPurchasedCityThisTurn && !_moveState.HasPurchasedWonderThisTurn && freeTechUpgradeResponse.HasUpgraded)
            {
                exsummary += $"Becuase I did not manage to purchase a wonder or build a city on this turn, I received a free Industry technology upgrade allowing me to upgrade from { freeTechUpgradeResponse.OldTechnology.Name} to { freeTechUpgradeResponse.NewTechnology.Name}\n";
            }
            return exsummary;
        }
    }
}
