using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;

namespace AutoCivilization.FocusCardResolvers
{
    public class EarlyEmpireFocusCardResolver : FocusCardMoveResolverBase, ICultureLevel1FocusCardResolver
    {
        public EarlyEmpireFocusCardResolver(IBotMoveStateCache botMoveStateService,
                                            ITokenPlacementCityAdjacentActionRequest placementInstructionRequest,
                                            ITokenPlacementCityAdjacentInformationRequest placedInformationRequest,
                                            ITokenPlacementNaturalWondersInformationRequest wondersControlledInformationRequest,
                                            ITokenPlacementNaturalResourcesInformationRequest resourcesControlledInformationRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl1;

            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, wondersControlledInformationRequest);
            _actionSteps.Add(3, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.CultureTokensAvailable = botGameStateService.CultureTradeTokens;
            _botMoveStateService.BaseCityControlTokensToBePlaced = 2;
            _botMoveStateService.BaseTerritoryControlTokensToBePlaced = 0;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            // TODO: this seems to be the same for all culture focus cards - review as this has a small smell to it
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
            if (_botMoveStateService.CityControlTokensPlaced > 0) summary += $"I updated my game state to show that I placed {_botMoveStateService.CityControlTokensPlaced} control tokens next to my cities on the board;\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.CultureTokensUsedThisTurn} culture trade tokens I had available to me to facilitate this move\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn < 0) summary += $"I updated my game state to show that I recieved {Math.Abs(_botMoveStateService.CultureTokensUsedThisTurn)} culture trade tokens which i may use in the future\n";
            if (_botMoveStateService.NaturalWonderTokensControlled > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalWonderTokensControlled} natural wonders\n";
            if (_botMoveStateService.NaturalResourceTokensControlled > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalResourceTokensControlled} natural resources\n";
            return summary;
        }
    }
}
