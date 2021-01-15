using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class MassMediaFocusCardResolver : FocusCardResolverBase, ICultureLevel4FocusCardResolver
    {
        public MassMediaFocusCardResolver(IBotGameStateService botGameStateService,
                                            IBotMoveStateService botMoveStateService,
                                            ITokenPlacementCityAdjacentActionRequest placementInstructionRequest,
                                            ITokenPlacementCityAdjacentInformationRequest placedInformationRequest,
                                            ITokenPlacementNaturalWondersInformationRequest wondersControlledInformationRequest,
                                            ITokenPlacementNaturalResourcesInformationRequest resourcesControlledInformationRequest) : base(botGameStateService, botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl4;

            // TODO: we need the flip action at index 1!
            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, wondersControlledInformationRequest);
            _actionSteps.Add(3, resourcesControlledInformationRequest);
        }

        public override void InitialiseMoveState()
        {
            _botMoveStateService.CultureTokensAvailable = _botGameStateService.CultureTradeTokens;
            _botMoveStateService.BaseCityControlTokensToBePlaced = 3;
            _botMoveStateService.BaseTerritoryControlTokensToBePlaced = 0;
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
        }
    }
}
