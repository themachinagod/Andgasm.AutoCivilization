using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class ReinforceFriendlyControlTokensActionRequest : StepActionBase, IReinforceFriendlyControlTokensActionRequest
    {
        private int _reinforementsavailable = 0;

        public ReinforceFriendlyControlTokensActionRequest() : base()
        {
            OperationType = OperationType.ActionRequest;
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
            return new MoveStepActionData($"Please reinforce {_reinforementsavailable} of my control tokens on the game board. Reinforce tokens closest to barbarians and/or rival spaces first, with highest terrain difficulty breaking ties",
                   new List<string>());
        }
    }
}
