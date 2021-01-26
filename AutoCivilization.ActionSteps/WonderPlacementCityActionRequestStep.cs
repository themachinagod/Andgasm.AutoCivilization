using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System.Collections.Generic;

namespace AutoCivilization.ActionSteps
{
    public class WonderPlacementCityActionRequestStep : StepActionBase, IWonderPlacementCityActionRequestStep
    {
        public WonderPlacementCityActionRequestStep() : base()
        {
            OperationType = OperationType.ActionRequest;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            //  we need to attempt to build a wonder from our wonder deck
            //      use our total production capacity :- BaseProduction + (TotalResources * 2) + Industry Trade Tokens
            //      if our PC > lowest cost wonder && CityCount > WonderCount
            //          ask the user to place purchased wonder token under strongest city with no wonder
            //          add wonder to move state collection
            //          add spent resources and trade tokens to move state
            //      other wise - do nout
            //var productionPoints = (moveState.BaseProductionPoints + (moveState.NaturalResourcesToSpend * 2) + moveState.TradeTokensAvailable[FocusType.Industry]);

            return true;
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            var maxPlacements = moveState.BaseCityControlTokensToBePlaced + moveState.TradeTokensAvailable[FocusType.Culture];
            return new MoveStepActionData($"Please place {maxPlacements} control tokens on spaces adjacent to any of my cities using the following placement priority rules:\nNatural wonder\nResource token\nVacant barbarian spawn point\nAdjacent to the most cities\nAdjacent to city closest to maturity\nHighest terrain difficulty\nFor each token that cannot be placed, I will recieve 1 culture trade token to be used in the future",
                   new List<string>());
        }
    }
}
