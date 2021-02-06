using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class FlightFocusCardMoveResolver : FocusCardMoveResolverBase, IMilitaryLevel4FocusCardMoveResolver
    {
        private const int BaseAttackPower = 8;
        private const int BaseAttackRange = 5;
        private const int BaseAttackCount = 2;
        private const int BaseMaxTargetPower = 10;
        private const int BaseReinforcementCount = 5;
        private const int BaseReinforcementAttackCost = 1;
        private const int BaseBarbarianAttackBonus = 2;

        private IMilitaryResolverUtility _militaryResolverUtility;

        private readonly IFocusBarTechnologyUpgradeResolver _focusBarTechnologyUpgradeResolver;

        public FlightFocusCardMoveResolver(IMilitaryResolverUtility militaryResolverUtility,
                                            IEnemyWithinAttackDistanceInformationRequestStep enemyWithinAttackDistanceInformationRequestStep,
                                            IEnemyTypeToAttackInformationRequestStep enemyTypeToAttackInformationRequestStep,
                                            IEnemyAttackPowerInformationRequestStep enemyAttackPowerInformationRequestStep,
                                            IAttackPrimaryResultActionRequestStep attackPrimaryResultActionRequestStep,
                                            IDefeatedBarbarianActionRequestStep defeatedBarbarianActionRequestStep,
                                            IConquerCityStateInformationRequestStep conquerCityStateActionRequestStep,
                                            IConquerdNaturalWonderInformationRequestStep conquerWorldNaturalInformationRequestStep,
                                            IDefeatedRivalControlTokenActionRequestStep defeatedRivalControlTokenActionRequestStep,
                                            IDefeatedCapitalCityActionRequestStep defeatedCapitalCityActionRequestStep,
                                            IConquerNonCapitalCityActionRequestStep conquerNonCapitalCityActionRequestStep,
                                            IConquerWorldWonderInformationRequestStep conquerWorldWonderInformationRequestStep,
                                            IFailedAttackActionRequestStep failedAttackActionRequestStep,
                                            ISupplementAttackPowerInformationRequestStep supplementAttackPowerInformationRequestStep,
                                            IReinforceFriendlyControlTokensActionRequest reinforceFriendlyControlTokensActionRequest,
                                            IReinforceFriendlyControlTokensInformationRequest reinforceFriendlyControlTokensInformationRequest,
                                            IFocusBarTechnologyUpgradeResolver focusBarTechnologyUpgradeResolver) : base()
        {
            _militaryResolverUtility = militaryResolverUtility;
            _focusBarTechnologyUpgradeResolver = focusBarTechnologyUpgradeResolver;

            FocusType = FocusType.Military;
            FocusLevel = FocusLevel.Lvl4;

            var loopSeed = 0;
            for (var attack = 0; attack < BaseAttackCount; attack++)
            {
                _actionSteps.Add(loopSeed, enemyWithinAttackDistanceInformationRequestStep);
                _actionSteps.Add(loopSeed + 1, enemyTypeToAttackInformationRequestStep);
                _actionSteps.Add(loopSeed + 2, enemyAttackPowerInformationRequestStep);
                _actionSteps.Add(loopSeed + 3, attackPrimaryResultActionRequestStep);
                _actionSteps.Add(loopSeed + 4, supplementAttackPowerInformationRequestStep);
                _actionSteps.Add(loopSeed + 5, conquerWorldNaturalInformationRequestStep);
                _actionSteps.Add(loopSeed + 6, conquerWorldWonderInformationRequestStep);
                _actionSteps.Add(loopSeed + 7, defeatedBarbarianActionRequestStep);
                _actionSteps.Add(loopSeed + 8, conquerCityStateActionRequestStep);
                _actionSteps.Add(loopSeed + 9, defeatedRivalControlTokenActionRequestStep);
                _actionSteps.Add(loopSeed + 10, defeatedCapitalCityActionRequestStep);
                _actionSteps.Add(loopSeed + 11, conquerNonCapitalCityActionRequestStep);
                _actionSteps.Add(loopSeed + 12, failedAttackActionRequestStep);
                loopSeed = _actionSteps.Count;
            }

            _actionSteps.Add(loopSeed, reinforceFriendlyControlTokensActionRequest);
            _actionSteps.Add(loopSeed + 1, reinforceFriendlyControlTokensInformationRequest);
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            _moveState = _militaryResolverUtility.CreateBasicMilitaryMoveState(botGameStateService, BaseAttackRange, BaseAttackPower, BaseMaxTargetPower, BaseAttackCount, BaseReinforcementCount, BaseReinforcementAttackCost, BaseBarbarianAttackBonus);
            _moveState.CanMoveOnWater = true;
        }

        public override string UpdateGameStateForMove(BotGameState gameState)
        {
            _militaryResolverUtility.UpdateBaseMilitaryGameStateForMove(_moveState, gameState, _moveState.CurrentAttackMoveId);

            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            // TODO: summaerise reinforcements
            //       summerise any techupgrade (if no action this turn)

            var summary = "To summarise my move I did the following;\n";
            return _militaryResolverUtility.BuildGeneralisedMilitaryMoveSummary(summary, _moveState);
        }
    }
}