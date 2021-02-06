using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class ConquerdNaturalWonderInformationRequestStep : StepActionBase, IConquerdNaturalWonderInformationRequestStep
    {
        private readonly IGlobalGameCache _globalGameCache;

        public ConquerdNaturalWonderInformationRequestStep(IGlobalGameCache globalGameCache) : base ()
        {
            OperationType = OperationType.InformationRequest;

            _globalGameCache = globalGameCache;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            return attckMove.BotIsWinning && attckMove.AttackTargetType == AttackTargetType.RivalControlToken;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: we need natural wonders for game
            //       currently hard wired!
            //       bit of a hack for multiple just now to avoid a request loop - fine for 2 just now

            var naturalWonders = new List<string> { "0. None", "1. Mt Everest", "2. Gran Mesa" };
            var prompt = "If there is a natural wonder on this city, the defated rival must relinquish the conquered natural wonder token and place it next to my leadersheet\n";
            return new MoveStepActionData(prompt +
                $"What is the name of the natural wonder that I have just conquered?",
                  naturalWonders);
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            // TODO: we need natural wonders for game
            //       currently hard wired!
            //       bit of a hack for multiple just now to avoid a request loop - fine for 2 just now

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            switch (Convert.ToInt32(input))
            {
                case 1:
                    moveState.ControlledNaturalWonders.Add("Mt. Everest");
                    moveState.NaturalWonderTokensControlledThisTurn = 1;
                    attckMove.HasStolenNaturalWonder = true;
                    break;
                case 2:
                    moveState.ControlledNaturalWonders.Add("Gran Mesa");
                    moveState.NaturalWonderTokensControlledThisTurn = 1;
                    attckMove.HasStolenNaturalWonder = true;
                    break;
                default:
                    moveState.NaturalWonderTokensControlledThisTurn = 0;
                    break;
            }
        }
    }
}
