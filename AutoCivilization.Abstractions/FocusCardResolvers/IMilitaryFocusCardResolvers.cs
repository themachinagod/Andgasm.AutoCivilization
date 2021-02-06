using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IMilitaryResolverUtility
    {
        BotMoveState CreateBasicMilitaryMoveState(BotGameState botGameStateCache, int baseRange, int basePower, int baseMaxTargetrPower, int noOfAttacks, int baseReinforcments, int baseReinforcemenstCost, int baseBarbarianBonus);
        void UpdateBaseMilitaryGameStateForMove(BotMoveState movesState, BotGameState botGameStateService, int attackIndex);
        string BuildGeneralisedMilitaryMoveSummary(string currentSummary, BotMoveState movesState);

    }

    public interface IMilitaryLevel1FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IMilitaryLevel2FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IMilitaryLevel3FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IMilitaryLevel4FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }
}
