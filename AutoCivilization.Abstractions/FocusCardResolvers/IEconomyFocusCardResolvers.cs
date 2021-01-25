using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IEconomyResolverUtility
    {
        BotMoveStateCache CreateBasicEconomyMoveState(BotGameStateCache botGameStateCache, int supportedCaravans, int baseMoves);
        void UpdateBaseEconomyGameStateForMove(BotMoveStateCache movesState, BotGameStateCache botGameStateService, int supportedCaravans);
        string BuildGeneralisedEconomyMoveSummary(string currentSummary, BotGameStateCache gameState, BotMoveStateCache movesState);
    }
    public interface IEconomyLevel1FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IEconomyLevel2FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IEconomyLevel3FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IEconomyLevel4FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }
}
