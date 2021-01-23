using System.Collections.Generic;

namespace AutoCivilization.Abstractions
{
    public interface IBotRoundStateCache
    {
        FocusType AdditionalFocusTypeToExecuteOnFocusBar { get; set; }
        bool ShouldExecuteAdditionalFocusCard { get; set; }
    }
}
