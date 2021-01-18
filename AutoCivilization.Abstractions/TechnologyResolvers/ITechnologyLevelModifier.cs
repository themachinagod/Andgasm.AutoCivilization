namespace AutoCivilization.Abstractions.TechnologyResolvers
{
    public interface ITechnologyLevelModifier
    {
        bool EncounteredBreakthrough { get; set; }
        FocusCardModel ReplacedFocusCard { get; set; }
        FocusCardModel UpgradedFocusCard { get; set; }

        void IncrementTechnologyLevel(int techPoints);
    }
}
