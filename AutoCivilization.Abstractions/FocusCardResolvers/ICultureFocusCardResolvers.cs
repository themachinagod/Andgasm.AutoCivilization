namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface ICultureResolverUtility
    {
        void PrimeBaseCultureState(BotGameStateCache botGameStateCache, int baseTokens);
        void UpdateBaseGameStateForMove(BotGameStateCache botGameStateService);
        string BuildGeneralisedCultureMoveSummary(string summary);
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
