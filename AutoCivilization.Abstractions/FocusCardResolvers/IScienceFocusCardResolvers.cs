using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IScienceResolverUtility
    {
        BotMoveStateCache CreateBasicScienceMoveState(BotGameStateCache botGameStateCache, int basePoints);
        TechnologyUpgradeResponse UpdateBaseScienceGameStateForMove(BotMoveStateCache moveState, BotGameStateCache botGameStateService);
        string BuildGeneralisedScienceMoveSummary(string currentSummary, TechnologyUpgradeResponse upgradeResponse, BotMoveStateCache moveState);
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
