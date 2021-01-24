using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalWonderControlledInformationRequestStep : StepActionBase, ITokenPlacementNaturalWonderControlledInformationRequestStep
    {
        public TokenPlacementNaturalWonderControlledInformationRequestStep(IBotMoveStateCache botMoveStateService) : base(botMoveStateService)
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction()
        {
            return _botMoveStateService.NaturalWonderTokensControlledThisTurn > 0;
        }

        public override MoveStepActionData ExecuteAction()
        {
            // TODO: we need natural wonders for game
            //       currently hard wired!
            //       bit of a hack for multiple just now to avoid a request loop - fine for 2 just now

            var naturalWonders = new List<string> { "0. None", "1. Mt Everest", "2. Gran Mesa", "3. Both" };
            return new MoveStepActionData($"Which natural wonder(s) did I manage to take control of on this turn?",
                   naturalWonders);
        }

        public override void ProcessActionResponse(string input)
        {
            switch (Convert.ToInt32(input))
            {
                case 1:
                    _botMoveStateService.ControlledNaturalWonders.Add("Gran Mesa");
                    _botMoveStateService.NaturalWonderTokensControlledThisTurn = 1;
                    break;
                case 2:
                    _botMoveStateService.ControlledNaturalWonders.Add("Mt. Everest");
                    _botMoveStateService.NaturalWonderTokensControlledThisTurn = 1;
                    break;
                case 3:
                    _botMoveStateService.ControlledNaturalWonders.Add("Gran Mesa");
                    _botMoveStateService.ControlledNaturalWonders.Add("Mt. Everest");
                    _botMoveStateService.NaturalWonderTokensControlledThisTurn = 2;
                    break;
                default:
                    _botMoveStateService.NaturalWonderTokensControlledThisTurn = 0;
                    break;
            }
        }
    }
}
