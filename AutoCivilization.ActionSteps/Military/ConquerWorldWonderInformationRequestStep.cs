using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Console;
using System;
using System.Linq;

namespace AutoCivilization.ActionSteps
{
    public class ConquerWorldWonderInformationRequestStep : StepActionBase, IConquerWorldWonderInformationRequestStep
    {
        private readonly IGlobalGameCache _globalGameCache;

        public ConquerWorldWonderInformationRequestStep(IGlobalGameCache globalGameCache) : base ()
        {
            OperationType = OperationType.InformationRequest;

            _globalGameCache = globalGameCache;
        }

        public override bool ShouldExecuteAction(BotMoveState moveState)
        {
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];           
            return attckMove.BotIsWinning && (attckMove.AttackTargetType == AttackTargetType.RivalCity ||
                (attckMove.AttackTargetType == AttackTargetType.RivalCapitalCity && moveState.FriendlyCityCount > moveState.BotPurchasedWonders.Count));
        }

        public override MoveStepActionData ExecuteAction(BotMoveState moveState)
        {
            // TODO: we show all wonders just now 
            //       this should only show the purchased wonders (less the wonders controlled by the bot)
            //       as we impl the round loop - as users tell us they purchased an unlocked wonder - we will add that wonder to the purchased collection - this way we know what wonders are on the board at any time

            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            var worldwonders = _globalGameCache.WonderCardsDeck.Select(x => $"{x.Id}. {x.Name}").ToList();
            worldwonders.Insert(0, "0. None");

            var prompt = "If there is a world wonder on this city, the defated rival must relinquish the conquered wonder card and place it next to my leadersheet\n";
            if (attckMove.AttackTargetType == AttackTargetType.RivalCapitalCity)
            {
                prompt += "The defeated rival must also move the cities wonder card token from their captial city to one of my strongest cities that has no current wonder\n";
            }
            return new MoveStepActionData(prompt +
                $"What is the name of the world wonder that I have just conquered?",
                  worldwonders);
        }

        public override void UpdateMoveStateForStep(string input, BotMoveState moveState)
        {
            if (input == "0") return;

            var selectedid = Convert.ToInt32(input);
            var wonder = _globalGameCache.WonderCardsDeck.First(x => x.Id == selectedid);
            var attckMove = moveState.AttacksAvailable[moveState.CurrentAttackMoveId - 1];
            attckMove.ConqueredWonder = wonder;
            moveState.BotPurchasedWonders.Add(wonder);
        }
    }
}
