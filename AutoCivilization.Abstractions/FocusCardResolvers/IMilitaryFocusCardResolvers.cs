using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IMilitaryResolverUtility
    {
        BotMoveState CreateBasicMilitaryMoveState(BotGameState botGameStateCache, int baseRange, int basePower, int noOfAttacks);
        void UpdateBaseMilitaryGameStateForMove(BotMoveState movesState, BotGameState botGameStateService, int attackIndex);
        string BuildGeneralisedMilitaryMoveSummary(string currentSummary, BotGameState gameState, BotMoveState movesState);

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
