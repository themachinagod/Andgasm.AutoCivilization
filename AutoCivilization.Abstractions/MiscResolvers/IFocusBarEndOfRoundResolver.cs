namespace AutoCivilization.Abstractions.MiscResolvers
{
    public interface IFocusBarResetResolver
    {
        FocusBarModel ResetFocusBarForNextMove(FocusBarModel activeFocusBar);
    }
}
