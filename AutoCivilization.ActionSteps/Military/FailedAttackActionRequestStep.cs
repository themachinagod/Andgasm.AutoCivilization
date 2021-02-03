using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class FailedAttackActionRequestStep : StepActionBase, IFailedAttackActionRequestStep
    {
        public FailedAttackActionRequestStep() : base ()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return attckMove.IsTargetWithinRange && !attckMove.BotIsWinning;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"My attack on the {attckMove.AttackTargetType} failed, no further action is required for this attack.",
                   new List<string>() { });
        }
    }
}
