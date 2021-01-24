using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class CultureResolverUtility : ICultureResolverUtility
    {
        private readonly IBotMoveStateCache _botMoveStateService;

        public CultureResolverUtility(IBotMoveStateCache botMoveStateService)
        {
            _botMoveStateService = botMoveStateService;
        }

        public void PrimeBaseCultureState(BotGameStateCache botGameStateCache, int baseTokens)
        {
            _botMoveStateService.ControlledNaturalWonders = new List<string>(botGameStateCache.ControlledNaturalWonders);
            _botMoveStateService.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            _botMoveStateService.BaseCityControlTokensToBePlaced = baseTokens;
        }

        public void UpdateBaseGameStateForMove(BotGameStateCache botGameStateService)
        {
            var totalTokensPlacedThisTurn = _botMoveStateService.CityControlTokensPlacedThisTurn + _botMoveStateService.TerritroyControlTokensPlacedThisTurn;
            botGameStateService.ControlledSpaces += totalTokensPlacedThisTurn;
            botGameStateService.ControlledNaturalResources += _botMoveStateService.NaturalResourceTokensControlledThisTurn;
            botGameStateService.ControlledNaturalWonders = new List<string>(_botMoveStateService.ControlledNaturalWonders);
            botGameStateService.TradeTokens = new Dictionary<FocusType, int>(_botMoveStateService.TradeTokensAvailable);
        }

        public string BuildGeneralisedCultureMoveSummary(string currentSummary)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            if (_botMoveStateService.CityControlTokensPlacedThisTurn > 0) sb.Append($"I updated my game state to show that I placed {_botMoveStateService.CityControlTokensPlacedThisTurn} control tokens next to my cities on the board\n");
            if (_botMoveStateService.TerritroyControlTokensPlacedThisTurn > 0) sb.Append($"I updated my game state to show that I placed {_botMoveStateService.TerritroyControlTokensPlacedThisTurn} control tokens next to my friendly territory on the board\n");
            if (_botMoveStateService.CultureTokensUsedThisTurn > 0) sb.Append($"I updated my game state to show that I used {_botMoveStateService.CultureTokensUsedThisTurn} culture trade tokens I had available to me to facilitate this move\n");
            if (_botMoveStateService.CultureTokensUsedThisTurn < 0) sb.Append($"I updated my game state to show that I recieved {Math.Abs(_botMoveStateService.CultureTokensUsedThisTurn)} culture trade tokens which i may use in the future\n");
            if (_botMoveStateService.NaturalWonderTokensControlledThisTurn > 0) sb.Append($"I updated my game state to show that I controlled the {string.Join(",", _botMoveStateService.ControlledNaturalWonders)} natural wonders\n");
            if (_botMoveStateService.NaturalResourceTokensControlledThisTurn > 0) sb.Append($"I updated my game state to show that I controlled {_botMoveStateService.NaturalResourceTokensControlledThisTurn} natural resources\n");
            return sb.ToString();
        }
    }
}
