namespace AutoCivilization.Abstractions.ActionSteps
{
    // below only used in military resolution... so far!

    public interface IEnemyWithinAttackDistanceInformationRequestStep : IStepAction { }
    public interface IEnemyTypeToAttackInformationRequestStep : IStepAction { }
    public interface IEnemyAttackPowerInformationRequestStep : IStepAction { }
    public interface IAttackPrimaryResultActionRequestStep : IStepAction { }
    public interface IDefeatedBarbarianActionRequestStep : IStepAction { }
    public interface IConquerCityStateActionRequestStep : IStepAction { }
    public interface IDefeatedRivalControlTokenActionRequestStep : IStepAction { }
    public interface IDefeatedCapitalCityActionRequestStep : IStepAction { }
    public interface IConquerNonCapitalCityActionRequestStep : IStepAction { }
    public interface IReinforceFriendlyControlTokensActionRequest : IStepAction { }
    public interface IReinforceFriendlyControlTokensInformationRequest : IStepAction { }
    public interface ISupplementAttackPowerInformationRequestStep : IStepAction { }
    public interface IFailedAttackActionRequestStep : IStepAction { }
}
