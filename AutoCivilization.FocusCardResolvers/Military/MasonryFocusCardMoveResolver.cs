using AutoCivilization.Abstractions;
using AutoCivilization.Abstractions.ActionSteps;
using AutoCivilization.Abstractions.FocusCardResolvers;
using AutoCivilization.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCivilization.FocusCardResolvers
{
    public class MasonryFocusCardMoveResolver : FocusCardMoveResolverBase, IMilitaryLevel1FocusCardMoveResolver
    {
        private const int BaseAttackPower = 5;
        private const int BaseAttackRange = 2;
        private const int BaseAttackCount = 2;
        private const int BaseMaxTargetPower = 4;

        private IMilitaryResolverUtility _cultureResolverUtility;

        public MasonryFocusCardMoveResolver(IMilitaryResolverUtility cultureResolverUtility,
                                            IEnemyWithinAttackDistanceInformationRequestStep enemyWithinAttackDistanceInformationRequestStep,
                                            IEnemyTypeToAttackInformationRequestStep enemyTypeToAttackInformationRequestStep,
                                            IEnemyAttackPowerInformationRequestStep enemyAttackPowerInformationRequestStep,
                                            IAttackPrimaryResultActionRequestStep attackPrimaryResultActionRequestStep,
                                            IDefeatedBarbarianActionRequestStep defeatedBarbarianActionRequestStep,
                                            IConquerCityStateInformationRequestStep conquerCityStateActionRequestStep,
                                            IConquerdNaturalWonderInformationRequestStep conquerWorldNaturalInformationRequestStep,
                                            IDefeatedRivalControlTokenActionRequestStep defeatedRivalControlTokenActionRequestStep,
                                            IDefeatedCapitalCityActionRequestStep defeatedCapitalCityActionRequestStep,
                                            IConquerNonCapitalCityActionRequestStep conquerNonCapitalCityActionRequestStep,
                                            IConquerWorldWonderInformationRequestStep conquerWorldWonderInformationRequestStep,
                                            IFailedAttackActionRequestStep failedAttackActionRequestStep, 
                                            ISupplementAttackPowerInformationRequestStep supplementAttackPowerInformationRequestStep) : base()
        {
            _cultureResolverUtility = cultureResolverUtility;

            FocusType = FocusType.Military;
            FocusLevel = FocusLevel.Lvl1;

            var loopSeed = 0;
            for (var attack = 0; attack < BaseAttackCount; attack++)
            {
                _actionSteps.Add(loopSeed, enemyWithinAttackDistanceInformationRequestStep);
                _actionSteps.Add(loopSeed + 1, enemyTypeToAttackInformationRequestStep);
                _actionSteps.Add(loopSeed + 2, enemyAttackPowerInformationRequestStep);
                _actionSteps.Add(loopSeed + 3, attackPrimaryResultActionRequestStep);
                _actionSteps.Add(loopSeed + 4, supplementAttackPowerInformationRequestStep);
                _actionSteps.Add(loopSeed + 5, conquerWorldNaturalInformationRequestStep);
                _actionSteps.Add(loopSeed + 6, conquerWorldWonderInformationRequestStep);
                _actionSteps.Add(loopSeed + 7, defeatedBarbarianActionRequestStep);
                _actionSteps.Add(loopSeed + 8, conquerCityStateActionRequestStep);
                _actionSteps.Add(loopSeed + 9, defeatedRivalControlTokenActionRequestStep);
                _actionSteps.Add(loopSeed + 10, defeatedCapitalCityActionRequestStep);
                _actionSteps.Add(loopSeed + 11, conquerNonCapitalCityActionRequestStep);
                _actionSteps.Add(loopSeed + 12, failedAttackActionRequestStep);
                loopSeed = _actionSteps.Count;
            }
        }

        public override void PrimeMoveState(BotGameState botGameStateService)
        {
            //_moveState = _cultureResolverUtility.CreateBasicCultureMoveState(botGameStateService, BaseCityControlTokens);

            _moveState = new BotMoveState();
            _moveState.BaseAttackPower = BaseAttackPower;
            _moveState.BaseAttackRange = BaseAttackRange;
            _moveState.BaseMaxTargetPower = BaseMaxTargetPower;

            _moveState.ActiveFocusBarForMove = botGameStateService.ActiveFocusBar;
            _moveState.TradeTokensAvailable = new Dictionary<FocusType, int>(botGameStateService.TradeTokens);
            _moveState.BotPurchasedWonders = new List<WonderCardModel>(botGameStateService.BotPurchasedWonders);
            _moveState.AllPurchasedWonders = new List<WonderCardModel>(botGameStateService.BotPurchasedWonders);
            _moveState.CityStatesDiplomacyCardsHeld = new List<CityStateModel>(botGameStateService.CityStateDiplomacyCardsHeld);
            _moveState.ConqueredCityStateTokensHeld = new List<CityStateModel>(botGameStateService.ConqueredCityStateTokensHeld);
            _moveState.ControlledNaturalWonders = new List<string>(botGameStateService.ControlledNaturalWonders);

            _moveState.FriendlyCityCount = botGameStateService.FriendlyCityCount;

            for (int tc = 0; tc < BaseAttackCount; tc++)
            {
                _moveState.AttacksAvailable.Add(tc, new AttackTargetMoveState() );
            }
        }

        public override string UpdateGameStateForMove(BotGameState gameState)
        {
            //_cultureResolverUtility.UpdateBaseCultureGameStateForMove(_moveState, botGameStateService);

            // TODO: 
            // something to do with visited player cities & diplomacy cards
            // update focus bar if free upgrade (no wonder or city built)??
            // update no of battles won/lost (some additional info) 

            gameState.TradeTokens = new Dictionary<FocusType, int>(_moveState.TradeTokensAvailable);
            gameState.BotPurchasedWonders = new List<WonderCardModel>(_moveState.BotPurchasedWonders);
            gameState.ConqueredCityStateTokensHeld = new List<CityStateModel>(_moveState.ConqueredCityStateTokensHeld);
            gameState.CityStateDiplomacyCardsHeld = new List<CityStateModel>(_moveState.CityStatesDiplomacyCardsHeld);
            gameState.ControlledNaturalWonders = new List<string>(_moveState.ControlledNaturalWonders);

            gameState.FriendlyCityCount = _moveState.FriendlyCityCount;
            gameState.ControlledSpaces += _moveState.CityControlTokensPlacedThisTurn;

            _currentStep = -1;
            return BuildMoveSummary();
        }

        private string BuildMoveSummary()
        {
            //return _cultureResolverUtility.BuildGeneralisedCultureMoveSummary(summary, _moveState);

            var summary = "To summarise my move I did the following;\n";
            StringBuilder sb = new StringBuilder(summary);
            sb.AppendLine($"I executed {_moveState.AttacksAvailable.Count} attacks againt enemy targets;");
            var attackOrdinal = 1;
            foreach (var attackmove in _moveState.AttacksAvailable)
            {
                // HACK: currently we dont know what natural wonder was acquired for a specific attack!!
                sb.AppendLine();
                if (attackmove.Value.IsTargetWithinRange)
                {
                    var winLoss = attackmove.Value.BotIsWinning ? "defeated" : "lost in battle to";
                    sb.AppendLine($"Attack {attackOrdinal} : I {winLoss} a {attackmove.Value.AttackTargetType} as follows;");
                    sb.AppendLine($"\tTarget total power: {attackmove.Value.AttackTargetPowerWithoutTradeTokens}");
                    sb.AppendLine($"\tMy base power: {_moveState.BaseAttackPower}");
                    sb.AppendLine($"\tMy dice roll: {attackmove.Value.DiceRollAttackPower}");
                    sb.AppendLine($"\tMy wonder cards bonus: {_moveState.BotPurchasedWonders.Where(x => x.Type == FocusType.Military).Count()}");
                    sb.AppendLine($"\tMy diplomacy cards bonus: {_moveState.CityStatesDiplomacyCardsHeld.Where(x => x.Type == FocusType.Military).Count()}");
                    sb.AppendLine($"\tMy military trade tokens: {_moveState.TradeTokensAvailable[FocusType.Military]}");

                    if (attackmove.Value.BotIsWinning)
                    {
                        sb.AppendLine($"In order to win this battle I had to expend {attackmove.Value.BotSpentMilitaryTradeTokensThisTurn} military trade token(s)");

                        if (attackmove.Value.AttackTargetType == AttackTargetType.Barbarian)
                        {
                            sb.AppendLine($"The barbarian was removed from the board");
                            sb.AppendLine($"I recieved 1 trade token for this victory which I assigned to my {_moveState.SmallestTradeTokenPileType} focus slot");
                        }
                        if (attackmove.Value.AttackTargetType == AttackTargetType.RivalControlToken)
                        {
                            sb.AppendLine($"The rival control token was replaced with one of my own on the board");
                            if (attackmove.Value.HasStolenNaturalWonder) sb.AppendLine($"As the battle took place on the { _moveState.ControlledNaturalWonders.LastOrDefault()} natural wonder, I have now taken control of this natural wonder token and any bnous it provides");
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
                            sb.AppendLine($"I recieved 2 trade token for this victory which I assigned to my {_moveState.SmallestTradeTokenPileType} focus slot");
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
