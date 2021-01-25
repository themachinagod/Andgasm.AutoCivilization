using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface ICultureResolverUtility
    {
        BotMoveStateCache CreateBasicCultureMoveState(BotGameStateCache botGameStateCache, int baseTokens);
        void UpdateBaseCultureGameStateForMove(BotMoveStateCache movesState, BotGameStateCache botGameStateService);
        string BuildGeneralisedCultureMoveSummary(string currentSummary, BotMoveStateCache movesState);
    }

    public interface ICultureLevel1FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface ICultureLevel2FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface ICultureLevel3FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface ICultureLevel4FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }
}
