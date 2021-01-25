using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using AutoCivilization.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalWonderControlledInformationRequestStep : StepActionBase, ITokenPlacementNaturalWonderControlledInformationRequestStep
    {
        public TokenPlacementNaturalWonderControlledInformationRequestStep() : base()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction(BotMoveStateCache moveState)
        {
            var totalTokensPlaced = moveState.CityControlTokensPlacedThisTurn + moveState.TerritroyControlTokensPlacedThisTurn;
            return (totalTokensPlaced > 0) &&
                   (moveState.NaturalResourceTokensControlledThisTurn < totalTokensPlaced);
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            // TODO: we need natural wonders for game
            //       currently hard wired!
            //       bit of a hack for multiple just now to avoid a request loop - fine for 2 just now

            var naturalWonders = new List<string> { "0. None", "1. Mt Everest", "2. Gran Mesa", "3. Both" };
            return new MoveStepActionData($"Which natural wonder(s) did I manage to take control of on this turn?",
                   naturalWonders);
        }

        /// <summary>
        /// Update move state with how natural wonders that were controlled this turn
        /// </summary>
        /// <param name="input">The code for the natural wonders visited specified by the user</param>
        /// <param name="moveState">The current move state to work from</param>
        public override BotMoveStateCache ProcessActionResponse(string input, BotMoveStateCache moveState)
        {
            var updatedMoveState = moveState.Clone();
            switch (Convert.ToInt32(input))
            {
                case 1:
                    updatedMoveState.ControlledNaturalWonders.Add("Mt. Everest");
                    updatedMoveState.NaturalWonderTokensControlledThisTurn = 1;
                    break;
                case 2:
                    updatedMoveState.ControlledNaturalWonders.Add("Gran Mesa");
                    updatedMoveState.NaturalWonderTokensControlledThisTurn = 1;
                    break;
                case 3:
                    updatedMoveState.ControlledNaturalWonders.Add("Gran Mesa");
                    updatedMoveState.ControlledNaturalWonders.Add("Mt. Everest");
                    updatedMoveState.NaturalWonderTokensControlledThisTurn = 2;
                    break;
                default:
                    updatedMoveState.NaturalWonderTokensControlledThisTurn = 0;
                    break;
            }
            return updatedMoveState;
        }
    }
}
