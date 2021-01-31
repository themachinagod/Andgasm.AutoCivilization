using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class TokenFlipEnemyActionRequestStep : StepActionBase, ITokenFlipEnemyActionRequestStep
    {
        public TokenFlipEnemyActionRequestStep() : base()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            return new MoveStepActionData("For each rival control token adjacent to my friendly territory, flip that token to its unreinforced side. If that token was already unreinforced, remove it from the board instead.",
                   new List<string>());
        }
    }
}
