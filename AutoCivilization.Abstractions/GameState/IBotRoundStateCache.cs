using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    public enum SubMoveExecutionPhase
    {
        PrePrimaryReset,
        PostPrimaryReset
    }

    public interface IBotRoundStateCache
    {
        List<SubMoveConfiguration> SubMoveConfigurations { get; set; }
    }

    public class SubMoveConfiguration
    {
        public FocusType AdditionalFocusTypeToExecuteOnFocusBar { get; set; }
        public SubMoveExecutionPhase SubMoveExecutionPhase { get; set; }
        public bool ShouldResetSubFocusCard { get; set; }
    }
}
