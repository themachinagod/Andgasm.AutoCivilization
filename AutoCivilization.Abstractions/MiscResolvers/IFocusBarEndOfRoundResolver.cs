namespace AutoCivilization.Abstractions.MiscResolvers
{
    public interface IFocusBarResetResolver
    {
        FocusBarModel ResetFocusBarForNextMove(FocusBarModel activeFocusBar);
        FocusBarModel ResetFocusBarForSubMove(FocusBarModel activeFocusBar, FocusType focusType);
    }
}
