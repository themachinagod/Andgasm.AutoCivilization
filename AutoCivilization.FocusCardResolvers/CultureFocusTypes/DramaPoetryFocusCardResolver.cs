﻿using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class DramaPoetryFocusCardResolver : FocusCardResolverBase, ICultureLevel2FocusCardResolver
    {
        public DramaPoetryFocusCardResolver(IBotGameStateService botGameStateService,
                                            IBotMoveStateService botMoveStateService,
                                            ITokenPlacementCityAdjacentActionRequest placementInstructionRequest,
                                            ITokenPlacementCityAdjacentInformationRequest placedInformationRequest,
                                            ITokenPlacementNaturalWondersInformationRequest wondersControlledInformationRequest,
                                            ITokenPlacementNaturalResourcesInformationRequest resourcesControlledInformationRequest) : base(botGameStateService, botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl2;

            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, wondersControlledInformationRequest);
            _actionSteps.Add(3, resourcesControlledInformationRequest);
        }

        public override IStepAction GetNextStep()
        {
            if (_currentStep == -1)
            {
                _botMoveStateService.CultureTokensAvailable = _botGameStateService.CultureTradeTokens;
                _botMoveStateService.BaseCityControlTokensToBePlaced = 3;
                _botMoveStateService.BaseTerritoryControlTokensToBePlaced = 0;
            }
            return base.GetNextStep();
        }

        public override void Resolve()
        {
            // TODO: this seems to be the same for all culture focus cards - review as this has a small smell to it
            var cultureTokensIncrement = _botMoveStateService.BaseCityControlTokensToBePlaced - _botMoveStateService.CityControlTokensPlaced;
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;

            _botGameStateService.ControlledSpaces += totalTokensPlaced;
            _botGameStateService.ControlledResources += _botMoveStateService.NaturalResourceTokensControlled;
            _botGameStateService.ControlledWonders += _botMoveStateService.NaturalWonderTokensControlled;
            _botGameStateService.CultureTradeTokens += cultureTokensIncrement;

            _currentStep = -1;
        }
    }
}
