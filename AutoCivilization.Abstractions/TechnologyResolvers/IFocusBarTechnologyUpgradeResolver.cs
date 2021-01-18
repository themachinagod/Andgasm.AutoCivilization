namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface IFocusBarTechnologyUpgradeResolver
    {
        (FocusCardModel OldTech, FocusCardModel NewTech) UpgradeFocusBarsLowestTechLevel(FocusLevel levelBarrierHit);
    }

}
