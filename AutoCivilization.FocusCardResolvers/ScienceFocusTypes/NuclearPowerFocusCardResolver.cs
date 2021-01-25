using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class NuclearPowerFocusCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel4FocusCardMoveResolver
    {
        private readonly IScienceResolverUtility _scienceResolverUtility;

        private const int BaseTechIncreasePoints = 5;

        public NuclearPowerFocusCardMoveResolver(IScienceResolverUtility scienceResolverUtility,
                                                 INukePlayerCityFocusCardActionRequestStep nukePlayerCityFocusCardActionRequest) : base()
        {
            _scienceResolverUtility = scienceResolverUtility;

            _actionSteps.Add(0, nukePlayerCityFocusCardActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl4;
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _moveState = _scienceResolverUtility.CreateBasicScienceMoveState(botGameStateService, BaseTechIncreasePoints);
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var techUpgradeResponse = _scienceResolverUtility.UpdateBaseScienceGameStateForMove(_moveState, botGameStateService);
            botGameStateService.TradeTokens[FocusType.Science] = 0;
            _currentStep = -1;
            return BuildMoveSummary(techUpgradeResponse);
        }

        private string BuildMoveSummary(TechnologyUpgradeResponse upgradeResponse)
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I asked you to remove one non capital city and adjacent territory for each rival player\n";
            return _scienceResolverUtility.BuildGeneralisedScienceMoveSummary(summary, upgradeResponse, _moveState);
        }
    }
}
