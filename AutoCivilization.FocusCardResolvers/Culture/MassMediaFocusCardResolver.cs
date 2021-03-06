﻿using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.FocusCardResolvers
{
    public class MassMediaFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel4FocusCardMoveResolver
    {
        private const int BaseCityControlTokens = 4;

        private readonly ICultureResolverUtility _cultureResolverUtility;

        public MassMediaFocusCardMoveResolver(ICultureResolverUtility cultureResolverUtility,
                                              ITokenFlipEnemyActionRequestStep tokenFlipEnemyActionRequest,
                                              ITokenPlacementCityAdjacentActionRequestStep placementInstructionRequest,
                                              ITokenPlacementCityAdjacentInformationRequestStep placedInformationRequest,
                                              ITokenPlacementNaturalWonderControlledInformationRequestStep wondersControlledInformationRequest,
                                              ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base()
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl4;

            _cultureResolverUtility = cultureResolverUtility;

            _actionSteps.Add(0, tokenFlipEnemyActionRequest);
            _actionSteps.Add(1, placementInstructionRequest);
            _actionSteps.Add(2, placedInformationRequest);
            _actionSteps.Add(3, wondersControlledInformationRequest);
            _actionSteps.Add(4, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            _moveState = _cultureResolverUtility.CreateBasicCultureMoveState(botGameStateService, BaseCityControlTokens);
        }

        public override string UpdateGameStateForMove(BotGameState botGameStateService)
        {
            _cultureResolverUtility.UpdateBaseCultureGameStateForMove(_moveState, botGameStateService);
            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            summary += $"I asked you to flip or remove any rival control tokens adjacent to my territory\n";
            return _cultureResolverUtility.BuildGeneralisedCultureMoveSummary(summary, _moveState);
        }
    }
}
