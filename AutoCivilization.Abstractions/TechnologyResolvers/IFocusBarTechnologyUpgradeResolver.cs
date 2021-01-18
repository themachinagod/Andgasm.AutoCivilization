namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface IFocusBarTechnologyUpgradeResolver
    {
        (FocusCardModel OldTech, FocusCardModel NewTech) UpgradeFocusBarsLowestTechLevel(FocusType focusType, FocusLevel levelBarrierHit);
    }

}
