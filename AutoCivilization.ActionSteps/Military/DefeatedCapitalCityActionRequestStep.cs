using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class DefeatedCapitalCityActionRequestStep : StepActionBase, IDefeatedCapitalCityActionRequestStep
    {
        private readonly ISmallestTradeTokenPileResolver _smallestTradeTokenPileResolver;

        public DefeatedCapitalCityActionRequestStep(ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver) : base ()
        {
            OperationType = OperationType.ActionRequest;

            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.RivalCapitalCity;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: diplomacy cards impact??
            //       need to think about player dip cards some more - thebot should lose any it has for an attacked rival - but unlike city state dip cards - the bot is meant to gain specific bonus!

            // TODO: this prompt is a hack - doesnt belong here!!

            var prompt = (moveState.CurrentAttackMoveId == moveState.AttacksAvailable.Count) ? "Press any key to proceed to my defensive reinforcements instructions" : "Press any key to advance to my next attack...";
            return new MoveStepActionData($"My attack on the rivals capital city was successful and now they pay homage to my glorious kingdom\nAs this is a capital city DO NOT replace the city on the board\nThe defeated rival must return 2 trade tokens to the supply, to my gain...",
                   new List<string>() { prompt });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var smallestpile = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(moveState.ActiveFocusBarForMove, moveState.TradeTokensAvailable);
            moveState.SmallestTradeTokenPileType = smallestpile;
            moveState.TradeTokensAvailable[moveState.SmallestTradeTokenPileType] += 2;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
