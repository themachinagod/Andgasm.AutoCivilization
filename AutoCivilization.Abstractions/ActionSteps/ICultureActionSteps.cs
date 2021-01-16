namespace AutoCivilization.Abstractions.ActionSteps
{
    // below only used in culture resolution... so far!
    public interface ITokenPlacementCityAdjacentActionRequest : IStepAction { }
    public interface ITokenPlacementTerritoryAdjacentActionRequest : IStepAction { }
    public interface ITokenPlacementCityAdjacentInformationRequest : IStepAction { }
    public interface ITokenPlacementTerritoryAdjacentInformationRequest : IStepAction { }
    public interface ITokenPlacementNaturalWondersInformationRequest : IStepAction { }
    public interface ITokenPlacementNaturalResourcesInformationRequest : IStepAction { }
    public interface ITokenFlipEnemyActionRequest : IStepAction { }
}
