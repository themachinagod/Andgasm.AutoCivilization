using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class MilitaryResolverUtility : IMilitaryResolverUtility
    {
        public BotMoveState CreateBasicMilitaryMoveState(BotGameState botGameStateCache, int baseRange, int basePower, int baseMaxTargetrPower, int noOfAttacks, int baseReinforcments, int baseReinforcemenstCost, int baseBarbarianBonus)
        {
            var _moveState = new BotMoveState();
            _moveState.BaseAttackPower = basePower;
            _moveState.BaseAttackRange = baseRange;
            _moveState.BaseMaxTargetPower = baseMaxTargetrPower;
            _moveState.BaseReinforcementCount = baseReinforcments;
            _moveState.BaseReinforcementAttackCost = baseReinforcemenstCost;
            _moveState.BaseBarbarianAttackBonus = baseBarbarianBonus;

            _moveState.ActiveFocusBarForMove = botGameStateCache.ActiveFocusBar;
            _moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateCache.TradeTokens);
            _moveState.BotPurchasedWonders = new List<WonderCardModel>(botGameStateCache.BotPurchasedWonders);
            _moveState.AllPurchasedWonders = new List<WonderCardModel>(botGameStateCache.BotPurchasedWonders);
            _moveState.CityStatesDiplomacyCardsHeld = new List<CityStateModel>(botGameStateCache.CityStateDiplomacyCardsHeld);
            _moveState.ConqueredCityStateTokensHeld = new List<CityStateModel>(botGameStateCache.ConqueredCityStateTokensHeld);
            _moveState.ControlledNaturalWonders = new List<string>(botGameStateCache.ControlledNaturalWonders);

            _moveState.FriendlyCityCount = botGameStateCache.FriendlyCityCount;

            for (int tc = 0; tc < noOfAttacks; tc++)
            {
                _moveState.AttacksAvailable.Add(tc, new AttackTargetMoveState());
            }
            return _moveState;
        }

        public void UpdateBaseMilitaryGameStateForMove(BotMoveState movesState, BotGameState gameState, int attackIndex)
        {
            // TODO: 
            // something to do with visited player cities & diplomacy cards
            // update focus bar if free upgrade (no wonder or city built)??
            // update no of battles won/lost (some additional info) 

            gameState.TradeTokens = new Dictionary<FocusType, int>(movesState.TradeTokensAvailable);
            gameState.BotPurchasedWonders = new List<WonderCardModel>(movesState.BotPurchasedWonders);
            gameState.ConqueredCityStateTokensHeld = new List<CityStateModel>(movesState.ConqueredCityStateTokensHeld);
            gameState.CityStateDiplomacyCardsHeld = new List<CityStateModel>(movesState.CityStatesDiplomacyCardsHeld);
            gameState.ControlledNaturalWonders = new List<string>(movesState.ControlledNaturalWonders);

            gameState.FriendlyCityCount = movesState.FriendlyCityCount;
            gameState.ControlledSpaces += movesState.CityControlTokensPlacedThisTurn;
        }

        public string BuildGeneralisedMilitaryMoveSummary(string currentSummary, BotMoveState moveState)
        {
            StringBuilder sb = new StringBuilder(currentSummary);
            sb.AppendLine($"I executed {moveState.AttacksAvailable.Count} attacks againt enemy targets;");
            var attackOrdinal = 1;
            foreach (var attackmove in moveState.AttacksAvailable)
            {
                // HACK: currently we dont know what natural wonder was acquired for a specific attack!!
                // HACK: smallest pile issues!!

                sb.AppendLine();
                if (attackmove.Value.IsTargetWithinRange)
                {
                    var winLoss = attackmove.Value.BotIsWinning ? "defeated" : "lost in battle to";
                    sb.AppendLine($"Attack {attackOrdinal} : I {winLoss} a {attackmove.Value.AttackTargetType} as follows;");
                    sb.AppendLine($"\tTarget total power: {attackmove.Value.AttackTargetPowerWithoutTradeTokens}");
                    sb.AppendLine($"\tMy base power: {moveState.BaseAttackPower}");
                    sb.AppendLine($"\tMy dice roll: {attackmove.Value.DiceRollAttackPower}");
                    sb.AppendLine($"\tMy wonder cards bonus: {moveState.BotPurchasedWonders.Where(x => x.Type == FocusType.Military).Count()}");
                    sb.AppendLine($"\tMy diplomacy cards bonus: {moveState.CityStatesDiplomacyCardsHeld.Where(x => x.Type == FocusType.Military).Count()}");
                    sb.AppendLine($"\tMy military trade tokens: {moveState.TradeTokensAvailable[FocusType.Military]}");
                    if (attackmove.Value.AttackTargetType == AttackTargetType.Barbarian) sb.AppendLine($"\tMy barbarian bonus: {moveState.BaseBarbarianAttackBonus}");

                    if (attackmove.Value.BotIsWinning)
                    {
                        sb.AppendLine($"In order to win this battle I had to expend {attackmove.Value.BotSpentMilitaryTradeTokensThisTurn} military trade token(s)");

                        if (attackmove.Value.AttackTargetType == AttackTargetType.Barbarian)
                        {
                            sb.AppendLine($"The barbarian was removed from the board");
                            sb.AppendLine($"I recieved 1 trade token for this victory which I assigned to my {moveState.SmallestTradeTokenPileType} focus slot");
                        }
                        if (attackmove.Value.AttackTargetType == AttackTargetType.RivalControlToken)
                        {
                            sb.AppendLine($"The rival control token was replaced with one of my own on the board");
                            if (attackmove.Value.HasStolenNaturalWonder) sb.AppendLine($"As the battle took place on the { moveState.ControlledNaturalWonders.LastOrDefault()} natural wonder, I have now taken control of this natural wonder token and any bnous it provides");
                        }
                        if (attackmove.Value.AttackTargetType == AttackTargetType.CityState)
                        {
                            sb.AppendLine($"The city state was replaced with one of my own cities on the board");
                            sb.AppendLine($"The city state token was placed on my leadersheet and I will be able to utilise any bonuses it grants in the future");
                            sb.AppendLine($"I have returned my diplomacy card for the city state and all players should have done the same");
                        }
                        if (attackmove.Value.AttackTargetType == AttackTargetType.RivalCity)
                        {
                            sb.AppendLine($"The rival city was replaced with one of my own cities on the board");
                            if (attackmove.Value.ConqueredWonder != null) sb.AppendLine($"As a result of this victory, I took control of the {attackmove.Value.ConqueredWonder.Name} world wonder and all of its bonuses");
                        }
                        if (attackmove.Value.AttackTargetType == AttackTargetType.RivalCapitalCity)
                        {
                            if (attackmove.Value.ConqueredWonder != null)
                            {
                                sb.AppendLine($"As a result of this victory, I took control of the {attackmove.Value.ConqueredWonder.Name} world wonder and all of its bonuses");
                                sb.AppendLine($"The wonder token was placed under my strongest free city");
                            }
                            sb.AppendLine($"I recieved 2 trade token for this victory which I assigned to my {moveState.SmallestTradeTokenPileType} focus slot");
                            sb.AppendLine($"The defeated rival also lost 2 trade tokens as a result their capital being sacked");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"As I lost this battle no military trade tokens were comitted to this attack");
                    }
                }
                else
                {
                    sb.AppendLine($"Attack {attackOrdinal} : There were no targets available for this attack, no action was taken");
                }
                attackOrdinal++;
            }
            return sb.ToString();
        }
    }
}
