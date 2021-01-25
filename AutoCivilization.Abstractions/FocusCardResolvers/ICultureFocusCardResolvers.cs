using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface ICultureResolverUtility
    {
        BotMoveState CreateBasicCultureMoveState(BotGameStateCache botGameStateCache, int baseTokens);
        void UpdateBaseCultureGameStateForMove(BotMoveState movesState, BotGameStateCache botGameStateService);
        string BuildGeneralisedCultureMoveSummary(string currentSummary, BotMoveState movesState);
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
