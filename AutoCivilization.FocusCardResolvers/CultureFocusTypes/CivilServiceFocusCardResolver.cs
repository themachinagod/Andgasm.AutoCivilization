using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;

namespace AutoCivilization.FocusCardResolvers
{
    public class CivilServiceFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel3FocusCardMoveResolver
    {
        public CivilServiceFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                                 ITokenPlacementCityAdjacentActionRequest placementCityInstructionRequest,
                                                 ITokenPlacementTerritoryAdjacentActionRequest placementTerritoryInstructionRequest,
                                                 ITokenPlacementCityAdjacentInformationRequest placedCityInformationRequest,
                                                 ITokenPlacementTerritoryAdjacentInformationRequest placedTerritoryInformationRequest,
                                                 ITokenPlacementNaturalWondersInformationRequest wondersControlledInformationRequest,
                                                 ITokenPlacementNaturalResourcesInformationRequest resourcesControlledInformationRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl3;

            _actionSteps.Add(0, placementCityInstructionRequest);
            _actionSteps.Add(1, placedCityInformationRequest);
            _actionSteps.Add(2, placementTerritoryInstructionRequest);
            _actionSteps.Add(3, placedTerritoryInformationRequest);
            _actionSteps.Add(4, wondersControlledInformationRequest);
            _actionSteps.Add(5, resourcesControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.CultureTokensAvailable = botGameStateService.CultureTradeTokens;
            _botMoveStateService.BaseCityControlTokensToBePlaced = 3;
            _botMoveStateService.BaseTerritoryControlTokensToBePlaced = 1;
        }

        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            var totalTokensPlaced = _botMoveStateService.CityControlTokensPlaced + _botMoveStateService.TerritroyControlTokensPlaced;
            botGameStateService.ControlledSpaces += totalTokensPlaced;
            botGameStateService.ControlledResources += _botMoveStateService.NaturalResourceTokensControlled;
            botGameStateService.ControlledWonders += _botMoveStateService.NaturalWonderTokensControlled;
            botGameStateService.CultureTradeTokens = _botMoveStateService.CultureTokensAvailable;
            _currentStep = -1;

            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            if (_botMoveStateService.CityControlTokensPlaced > 0) summary += $"I updated my game state to show that I placed {_botMoveStateService.CityControlTokensPlaced} control tokens next to my cities on the board;\n";
            if (_botMoveStateService.TerritroyControlTokensPlaced > 0) summary += $"I updated my game state to show that I placed {_botMoveStateService.TerritroyControlTokensPlaced} control tokens next to my friendly territory on the board;\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.CultureTokensUsedThisTurn} culture trade tokens giving me {_botMoveStateService.CultureTokensAvailable} to use later\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn < 0) summary += $"I updated my game state to show that I recieved {Math.Abs(_botMoveStateService.CultureTokensUsedThisTurn)} culture trade tokens giving me {_botMoveStateService.CultureTokensAvailable} to use later\n";
            if (_botMoveStateService.NaturalWonderTokensControlled > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalWonderTokensControlled} natural wonders\n";
            if (_botMoveStateService.NaturalResourceTokensControlled > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalResourceTokensControlled} natural resources\n";
            return summary;
        }
    }
}
