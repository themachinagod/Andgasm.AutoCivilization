namespace AutoCivilization.Abstractions
{
    public interface IBotMoveStateService
    {
        int CultureTokensAvailable { get; set; }
        int CultureTokensUsedThisTurn { get; set; }

        int ScienceTokensAvailable { get; set; }
        int ScienceTokensUsedThisTurn { get; set; }

        int BaseCityControlTokensToBePlaced { get; set; }
        int BaseTerritoryControlTokensToBePlaced { get; set; }

        int CityControlTokensPlaced { get; set; }
        int TerritroyControlTokensPlaced { get; set; }

        int NaturalWonderTokensControlled { get; set; }
        int NaturalResourceTokensControlled { get; set; }

        int BaseTechnologyIncrease { get; set; }
        int TechnologyLevelIncrease { get; set; }
    }
}
