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
            // TODO: this prompt is a hack - doesnt belong here!!

            var prompt = (moveState.CurrentAttackMoveId == moveState.AttacksAvailable.Count) ? "Press any key to proceed to my defensive reinforcements instructions" : "Press any key to advance to my next attack...";
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return new MoveStepActionData($"Fortunatley for my enemies, the attack on the {attckMove.AttackTargetType} was a failure, no further action is required for this attack",
                   new List<string>() { prompt });
        }
    }
}
