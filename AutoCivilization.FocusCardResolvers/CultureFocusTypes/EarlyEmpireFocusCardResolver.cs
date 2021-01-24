using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class EarlyEmpireFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel1FocusCardMoveResolver
    {
        private const int BaseCityControlTokens = 2;

        public EarlyEmpireFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                                ITokenPlacementCityAdjacentActionRequestStep placementInstructionRequest,
                                                ITokenPlacementCityAdjacentInformationRequestStep placedInformationRequest,
                                                ITokenPlacementNaturalWonderControlledInformationRequestStep naturalWonderControlledInformationRequest,
                                                ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base(botMoveStateService)
        {
            FocusType = FocusType.Culture;
            FocusLevel = FocusLevel.Lvl1;

            _actionSteps.Add(0, placementInstructionRequest);
            _actionSteps.Add(1, placedInformationRequest);
            _actionSteps.Add(2, resourcesControlledInformationRequest);
            _actionSteps.Add(3, naturalWonderControlledInformationRequest);
        }

        public override void PrimeMoveState(BotGameStateCache botGameStateService)
        {
            _botMoveStateService.ControlledNaturalWonders = new List<string>(botGameStateService.ControlledNaturalWonders);
            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);
            _botMoveStateService.BaseCityControlTokensToBePlaced = BaseCityControlTokens;
        }

        /// <summary>
        /// Resolve the updated game state from the current move state
        /// Updtes game states controlled spaces count
        /// Update game states controlled natural wonders
        /// Update game states controlled natural resources
        /// Update game states trade tokens counters
        /// Increment the moves step counter
        /// </summary>
        /// <param name="botGameStateService">The game state to update for move</param>
        /// <returns>A textual summary of what the bot did this move</returns>
        public override string UpdateGameStateForMove(BotGameStateCache botGameStateService)
        {
            botGameStateService.ControlledSpaces += _botMoveStateService.CityControlTokensPlacedThisTurn;
            botGameStateService.ControlledNaturalResources += _botMoveStateService.NaturalResourceTokensControlledThisTurn;
            botGameStateService.ControlledNaturalWonders = new List<string>(_botMoveStateService.ControlledNaturalWonders);
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(_botMoveStateService.TradeTokensAvailable);
            _currentStep = -1;

            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            var summary = "To summarise my move I did the following;\n";
            if (_botMoveStateService.CityControlTokensPlacedThisTurn > 0) summary += $"I updated my game state to show that I placed {_botMoveStateService.CityControlTokensPlacedThisTurn} control tokens next to my cities on the board;\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.CultureTokensUsedThisTurn} culture trade tokens I had available to me to facilitate this move\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn < 0) summary += $"I updated my game state to show that I recieved {Math.Abs(_botMoveStateService.CultureTokensUsedThisTurn)} culture trade tokens which i may use in the future\n";
            if (_botMoveStateService.NaturalWonderTokensControlledThisTurn > 0) summary += $"I updated my game state to show that I controlled the {string.Join(",", _botMoveStateService.ControlledNaturalWonders)} natural wonders\n";
            if (_botMoveStateService.NaturalResourceTokensControlledThisTurn > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalResourceTokensControlledThisTurn} natural resources\n";
            return summary;
        }
    }
}
