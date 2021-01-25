using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using AutoCivilization.StateManagement;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class TokenPlacementNaturalWonderCountInformationRequestStep : StepActionBase, ITokenPlacementNaturalWonderCountInformationRequestStep
    {
        public TokenPlacementNaturalWonderCountInformationRequestStep() : base()
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
            // TODO: there will be a max limit here based on the number of natural wonders (hard wired to 2 for 2 player game)
            var preplaced = moveState.NaturalResourceTokensControlledThisTurn + moveState.NaturalWonderTokensControlledThisTurn;
            var maxTokensToBePlaced = (moveState.BaseCityControlTokensToBePlaced + moveState.TradeTokensAvailable[FocusType.Culture]) - preplaced;
            maxTokensToBePlaced = maxTokensToBePlaced > 2 ? 2 : maxTokensToBePlaced;
            var options = Array.ConvertAll(Enumerable.Range(0, maxTokensToBePlaced + 1).ToArray(), ele => ele.ToString());
            return new MoveStepActionData("How many natural wonders did I manage to take control of on this turn?",
                   options);
        }

        /// <summary>
        /// Update move state with how many natural rsources were recieved due to placed control tokens
        /// </summary>
        /// <param name="input">The number of natural wonder tokens the bot controlled this turn</param>
        public override void UpdateMoveStateForUserResponse(string input, BotMoveStateCache moveState)
        {
            moveState.NaturalWonderTokensControlledThisTurn = Convert.ToInt32(input);
        }
    }
}
