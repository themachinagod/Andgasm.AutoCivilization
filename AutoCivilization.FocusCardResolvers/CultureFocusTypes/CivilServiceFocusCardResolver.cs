using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class CivilServiceFocusCardResolver : FocusCardResolverBase, ICultureLevel3FocusCardResolver
    {
        public CivilServiceFocusCardResolver(IBotGameStateService botGameStateService,
                                             IBotMoveStateService botMoveStateService,
                                             ITokenPlacementCityAdjacentActionRequest placementCityInstructionRequest,
                                             ITokenPlacementTerritoryAdjacentActionRequest placementTerritoryInstructionRequest,
                                             ITokenPlacementCityAdjacentInformationRequest placedCityInformationRequest,
                                             ITokenPlacementTerritoryAdjacentInformationRequest placedTerritoryInformationRequest,
                                             ITokenPlacementNaturalWondersInformationRequest wondersControlledInformationRequest,
                                             ITokenPlacementNaturalResourcesInformationRequest resourcesControlledInformationRequest) : base(botGameStateService, botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl3;

            _actionSteps.Add(0, placementCityInstructionRequest);
            _actionSteps.Add(1, placementTerritoryInstructionRequest);
            _actionSteps.Add(2, placedCityInformationRequest);
            _actionSteps.Add(3, placedTerritoryInformationRequest);
            _actionSteps.Add(4, wondersControlledInformationRequest);
            _actionSteps.Add(5, resourcesControlledInformationRequest);
        }

        public override IStepAction GetNextStep()
        {
            if (_currentStep == -1)
            {
                _botMoveStateService.CultureTokensAvailable = _botGameStateService.CultureTradeTokens;
                _botMoveStateService.BaseCityControlTokensToBePlaced = 3;
                _botMoveStateService.BaseTerritoryControlTokensToBePlaced = 1;
            }
            return base.GetNextStep();
        }

        public override void Resolve()
        {
            // TODO: this seems to be the same for all culture focus cards - review as this has a small smell to it
            var cultureTokensIncrement = _botMoveStateService.BaseCityControlTokensToBePlaced - _botMoveStateService.CityControlTokensPlaced;
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;

            _botGameStateService.ControlledSpaces += totalTokensPlaced;
            _botGameStateService.ControlledResources += _botMoveStateService.BaseTechnologyIncrease;
            _botGameStateService.ControlledWonders += _botMoveStateService.NaturalWonderTokensControlled;
            _botGameStateService.CultureTradeTokens += cultureTokensIncrement;

            _currentStep = -1;
        }
    }
}
