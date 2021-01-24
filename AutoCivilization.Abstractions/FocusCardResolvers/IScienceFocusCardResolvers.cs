using AutoCivilization.Abstractions.TechnologyResolvers;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IScienceResolverUtility
    {
        void PrimeBaseScienceState(BotGameStateCache botGameStateCache, int basePoints);
        TechnologyUpgradeResponse UpdateBaseScienceGameStateForMove(BotGameStateCache botGameStateService);
        string BuildGeneralisedScienceMoveSummary(string currentSummary, TechnologyUpgradeResponse techResponse);
    }
    public interface IScienceLevel1FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IScienceLevel2FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IScienceLevel3FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IScienceLevel4FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }
}
