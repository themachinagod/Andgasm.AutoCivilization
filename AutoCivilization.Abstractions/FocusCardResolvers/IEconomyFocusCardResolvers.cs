using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IEconomyResolverUtility
    {
        BotMoveState CreateBasicEconomyMoveState(BotGameStateCache botGameStateCache, int supportedCaravans, int baseMoves);
        void UpdateBaseEconomyGameStateForMove(BotMoveState movesState, BotGameStateCache botGameStateService, int supportedCaravans);
        string BuildGeneralisedEconomyMoveSummary(string currentSummary, BotGameStateCache gameState, BotMoveState movesState);
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
