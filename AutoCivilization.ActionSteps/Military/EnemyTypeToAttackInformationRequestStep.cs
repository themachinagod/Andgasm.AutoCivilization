using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class EnemyTypeToAttackInformationRequestStep : StepActionBase, IEnemyTypeToAttackInformationRequestStep
    {
        public EnemyTypeToAttackInformationRequestStep() : base ()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return attckMove.IsTargetWithinRange;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: variance on the applicable types - currently hardwired for lvl 1 & 2

            return new MoveStepActionData($"What type of enemy unit will my attack be aimed at?",
                   new List<string>() { "1. Barbarian", $"2. Rival player city with defense of {moveState.BaseMaxTargetPower} or less", "3. Rival control token" });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            // TODO: variance on the applicable types - currently hardwired for lvl 1 & 2

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            switch (input)
            {
                case "1":
                    attckMove.AttackTargetType =  AttackTargetType.Barbarian;
                    break;
                case "2":
                    attckMove.AttackTargetType = AttackTargetType.RivalCity;
                    break;
                case "3":
                    attckMove.AttackTargetType = AttackTargetType.RivalControlToken;
                    break;
                case "4":
                    attckMove.AttackTargetType = AttackTargetType.CityState;
                    break;
                case "5":
                    attckMove.AttackTargetType = AttackTargetType.RivalCapitalCity;
                    break;
            }
        }
    }
}
