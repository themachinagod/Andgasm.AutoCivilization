namespace AutoCivilization.Abstractions.Models
{
    public class BreakthroughModel
    {
        public FocusCardModel ReplacedFocusCard { get; }
        public FocusCardModel UpgradedFocusCard { get; }

        public BreakthroughModel(FocusCardModel replaced, FocusCardModel upgrade)
        {
            ReplacedFocusCard = replaced;
            UpgradedFocusCard = upgrade;
        }
    }
}
