using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class CultureResolverUtility : ICultureResolverUtility
    {
        public BotMoveState CreateBasicCultureMoveState(BotGameState botGameStateCache, int baseTokens)
        {
            var moveState = new BotMoveState();
            moveState.ControlledNaturalWonders = new List<string>(botGameStateCache.ControlledNaturalWonders);
            moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            moveState.BaseCityControlTokensToBePlaced = baseTokens;
            return moveState;
        }

        public void UpdateBaseCultureGameStateForMove(BotMoveState movesState, BotGameState botGameStateService)
        {
            var totalTokensPlacedThisTurn = movesState.CityControlTokensPlacedThisTurn + movesState.TerritroyControlTokensPlacedThisTurn;
            botGameStateService.ControlledSpaces += totalTokensPlacedThisTurn;
            botGameStateService.ControlledNaturalResources += (movesState.NaturalResourceTokensControlledThisTurn + movesState.NaturalWonderTokensControlledThisTurn);
            botGameStateService.ControlledNaturalWonders = new List<string>(movesState.ControlledNaturalWonders);
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(movesState.TradeTokensAvailable);
        }

        public string BuildGeneralisedCultureMoveSummary(string currentSummary, BotMoveState movesState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            if (movesState.CityControlTokensPlacedThisTurn > 0) sb.Append($"I updated my game state to show that I placed {movesState.CityControlTokensPlacedThisTurn} control token(s) next to my cities on the board\n");
            if (movesState.TerritroyControlTokensPlacedThisTurn > 0) sb.Append($"I updated my game state to show that I placed {movesState.TerritroyControlTokensPlacedThisTurn} control token(s) next to my friendly territory on the board\n");
            if (movesState.CultureTokensUsedThisTurn > 0) sb.Append($"I updated my game state to show that I used {movesState.CultureTokensUsedThisTurn} culture trade token(s) I had available to me to facilitate this move\n");
            if (movesState.CultureTokensUsedThisTurn < 0) sb.Append($"I updated my game state to show that I recieved {Math.Abs(movesState.CultureTokensUsedThisTurn)} culture trade token(s) for not using all my available control token placements which I may use in the future\n");
            if (movesState.NaturalResourceTokensControlledThisTurn > 0) sb.Append($"I updated my game state to show that I controlled {movesState.NaturalResourceTokensControlledThisTurn} natural resources which I may use for future construction projects\n");
            if (movesState.NaturalWonderTokensControlledThisTurn > 0)
            {
                sb.Append($"I updated my game state to show that I controlled the {string.Join(",", movesState.ControlledNaturalWonders)} natural wonder(s)\n");
                sb.Append($"As a result of controlling natural wonders on this turn I have recieved {movesState.NaturalWonderTokensControlledThisTurn} natural resources that I may use for future construction projects\n");
            }
            return sb.ToString();
        }
    }
}
