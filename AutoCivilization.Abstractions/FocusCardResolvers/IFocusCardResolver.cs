using AutoCivilization.Abstractions.ActionSteps;

namespace AutoCivilization.Abstractions
{
    public interface IFocusCardResolver
    {
        FocusType FocusType { get; set; }
        FocusLevel FocusLevel { get; set; }
        bool HasMoreSteps { get; }

        void InitialiseMoveState();
        IStepAction GetNextStep();
        void Resolve();
    }
}
