using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IIndustryResolverUtility
    {
        BotMoveState CreateBasicIndustryMoveState(BotGameState botGameStateCache, int baseProduction);
        FocusBarUpgradeResponse UpdateBaseIndustryGameStateForMove(BotMoveState movesState, BotGameState botGameStateService);
        string BuildGeneralisedIndustryMoveSummary(string currentSummary, BotMoveState movesState);
    }

    public interface IIndustryLevel1FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IIndustryLevel2FocusCardResolver : IFocusCardMoveResolver
    {
    }

    public interface IIndustryLevel3FocusCardResolver : IFocusCardMoveResolver
    {
    }

    public interface IIndustryLevel4FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }
}
