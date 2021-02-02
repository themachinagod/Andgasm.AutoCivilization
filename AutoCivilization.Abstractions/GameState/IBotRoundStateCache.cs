using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    public interface IBotRoundStateCache
    {
        List<SubMoveConfiguration> SubMoveConfigurations { get; set; }
    }

    public class SubMoveConfiguration
    {
        public FocusType AdditionalFocusTypeToExecuteOnFocusBar { get; set; }
        public bool ShouldResetSubFocusCard { get; set; }
        public bool HasCompletedExecution { get; set; }
    }
}
