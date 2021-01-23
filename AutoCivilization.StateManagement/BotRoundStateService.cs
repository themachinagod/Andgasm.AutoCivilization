using AutoCivilization.Abstractions;
using System.Collections.Generic;

namespace AutoCivilization.Console
{
    public class BotRoundStateCache : IBotRoundStateCache
    {
        public FocusType AdditionalFocusTypeToExecuteOnFocusBar { get; set; }
        public bool ShouldExecuteAdditionalFocusCard { get; set; }
    }
}
