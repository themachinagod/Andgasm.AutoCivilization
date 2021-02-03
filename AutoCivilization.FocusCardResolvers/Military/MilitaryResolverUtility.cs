using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class MilitaryResolverUtility : IMilitaryResolverUtility
    {
        public BotMoveState CreateBasicMilitaryMoveState(BotGameState botGameStateCache, int baseRange, int basePower, int noOfAttacks)
        {
            var moveState = new BotMoveState();
            
            return moveState;
        }

        public void UpdateBaseMilitaryGameStateForMove(BotMoveState movesState, BotGameState botGameStateService, int attackIndex)
        {
            
        }

        public string BuildGeneralisedMilitaryMoveSummary(string currentSummary, BotGameState gameState, BotMoveState movesState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            
            return sb.ToString();
        }
    }
}
