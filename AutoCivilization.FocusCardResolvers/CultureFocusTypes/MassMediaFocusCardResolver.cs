using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;

namespace AutoCivilization.FocusCardResolvers
{
    public class MassMediaFocusCardResolver : FocusCardMoveResolverBase, ICultureLevel4FocusCardResolver
    {
        public MassMediaFocusCardResolver(IBotMoveStateCache botMoveStateService,
                                            ITokenFlipEnemyActionRequest tokenFlipEnemyActionRequest,
                                            ITokenPlacementCityAdjacentActionRequest placementInstructionRequest,
                                            ITokenPlacementCityAdjacentInformationRequest placedInformationRequest,
                                            ITokenPlacementNaturalWondersInformationRequest wondersControlledInformationRequest,
                                            ITokenPlacementNaturalResourcesInformationRequest resourcesControlledInformationRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl4;

            _actionSteps.Add(0, tokenFlipEnemyActionRequest);
            _actionSteps.Add(1, placementInstructionRequest);
            _actionSteps.Add(2, placedInformationRequest);
            _actionSteps.Add(3, wondersControlledInformationRequest);
            _actionSteps.Add(4, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.CultureTokensAvailable = botGameStateService.CultureTradeTokens;
            _botMoveStateService.BaseCityControlTokensToBePlaced = 4;
            _botMoveStateService.BaseTerritoryControlTokensToBePlaced = 0;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;
            botGameStateService.ControlledSpaces += totalTokensPlaced;
            botGameStateService.ControlledResources += _botMoveStateService.BaseTechnologyIncrease;
            botGameStateService.ControlledWonders += _botMoveStateService.NaturalWonderTokensControlled;
            botGameStateService.CultureTradeTokens = _botMoveStateService.CultureTokensAvailable;
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
