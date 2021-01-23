namespace AutoCivilization.Abstractions.MiscResolvers
{
    public interface IFocusBarEndOfRoundResolver
    {
        FocusBarModel ResetFocusBarForNextMove(FocusBarModel activeFocusBar);
    }
}
