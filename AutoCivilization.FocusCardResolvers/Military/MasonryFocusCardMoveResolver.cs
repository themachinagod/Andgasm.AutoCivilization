using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class MasonryFocusCardMoveResolver : FocusCardMoveResolverBase, IMilitaryLevel1FocusCardMoveResolver
    {
        private const int BaseAttackPower = 7;
        private const int BaseAttackRange = 2;
        private const int BaseAttackCount = 2;
        private const int BaseMaxTargetPower = 4;

        private IMilitaryResolverUtility _cultureResolverUtility;

        public MasonryFocusCardMoveResolver(IMilitaryResolverUtility cultureResolverUtility,
                                            IEnemyWithinAttackDistanceInformationRequestStep enemyWithinAttackDistanceInformationRequestStep,
                                            IEnemyTypeToAttackInformationRequestStep enemyTypeToAttackInformationRequestStep,
                                            IEnemyAttackPowerInformationRequestStep enemyAttackPowerInformationRequestStep,
                                            IAttackPrimaryResultActionRequestStep attackPrimaryResultActionRequestStep,
                                            IDefeatedBarbarianActionRequestStep defeatedBarbarianActionRequestStep,
                                            IConquerCityStateActionRequestStep conquerCityStateActionRequestStep,
                                            IDefeatedRivalControlTokenActionRequestStep defeatedRivalControlTokenActionRequestStep,
                                            IDefeatedCapitalCityActionRequestStep defeatedCapitalCityActionRequestStep,
                                            IConquerNonCapitalCityActionRequestStep conquerNonCapitalCityActionRequestStep,
                                            IFailedAttackActionRequestStep failedAttackActionRequestStep) : base()
        {
            _cultureResolverUtility = cultureResolverUtility;

            FocusType = FocusType.Military;
            FocusLevel = FocusLevel.Lvl1;

            // TODO: this is just supporting 1 single attack just now!!!


			_actionSteps.Add(0, enemyWithinAttackDistanceInformationRequestStep);
            _actionSteps.Add(1, enemyTypeToAttackInformationRequestStep);
            _actionSteps.Add(2, enemyAttackPowerInformationRequestStep);
            _actionSteps.Add(3, attackPrimaryResultActionRequestStep);
            _actionSteps.Add(4, defeatedBarbarianActionRequestStep);
            _actionSteps.Add(5, conquerCityStateActionRequestStep);
            _actionSteps.Add(6, defeatedRivalControlTokenActionRequestStep);
            _actionSteps.Add(7, defeatedCapitalCityActionRequestStep);
            _actionSteps.Add(8, conquerNonCapitalCityActionRequestStep);
            _actionSteps.Add(9, failedAttackActionRequestStep);
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            _moveState = new BotMoveState();
            _moveState.BaseAttackPower = BaseAttackPower;
            _moveState.BaseAttackRange = BaseAttackRange;
            _moveState.BaseMaxTargetPower = BaseMaxTargetPower;

            _moveState.ActiveFocusBarForMove = botGameStateService.ActiveFocusBar;
            _moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);

            //_moveState = _cultureResolverUtility.CreateBasicCultureMoveState(botGameStateService, BaseCityControlTokens);
            for (int tc = 0; tc < BaseAttackCount; tc++)
            {
                _moveState.AttacksAvailable.Add(tc, new AttackTargetMoveState() );
            }
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            //_cultureResolverUtility.UpdateBaseCultureGameStateForMove(_moveState, botGameStateService);
            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            //return _cultureResolverUtility.BuildGeneralisedCultureMoveSummary(summary, _moveState);
            return summary;
        }
    }
}
