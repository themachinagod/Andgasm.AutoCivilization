using AutoCivilization.Abstractions.TechnologyResolvers;
using AutoCivilization.Console;

namespace AutoCivilization.Abstractions.FocusCardResolvers
{
    public interface IIndustryResolverUtility
    {
        BotMoveState CreateBasicIndustryMoveState(BotGameState botGameStateCache, int baseProduction, int baseDistance);
        void UpdateBaseIndustryGameStateForMove(BotMoveState movesState, BotGameState botGameStateService);
        string BuildGeneralisedIndustryMoveSummary(string currentSummary, BotMoveState movesState);
    }

    public interface IIndustryLevel1FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IIndustryLevel2FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IIndustryLevel3FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }

    public interface IIndustryLevel4FocusCardMoveResolver : IFocusCardMoveResolver
    {
    }
}
