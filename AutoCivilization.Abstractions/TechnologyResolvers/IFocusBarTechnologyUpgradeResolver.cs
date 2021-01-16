namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface IFocusBarTechnologyUpgradeResolver
    {
        void UpgradeFocusBarsLowestTechLevel(FocusType focusType, FocusLevel levelBarrierHit);
    }

}
