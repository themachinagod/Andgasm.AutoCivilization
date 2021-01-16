using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class MassMediaFocusCardResolver : FocusCardResolverBase, ICultureLevel4FocusCardResolver
    {
        public MassMediaFocusCardResolver(IBotGameStateService botGameStateService,
                                            IBotMoveStateService botMoveStateService,
                                            ITokenFlipEnemyActionRequest tokenFlipEnemyActionRequest,
                                            ITokenPlacementCityAdjacentActionRequest placementInstructionRequest,
                                            ITokenPlacementCityAdjacentInformationRequest placedInformationRequest,
                                            ITokenPlacementNaturalWondersInformationRequest wondersControlledInformationRequest,
                                            ITokenPlacementNaturalResourcesInformationRequest resourcesControlledInformationRequest) : base(botGameStateService, botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl4;

            _actionSteps.Add(0, tokenFlipEnemyActionRequest);
            _actionSteps.Add(1, placementInstructionRequest);
            _actionSteps.Add(2, placedInformationRequest);
            _actionSteps.Add(3, wondersControlledInformationRequest);
            _actionSteps.Add(4, resourcesControlledInformationRequest);
        }

        public override IStepAction GetNextStep()
        {
            if (_currentStep == -1)
            {
                _botMoveStateService.CultureTokensAvailable = _botGameStateService.CultureTradeTokens;
                _botMoveStateService.BaseCityControlTokensToBePlaced = 4;
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
            _botGameStateService.ControlledResources += _botMoveStateService.BaseTechnologyIncrease;
            _botGameStateService.ControlledWonders += _botMoveStateService.NaturalWonderTokensControlled;
            _botGameStateService.CultureTradeTokens += cultureTokensIncrement;
        }
    }
}
