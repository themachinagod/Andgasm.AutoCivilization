using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class EnemyWithinAttackDistanceInformationRequestStep : StepActionBase, IEnemyWithinAttackDistanceInformationRequestStep
    {
        public EnemyWithinAttackDistanceInformationRequestStep() : base ()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: variance on the applicable types - currently hardwired for lvl 1 & 2
            if (moveState.CurrentAttackMoveId < moveState.AttacksAvailable.Count) moveState.CurrentAttackMoveId++;
            return new MoveStepActionData($"Is there any undefeated enemies of the following types within {moveState.BaseAttackRange} spaces of my friendly territory?\nBarbarian\nRival player city with defense of {moveState.BaseMaxTargetPower} or less\nRival control token",
                   new List<string>() { "1. Yes", "2. No" });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];

            bool targetIdentified = input == "1";
            attckMove.IsTargetWithinRange = targetIdentified;
        }
    }
}
