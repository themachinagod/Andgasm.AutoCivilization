using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IScienceResolverUtility
    {
        BotMoveState CreateBasicScienceMoveState(BotGameStateCache botGameStateCache, int basePoints);
        TechnologyUpgradeResponse UpdateBaseScienceGameStateForMove(BotMoveState moveState, BotGameStateCache botGameStateService);
        string BuildGeneralisedScienceMoveSummary(string currentSummary, TechnologyUpgradeResponse upgradeResponse, BotMoveState moveState);
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
