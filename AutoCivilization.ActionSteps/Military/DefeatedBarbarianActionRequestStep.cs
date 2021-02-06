using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class DefeatedBarbarianActionRequestStep : StepActionBase, IDefeatedBarbarianActionRequestStep
    {
        private readonly ISmallestTradeTokenPileResolver _smallestTradeTokenPileResolver;

        public DefeatedBarbarianActionRequestStep(ISmallestTradeTokenPileResolver smallestTradeTokenPileResolver) : base ()
        {
            OperationType = OperationType.InformationRequest;

            _smallestTradeTokenPileResolver = smallestTradeTokenPileResolver;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.Barbarian;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: this prompt is a hack - doesnt belong here!!

            var prompt = (moveState.CurrentAttackMoveId == moveState.AttacksAvailable.Count) ? "Press any key to proceed to my defensive reinforcements instructions" : "Press any key to advance to my next attack...";
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"My attack on the inferior {attckMove.AttackTargetType} was a complete success. As this barbarian unit has been vanquished, please remove it from the game board",
                   new List<string>() { prompt });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var smallestpile = _smallestTradeTokenPileResolver.ResolveSmallestTokenPile(moveState.ActiveFocusBarForMove, moveState.TradeTokensAvailable);
            moveState.SmallestTradeTokenPileType = smallestpile;
            moveState.TradeTokensAvailable[moveState.SmallestTradeTokenPileType] += 1;
            moveState.TradeTokensAvailable[FocusType.Military] -= attckMove.BotSpentMilitaryTradeTokensThisTurn;
        }
    }
}
