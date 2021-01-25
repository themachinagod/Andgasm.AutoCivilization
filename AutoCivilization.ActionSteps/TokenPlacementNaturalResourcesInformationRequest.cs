using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using AutoCivilization.StateManagement;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalResourcesInformationRequestStep : StepActionBase, ITokenPlacementNaturalResourcesInformationRequestStep
    {
        public TokenPlacementNaturalResourcesInformationRequestStep() : base()
        {
            OperationType = OperationType.InformationRequest;
        }

        public override bool ShouldExecuteAction(BotMoveStateCache moveState)
        {
            var totalTokensPlaced = moveState.CityControlTokensPlacedThisTurn + moveState.TerritroyControlTokensPlacedThisTurn;
            return (totalTokensPlaced > 0) &&
                   (moveState.NaturalWonderTokensControlledThisTurn < totalTokensPlaced);
        }

        public override MoveStepActionData ExecuteAction(BotMoveStateCache moveState)
        {
            var preplaced = moveState.NaturalResourceTokensControlledThisTurn + moveState.NaturalWonderTokensControlledThisTurn;
            var maxTokensToBePlaced = (moveState.BaseCityControlTokensToBePlaced + moveState.TradeTokensAvailable[FocusType.Culture]) - preplaced;
            var options = Array.ConvertAll(Enumerable.Range(0, maxTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many natural resources did I manage to take control of this turn?",
                   options);
        }

        /// <summary>
        /// Update move state with how many natural wonders were recieved due to placed control tokens
        /// </summary>
        /// <param name="input">The number of natural wonder tokens the bot controlled this turn</param>
        /// <param name="moveState">The current move state to work from</param>
        public override BotMoveStateCache ProcessActionResponse(string input, BotMoveStateCache moveState)
        {
            var updatedMoveState = moveState.Clone();
            moveState.NaturalResourceTokensControlledThisTurn = Convert.ToInt32(input);
            return updatedMoveState;
        }
    }
}
