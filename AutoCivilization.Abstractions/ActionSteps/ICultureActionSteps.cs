namespace AutoCivilization.Abstractions.ActionSteps
{
    // below only used in culture resolution... so far!
    public interface ITokenPlacementCityAdjacentActionRequestStep : IStepAction { }
    public interface ITokenPlacementTerritoryAdjacentActionRequestStep : IStepAction { }
    public interface ITokenPlacementCityAdjacentInformationRequestStep : IStepAction { }
    public interface ITokenPlacementTerritoryAdjacentInformationRequest : IStepAction { }
    public interface ITokenPlacementNaturalWondersInformationRequestStep : IStepAction { }
    public interface ITokenPlacementNaturalResourcesInformationRequestStep : IStepAction { }
    public interface ITokenFlipEnemyActionRequestStep : IStepAction { }
}
