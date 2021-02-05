namespace AutoCivilization.Abstractions.ActionSteps
{
    // below only used in economy resolution... so far!
    public interface ICaravanMovementActionRequestStep : IStepAction { }
    public interface ICaravanDestinationInformationRequestStep : IStepAction { }
    public interface ICityStateCaravanDestinationInformationRequestStep : IStepAction { }
    public interface IRivalCityCaravanDestinationInformationRequestStep : IStepAction { }
    public interface IRemoveCaravanActionRequestStep : IStepAction { }
    public interface ICaravanMovementInformationRequestStep : IStepAction { }
    public interface IRemoveAdjacentBarbariansActionRequestStep : IStepAction { }

}
