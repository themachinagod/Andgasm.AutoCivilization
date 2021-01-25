using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class AstrologyFocusCardMoveResolver : FocusCardMoveResolverBase, IScienceLevel1FocusCardMoveResolver
    {
        private const int BaseTechIncreasePoints = 5;

        private readonly IScienceResolverUtility _scienceResolverUtility;
        
        public AstrologyFocusCardMoveResolver(INoActionStep noActionRequestActionRequest,
                                              IScienceResolverUtility scienceResolverUtility) : base()
        {
            _scienceResolverUtility = scienceResolverUtility;

            _actionSteps.Add(0, noActionRequestActionRequest);

            FocusType = FocusType.Science;
            FocusLevel = FocusLevel.Lvl1;
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
            return _scienceResolverUtility.BuildGeneralisedScienceMoveSummary(summary, upgradeResponse, _moveState);
        }
    }
}
