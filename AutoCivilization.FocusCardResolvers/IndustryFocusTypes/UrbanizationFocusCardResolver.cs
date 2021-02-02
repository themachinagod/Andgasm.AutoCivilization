using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class UrbanizationFocusCardMoveResolver : FocusCardMoveResolverBase, IIndustryLevel4FocusCardMoveResolver
    {
        private const int BaseProduction = 8;
        private const int BaseCityDistance = 5;

        private readonly IBotRoundStateCache _botRoundStateCache;
        private readonly IIndustryResolverUtility _industryResolverUtiliity;
        private readonly IFocusBarTechnologyUpgradeResolver _focusBarTechnologyUpgradeResolver;

        public UrbanizationFocusCardMoveResolver(IWonderPlacementCityActionRequestStep wonderPlacementCityActionRequestStep,
                                                ICityPlacementActionRequestStep cityPlacementActionRequestStep,
                                                ICityPlacementInformationRequestStep cityPlacementInformationRequestStep,
                                                IFocusBarTechnologyUpgradeResolver focusBarTechnologyUpgradeResolver,
                                                IIndustryResolverUtility industryResolverUtility,
                                                IBotRoundStateCache botRoundStateCache) : base()
        {
            _industryResolverUtiliity = industryResolverUtility;
            _focusBarTechnologyUpgradeResolver = focusBarTechnologyUpgradeResolver;
            _botRoundStateCache = botRoundStateCache;

            FocusType = FocusType.Industry;
            FocusLevel = FocusLevel.Lvl4;

            _actionSteps.Add(0, wonderPlacementCityActionRequestStep);
            _actionSteps.Add(1, cityPlacementActionRequestStep);
            _actionSteps.Add(2, cityPlacementInformationRequestStep);
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            _moveState = _industryResolverUtiliity.CreateBasicIndustryMoveState(botGameStateService, BaseProduction, BaseCityDistance);
            _moveState.CanMoveOnWater = true;
            _botRoundStateCache.SubMoveConfigurations.Add(new SubMoveConfiguration()
            {
                AdditionalFocusTypeToExecuteOnFocusBar = FocusType.Culture,
                ShouldResetSubFocusCard = true,
            });
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            _industryResolverUtiliity.UpdateBaseIndustryGameStateForMove(_moveState, botGameStateService);

            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            var exsummary = _industryResolverUtiliity.BuildGeneralisedIndustryMoveSummary(summary, _moveState);
            exsummary += $"\nAs a result of executing this focus card, I will now resolve my culture focus card as it it were in the fifth slot in my focus bar, I will then reset it.\nPlease press any key to execute this action now...\n";
            return exsummary;
        }
    }
}
