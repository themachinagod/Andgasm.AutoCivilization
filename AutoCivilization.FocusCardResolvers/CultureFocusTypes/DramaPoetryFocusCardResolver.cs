using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class DramaPoetryFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel2FocusCardMoveResolver
    {
        public DramaPoetryFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                            ITokenPlacementCityAdjacentActionRequestStep placementInstructionRequest,
                                            ITokenPlacementCityAdjacentInformationRequestStep placedInformationRequest,
                                            ITokenPlacementNaturalWondersInformationRequestStep wondersControlledInformationRequest,
                                            ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl2;

            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, wondersControlledInformationRequest);
            _actionSteps.Add(3, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.TradeTokensAvailable[FocusType.Culture] = botGameStateService.TradeTokens[FocusType.Culture]; 
            _botMoveStateService.BaseCityControlTokensToBePlaced = 3;
            _botMoveStateService.BaseTerritoryControlTokensToBePlaced = 0;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            // TODO: this seems to be the same for all culture focus cards - review as this has a small smell to it
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;
            botGameStateService.ControlledSpaces += totalTokensPlaced;
            botGameStateService.ControlledResources += _botMoveStateService.BaseTechnologyIncrease;
            botGameStateService.ControlledWonders += _botMoveStateService.NaturalWonderTokensControlled;
            botGameStateService.TradeTokens[FocusType.Culture] = _botMoveStateService.TradeTokensAvailable[FocusType.Culture];
            _currentStep = -1;

            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            if (_botMoveStateService.CityControlTokensPlaced > 0) summary += $"I updated my game state to show that I placed {_botMoveStateService.CityControlTokensPlaced} controll tokens next to my cities on the board\n";
            if (_botMoveStateService.NaturalWonderTokensControlled > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalWonderTokensControlled} natural wonders\n";
            if (_botMoveStateService.NaturalResourceTokensControlled > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalResourceTokensControlled} natural resources\n";
            return summary;
        }
    }
}
