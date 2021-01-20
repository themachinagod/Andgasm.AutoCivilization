namespace AutoCivilization.Abstractions.Models
{
    // DBr: I think this is a good candidate for the record keyword...
    //      this should only be initialised at construction and should NEVER change its internal state

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
