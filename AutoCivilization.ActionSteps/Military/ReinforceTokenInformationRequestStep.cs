using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Linq;
using AutoCivilization.Console;

namespace AutoCivilization.ActionSteps
{
    public class ReinforceFriendlyControlTokensInformationRequest : StepActionBase, IReinforceFriendlyControlTokensInformationRequest
    {
        private int _reinforementsavailable = 0;

        public ReinforceFriendlyControlTokensInformationRequest() : base ()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var maxPlacements = moveState.BaseReinforcementCount;
            var attackcost = moveState.AttacksAvailable.Count(x => x.Value.IsTargetWithinRange) * moveState.BaseReinforcementAttackCost;
            _reinforementsavailable = maxPlacements - attackcost;
            _reinforementsavailable = Math.Max(0, _reinforementsavailable);
            return _reinforementsavailable > 0;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var options = Array.ConvertAll(Enumerable.Range(0, _reinforementsavailable + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many control tokens did you manage to reinforce on the board?",
                   options);
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            var reinforced = Convert.ToInt32(input);
            moveState.ControlTokensReinforcedThisTurn = reinforced;
        }
    }
}
