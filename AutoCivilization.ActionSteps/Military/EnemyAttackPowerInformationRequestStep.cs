using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class EnemyAttackPowerInformationRequestStep : StepActionBase, IEnemyAttackPowerInformationRequestStep
    {
        public EnemyAttackPowerInformationRequestStep() : base ()
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
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var targetPowerComputeInstruction = GetAttackPowerComputationInstructionsForTarget(attckMove.AttackTargetType);

            return new MoveStepActionData($"In order to resolve the attack on the enemy {attckMove.AttackTargetType}, I will need to know the power of my target which I will need you to compute as follows;\n{targetPowerComputeInstruction}\n",
                   new List<string>() { "Please enter the total power of the attack target in numerical format" });
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            attckMove.AttackTargetPowerWithoutTradeTokens = Convert.ToInt32(input);
        }

        private string GetAttackPowerComputationInstructionsForTarget(AttackTargetType targetType)
        {
            if (targetType == AttackTargetType.Barbarian)
            {
                return "Please compute the barbarians attack power as follows;\nCurrent value of the terrain the barbarian is on (1-5)\nRoll a single dice to get the base power adjustment for combat (1-6)";
            }
            else if (targetType == AttackTargetType.RivalCity || targetType == AttackTargetType.RivalCapitalCity)
            {
                return "Please compute the rival cities attack power as follows;\nCurrent value of the terrain the city is built on multiplied by 2 (2-10)\nRoll a single dice to get the base power adjustment for combat (1-6)\nAdd any leadersheet bonuses\nAdd any military diplomacy bonuses\nAdd any military wonder bonuses\nAdd +1 for each adjacent reinforced control token\nDO NOT INCLUDE ANY AVAILABLE TRADE TOKENS YET";
            }
            else if (targetType == AttackTargetType.RivalControlToken)
            {
                return "Please compute the rival control tokens attack power as follows;\nCurrent value of the control tokens terrain (1-5)\nRoll a single dice to get the base power adjustment for combat (1-6)\nAdd any leadersheet bonuses\nAdd any military diplomacy bonuses\nAdd any military wonder bonuses\nAdd +1 for each adjacent reinforced control token\nAdd +1 if the control token being attacked is reinforced\nDO NOT INCLUDE ANY AVAILABLE TRADE TOKENS YET";
            }
            else if (targetType == AttackTargetType.CityState)
            {
                return "Please compute the city states attack power as follows;\nStatic city state bonus is 8\nRoll a single dice to get the base power adjustment for combat (1-6)";
            }
            return string.Empty;
        }
    }
}
