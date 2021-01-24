namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IEconomyResolverUtility
    {
        void PrimeBaseEconomyState(BotGameStateCache botGameStateCache, int supportedCaravans, int baseMoves);
        void UpdateBaseEconomyGameStateForMove(BotGameStateCache botGameStateService, int supportedCaravans);
        string BuildGeneralisedEconomyMoveSummary(string currentSummary, BotGameStateCache gameState);
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
