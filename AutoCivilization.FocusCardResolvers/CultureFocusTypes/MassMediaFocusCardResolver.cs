using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;

namespace AutoCivilization.FocusCardResolvers
{
    public class MassMediaFocusCardMoveResolver : FocusCardMoveResolverBase, ICultureLevel4FocusCardMoveResolver
    {
        private const int BaseCityControlTokens = 4;

        public MassMediaFocusCardMoveResolver(IBotMoveStateCache botMoveStateService,
                                            ITokenFlipEnemyActionRequestStep tokenFlipEnemyActionRequest,
                                            ITokenPlacementCityAdjacentActionRequestStep placementInstructionRequest,
                                            ITokenPlacementCityAdjacentInformationRequestStep placedInformationRequest,
                                            ITokenPlacementNaturalWonderControlledInformationRequestStep wondersControlledInformationRequest,
                                            ITokenPlacementNaturalResourcesInformationRequestStep resourcesControlledInformationRequest) : base(botMoveStateService)
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
            summary += $"I asked you to flip or remove any rival control tokens adjacent to my territory;\n";
            if (_botMoveStateService.CityControlTokensPlacedThisTurn > 0) summary += $"I updated my game state to show that I placed {_botMoveStateService.CityControlTokensPlacedThisTurn} control tokens next to my cities on the board;\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn > 0) summary += $"I updated my game state to show that I used {_botMoveStateService.CultureTokensUsedThisTurn} culture trade tokens I had available to me to facilitate this move\n";
            if (_botMoveStateService.CultureTokensUsedThisTurn < 0) summary += $"I updated my game state to show that I recieved {Math.Abs(_botMoveStateService.CultureTokensUsedThisTurn)} culture trade tokens which i may use in the future\n";
            if (_botMoveStateService.NaturalWonderTokensControlledThisTurn > 0) summary += $"I updated my game state to show that I controlled the {string.Join(",", _botMoveStateService.ControlledNaturalWonders)} natural wonders\n";
            if (_botMoveStateService.NaturalResourceTokensControlledThisTurn > 0) summary += $"I updated my game state to show that I controlled {_botMoveStateService.NaturalResourceTokensControlledThisTurn} natural resources\n";
            return summary;
        }
    }
}
